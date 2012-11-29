using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisTimelinerPlugin
{
    class Core
    {
        #region логика конструктора - синглтона
        private static Core self;
        private Core()
        {

        }
        public static Core Self
        {
            get
            {
                if (self == null)
                {
                    self = new Core();
                }
                return self;
            }
        }
        #endregion

        static Document nDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        static Autodesk.Navisworks.Api.DocumentParts.IDocumentTimeliner Itimeliner = nDoc.Timeliner;
        static DocumentTimeliner timeliner = (DocumentTimeliner)Itimeliner;
        static TimelinerTask RootTimelinerTask;
        
        /// <summary>
        ////Проверка наличия тасков в таймлайнере и перезаполнение структуры массива тасков в программе.
        /// </summary>
        public bool AreTasksAccessible()
        {
            if (timeliner.Tasks.Count != 0)
            {
                this.tasks.Clear();
                RootTimelinerTask = timeliner.Tasks[0] as TimelinerTask;
                this.getChildren(RootTimelinerTask);
                return true;
            }
            else
            {
                MessageBox.Show("Отсутствуют задания в Timeliner", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        /// <summary>
        /// Рекурсивный обход дерева тасков и сбор всех тасков в один массив.
        /// </summary>
        void getChildren(TimelinerTask task)
        {
            foreach (TimelinerTask child in task.Children)
            {
                Collection<int> index = timeliner.TaskCreateIndexPath(child);
                this.tasks.Add(new TaskContainer(child.DisplayName, index));
                getChildren(child);
            }
        }


        public List<TaskContainer> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                value = tasks;
            }
        }//массив тасков проекта.
        List<TaskContainer> tasks = new List<TaskContainer>(); 
        public CurrentTimelinerTask currentTimelinerTask = new CurrentTimelinerTask();//Контейнер для хранения обрабатываемого в данный момент таска.
        SerializableDataHolder DataHolder = new SerializableDataHolder();//массив для сохранения обработанных тасков в файл.


        public void WriteTaskToTimeliner(Collection<int> index, string setName = null)
        {
            try
            {
                if (setName != null)
                {
                    SelectionSourceCollection collection = Core.Self.getSelectionSourceByName(setName);
                    if (collection.Count != 0)
                    {
                        TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                        task.Selection.CopyFrom(collection);
                        GroupItem foo = timeliner.TaskResolveIndexPath(index).Parent;
                        int id = foo.Children.IndexOfDisplayName(task.DisplayName);
                        timeliner.TaskEdit(foo, id, task);
                    }
                }
                else
                {
                    TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                    task.Selection.Clear();
                    GroupItem foo = timeliner.TaskResolveIndexPath(index).Parent;
                    int id = foo.Children.IndexOfDisplayName(task.DisplayName);
                    timeliner.TaskEdit(foo, id, task);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public void WriteTaskToTimeliner(TaskContainer taskC, string setName = null)
        {
            WriteTaskToTimeliner(taskC.Index, setName);
        }

        
        /// <summary>
        /// Убирает селекшны у всех тасков в таймлайнере.
        /// </summary>
        public void ClearSelections()
        {
            foreach (TaskContainer taskC in tasks)
            {
                WriteTaskToTimeliner(taskC);
            }
        }

        public void ClearSelection(TaskContainer taskC)
        {
            WriteTaskToTimeliner(taskC);
        }
        
        /// <summary>
        /// Поиск имени списка выбора у таска
        /// </summary>
        /// <param name="task">Таск, к которому привязан селекшн, имя которого надо найти</param>
        /// <returns>Имя списка выбора, который выбирает указанный селекшн</returns>
        public string findSelectionSetName(TimelinerTask task)
        {
            if (task.Selection.HasSelectionSources == true)
            {
                SelectionSource sSource = task.Selection.SelectionSources[0];
                SavedItem sSet = nDoc.SelectionSets.ResolveSelectionSource(sSource);
                return sSet.DisplayName;
            }
            return null;
        }

        /// <summary>
        /// Возвращает выборку элементов модели по имени списка выбора.
        /// </summary>
        /// <param name="Name">Имя списка выбора (Selextion Set), для которого нужно получить выборку.</param>
        public SelectionSourceCollection getSelectionSourceByName(string Name)
        {
            SelectionSourceCollection result = new SelectionSourceCollection();
            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                if (item.DisplayName == Name)
                {
                    SelectionSource source = nDoc.SelectionSets.CreateSelectionSource(item);
                    result.Add(source);
                    return result;
                }
            }
            return result;
        }
               
        public void SaveTasks()
        {
            DataHolder.Clear();
            foreach(TaskContainer tc in this.tasks)
            {
                Collection<int> index = tc.Index;
                string sel = findSelectionSetName(tc.Task);
                DataHolder.Add(index, sel);
            }
            Serializer.serialize(DataHolder);
        }
        
        /// <summary>
        /// Загрузка пар "порядковый номер" - "Selection set name" из бинарного файла и назначение селекшнов таскам.
        /// </summary>
        public void LoadTasksAssocFromFile()
        {
            DataHolder = Serializer.deserialize();

            if (DataHolder != null)
            {
                ClearSelections();
                foreach (KeyValuePair<Collection<int>, string> pair in DataHolder.Data)
                {
                    this.WriteTaskToTimeliner(pair.Key, pair.Value);
                }
            }
        }
       
        /// <summary>
        /// Показывает на модели только выборку элементов, остальное делает прозрачным.
        /// </summary>
        /// <param name="task">Таск, выборка которого отображается на модели.</param>
        public void hideAllExcepTimelinerTaskSelection(TimelinerTask task, TextBox textbox = null)
        {
            try
            {
                if (!task.Selection.IsClear)
                {
                    ModelItemCollection collection = task.Selection.GetSelectedItems(nDoc);
                    nDoc.CurrentSelection.SelectAll();
                    nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 0.99);
                    nDoc.CurrentSelection.Clear();
                    nDoc.Models.OverridePermanentTransparency(collection, 0);
                }
                else
                {
                    MakeAllModelItemsVisible();
                    if(textbox != null)
                        textbox.Text += " /не назначена выбрка/";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }        

        public void MakeAllModelItemsVisible()
        {
            nDoc.CurrentSelection.SelectAll();
            nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 0);
            nDoc.CurrentSelection.Clear();
        }

    }
}
