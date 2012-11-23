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
                this.tasks.Add(new TaskContainer(child, index));
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
        public CurrenTimelinerTask currenTimelinerTask = new CurrenTimelinerTask();//Контейнер для хранения обрабатываемого в данный момент таска.
        SerializableDataHolder DataHolder;//массив для сохранения обработанных тасков в файл.


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
        /// Поиск имени списка выбора по имеющемуся таску
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
            foreach (TimelinerTask task in RootTimelinerTask.Children)
            {
                string name = task.DisplayName;
                string sel = findSelectionSetName(task);
                DataHolder.Add(name, sel);
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
                foreach (KeyValuePair<string, string> pair in DataHolder.Data)
                {
                    if (pair.Value != null)
                    {
                        SelectionSourceCollection selectionSource = this.getSelectionSourceByName(pair.Value);
                        int taskIndex = RootTimelinerTask.Children.IndexOfDisplayName(pair.Key);
                        if (taskIndex != -1)
                        {
                            TimelinerTask task = ((TimelinerTask)RootTimelinerTask.Children[taskIndex]).CreateCopy();
                            task.Selection.CopyFrom(selectionSource);
                            timeliner.TaskEdit(RootTimelinerTask, taskIndex, task);
                        }
                    }
                }
            }
        }
       
        /// <summary>
        /// Показывает на модели только выборку элементов, остальное делает прозрачным.
        /// </summary>
        /// <param name="task">Таск, выборка которого отображается на модели.</param>
        public void hideAllExcepTimelinerTaskSelection(TimelinerTask task, TextBox textbox)
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
