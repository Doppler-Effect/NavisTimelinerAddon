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
        
        /// <summary>
        ////Проверка наличия тасков в таймлайнере и перезаполнение массива тасков в программе.
        /// </summary>
        public bool TasksOK()
        {
            if (timeliner.Tasks.Count != 0)
            {
                this.tasks.Clear();
                TimelinerTask Root = timeliner.Tasks[0] as TimelinerTask;
                this.getChildren(Root);
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
        void getChildren(TimelinerTask rootTask)
        {
            foreach (TimelinerTask child in rootTask.Children)
            {
                Collection<int> index = timeliner.TaskCreateIndexPath(child);
                TaskContainer Task = new TaskContainer(child.DisplayName, index);
                this.tasks.Add(Task);
                getChildren(Task);
            }
        }
        void getChildren(TaskContainer parentContainer)
        {
            foreach (TimelinerTask child in parentContainer.Task.Children)
            {
                Collection<int> index = timeliner.TaskCreateIndexPath(child);
                TaskContainer childContainer = new TaskContainer(child.DisplayName, index);
                this.tasks.Add(childContainer);
                parentContainer.Children.Add(childContainer);
                getChildren(childContainer);
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
        }
        List<TaskContainer> tasks = new List<TaskContainer>(); //массив тасков проекта.      

        /// <summary>
        /// Записывает значение в Timeliner. В случае пустого селекшн сета - очищает таск от прикреплённого набора.
        /// </summary>
        /// <param name="index">Коллекция int-ов, описывающая путь к элементу в иерархии таймлайнера.</param>
        /// <param name="setName">Имя прикрепляемого набора.</param>
        public void WriteTaskToTimeliner(Collection<int> index, string setName = null)
        {
            try
            {
                if (setName != null) //прикрепление к таску набора
                {
                    SelectionSourceCollection collection = Core.Self.GetSelectionSourceByName(setName);
                    if (collection.Count != 0)
                    {
                        TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                        task.Selection.CopyFrom(collection);
                        GroupItem parent = timeliner.TaskResolveIndexPath(index).Parent;
                        int id = parent.Children.IndexOfDisplayName(task.DisplayName);
                        timeliner.TaskEdit(parent, id, task);
                    }
                }
                else //удаление прикреплённого набора
                {
                    TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                    task.Selection.Clear();
                    GroupItem parent = timeliner.TaskResolveIndexPath(index).Parent;
                    int id = parent.Children.IndexOfDisplayName(task.DisplayName);
                    timeliner.TaskEdit(parent, id, task);
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
        public void ClearAllSelections()
        {
            foreach (TaskContainer taskC in tasks)
            {
                if(!taskC.Task.Selection.IsClear)
                    WriteTaskToTimeliner(taskC);
            }
        }

        /// <summary>
        /// Убирает селекшны у конкретного таска.
        /// </summary>
        public void ClearSelection(TaskContainer taskC)
        {
            WriteTaskToTimeliner(taskC);
        }
        
        /// <summary>
        /// Поиск имени набора, прикреплённого к таску.
        /// </summary>
        /// <param name="task">Таск, к которому привязан искомый набор</param>
        /// <returns>Имя списка выбора, который выбирает указанный селекшн</returns>
        public string FindSelectionSetName(TimelinerTask task)
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
        /// Возвращает выборку элементов модели по имени списка выбора. Если такой выборки нет - возвращает пустую коллекцию (обрабатывается уже в методах записи в timeliner).
        /// </summary>
        /// <param name="Name">Имя списка выбора (Selection Set), для которого нужно получить выборку.</param>
        public SelectionSourceCollection GetSelectionSourceByName(string Name)
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
          
        /// <summary>
        /// Сохранение данных из Timeliner в файл.
        /// </summary>
        public void SaveTasksToFile()
        {
            SerializableDataHolder DataHolder = new SerializableDataHolder();
            foreach(TaskContainer tc in this.tasks)
            {
                Collection<int> index = tc.Index;
                string sel = FindSelectionSetName(tc.Task);
                DataHolder.Add(index, sel);
            }
            Serializer.serialize(DataHolder);
        }
        
        /// <summary>
        /// Загрузка пар "порядковый номер" - "Selection set name" из бинарного файла и назначение селекшнов таскам.
        /// </summary>
        public void LoadTasksAssocFromFile()
        {
            SerializableDataHolder DataHolder = new SerializableDataHolder();
            DataHolder = Serializer.deserialize();

            if (DataHolder != null)
            {
                ClearAllSelections();
                foreach (KeyValuePair<Collection<int>, string> pair in DataHolder.Data)
                {
                    if(pair.Value != null)
                        this.WriteTaskToTimeliner(pair.Key, pair.Value);
                }
            }
        }
       
        /// <summary>
        /// Показывает на модели только выборку элементов, остальное прячет.
        /// </summary>
        /// <param name="task">Таск, выборка которого отображается на модели.</param>
        public void HideAllExceptTaskSelection(TimelinerTask task, TextBox textbox = null)
        {
            try
            {
                if (!task.Selection.IsClear)
                {
                    #region Способ из мануала, работает медленно и неправильно
                    //ModelItemCollection hidden = new ModelItemCollection();
                    //ModelItemCollection visible = new ModelItemCollection();
                    //ModelItemCollection taskItems = task.Selection.GetSelectedItems(nDoc);
                                        
                    //foreach (ModelItem item in taskItems)
                    //{
                    //if (item.AncestorsAndSelf != null)
                    //    visible.AddRange(item.AncestorsAndSelf);
                    //if (item.Descendants != null)
                    //    visible.AddRange(item.Descendants);
                    //}

                    ////mark as invisible all the siblings of the visible items as well as the visible items
                    //foreach (ModelItem toShow in visible)
                    //{
                    //    if (toShow.Parent != null)
                    //    {
                    //        hidden.AddRange(toShow.Parent.Children);
                    //    }
                    //}

                    ////remove the visible items from the collection
                    //foreach (ModelItem toShow in visible)
                    //{
                    //    hidden.Remove(toShow);
                    //}                   

                    ////hide the remaining items
                    //nDoc.Models.SetHidden(hidden, true);
                    #endregion

                    MakeAllModelItemsVisible();
                    ModelItemCollection taskItems = task.Selection.GetSelectedItems(nDoc);
                    taskItems.Invert(nDoc);

                    //nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 1);
                    nDoc.Models.SetHidden(taskItems, true);                    
                    //nDoc.Models.OverridePermanentTransparency(task.Selection.GetSelectedItems(nDoc), 0);
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

        /// <summary>
        /// Делает видимой ВСЮ модель.
        /// </summary>
        public void MakeAllModelItemsVisible()
        {
            nDoc.CurrentSelection.SelectAll();
            //nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 0);
            nDoc.Models.ResetAllHidden();
            nDoc.CurrentSelection.Clear();
        }

        /// <summary>
        /// Возвращает уникальный идентификатор элемента
        /// </summary>
        public string GetElementUniqueID(ModelItem item)
        {
            string Stable_ID = null;
            try
            {
                PropertyCategory idCategory = item.PropertyCategories.FindCategoryWithStableId();
                if (idCategory != null)
                {
                    if (idCategory.HasInt64StableId)
                        Stable_ID = idCategory.GetInt64StableId().ToString();
                    if (idCategory.HasStringStableId)
                        Stable_ID = idCategory.GetStringStableId();
                }
                return Stable_ID;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return Stable_ID;
            }
        }
    }
}
