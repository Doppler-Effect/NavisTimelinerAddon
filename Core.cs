﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
            //Инициализация базы данных для хранения вводимых пользователем значений.
            filesDB = new FilesDB.DataBase(projectName + ".base");
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
        static string projectName
        {
            get
            {
                string filepath = nDoc.FileName;
                string[] tokens = filepath.Split('\\');
                return tokens.Last();
            }
        }
        public FilesDB.DataBase filesDB;
        public inputForm activeInputForm;
        public UIform activeUIForm;

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
        void getChildren(SavedItem item, List<SavedItem> list)
        {
            if (item.IsGroup)
            {
                foreach (SavedItem childItem in ((GroupItem)item).Children)
                {
                    list.Add(childItem);
                    getChildren(childItem, list);
                }
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
        private List<TaskContainer> tasks = new List<TaskContainer>(); //массив тасков проекта.   
        public List<SavedItem> AllSelectionSets
        {
            get
            {
                List<SavedItem> list = new List<SavedItem>();
                foreach (SavedItem item in nDoc.SelectionSets.Value)
                {
                    list.Add(item);
                    getChildren(item, list);
                }
                return list;
            }
        }

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
                    SelectionSourceCollection collection = Core.Self.GetSelectionSetByName(setName);
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
        /// Записывает значение, вводимые пользователем в таск.
        /// </summary>
        /// <param name="index">Индекс таска, в который будет вестись запись</param>
        /// <param name="value">Прогресс выполнения</param>
        /// <param name="units">Единицы измерения</param>
        /// <param name="maxValue">Макс.возможное значение прогресса выполнения</param>
        /// <param name="percentage">Рассчитанный процент выполнения</param>
        /// <returns></returns>
        public bool WriteCompletionToTask(Collection<int> index, string value, string units, string maxValue, double percentage)
        {
            try
            {
                TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                GroupItem parent = timeliner.TaskResolveIndexPath(index).Parent;
                int id = parent.Children.IndexOfDisplayName(task.DisplayName);

                task.SetUserFieldByIndex(0, value);
                task.SetUserFieldByIndex(1, units);
                task.SetUserFieldByIndex(2, maxValue);
                task.ProgressPercent = percentage;

                timeliner.TaskEdit(parent, id, task);
                return true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
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
        /// <param name="Task">Таск, к которому привязан искомый набор</param>
        /// <returns>Имя списка выбора, который выбирает указанный селекшн</returns>
        public string FindSelectionSetName(TimelinerTask Task)
        {
            if (Task.Selection.HasSelectionSources)
            {
                SelectionSource sSource = Task.Selection.SelectionSources[0];
                SavedItem sSet = nDoc.SelectionSets.ResolveSelectionSource(sSource);
                return sSet.DisplayName;
            }
            return null;
        }

        /// <summary>
        /// Возвращает выборку элементов модели по имени списка выбора. Если такой выборки нет - возвращает пустую коллекцию.
        /// </summary>
        /// <param name="Name">Имя списка выбора (Selection Set), для которого нужно получить выборку.</param>
        public SelectionSourceCollection GetSelectionSetByName(string Name)
        {
            SelectionSourceCollection result = new SelectionSourceCollection();
            foreach (SavedItem item in this.AllSelectionSets)
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
        public void LoadTasksFromFile()
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
        /// Пересчитывает данные из БД и сохраняет в таск.
        /// </summary>
        public void CalculateTaskSummaryProgress(TimelinerTask Task)
        {
            if (Task.Selection.HasSelectionSources)
            {
                ModelItemCollection items = Task.Selection.GetSelectedItems(nDoc);
                List<double> values = new List<double>();
                foreach (ModelItem item in items.DescendantsAndSelf)
                {
                    string UniqueID = Core.Self.GetElementUniqueID(item);
                    Dictionary<string, string> qResult = Core.Self.filesDB.Select(UniqueID);
                    if (qResult != null)
                    {
                        double value = qResult["Value"].ToDouble();
                        double maxValue = qResult["MaxValue"].ToDouble();
                        if (maxValue != 0)
                        {
                            values.Add(value / maxValue);
                        }
                    }
                }

                //среднее арифметическое
                if (values.Count > 0)
                {
                    double Sum = 0;
                    foreach (double val in values)
                    {
                        Sum += val;
                    }
                    double percent = Sum * 100 / values.Count;
                    string result = percent.ToString("G", System.Globalization.CultureInfo.CurrentCulture);
                    this.WriteCompletionToTask(timeliner.TaskCreateIndexPath(Task), result, "%", null, percent);
                }
            }
        }
        
        /// <summary>
        /// Удаляет из базы все данные об элементах, присвоенных таску.
        /// </summary>
        /// <param name="Task"></param>
        public void ClearTaskItemsFromDB(TimelinerTask Task)
        {
            if (Task.Selection.HasSelectionSources)
            {
                ModelItemCollection items = Task.Selection.GetSelectedItems(nDoc);
                foreach (ModelItem item in items)
                {
                    string UniqueID = Core.Self.GetElementUniqueID(item);
                    this.filesDB.Remove(UniqueID);
                }
            }
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

        public double? GetVolumeFromModelItem(ModelItem Item)
        {
            double? result = null;
            try
            {

            }
            catch
            {

            }


            return result;
        }

        /// <summary>
        /// Заполняет TreeView структорой тасков.
        /// </summary>
        /// <param name="treeView">Элемент TreeView, который будет наполнен данными</param>
        /// <param name="highlight">Выделять ли элементы, у которых есть назначенный SelectionSet, назначены данные итд</param>
        public void FillTreeViewWithTasks(TreeView treeView, bool highlight = true)
        {
            treeView.BeginUpdate();
            foreach (TaskContainer tc in Core.Self.Tasks)
            {
                if (tc.HierarchyLevel == TaskContainer.MinHierarchyDepth)
                {
                    TreeNode node = new TreeNode(tc.TaskName);
                    Core.Self.CalculateTaskSummaryProgress(tc.Task);                               
                    node.Tag = tc.Index;
                    if (highlight)
                        setNodeFontAndColor(node, tc.Task);        

                    FillTreeViewWithChildrenTasks(tc, node, highlight);     
               
                    treeView.Nodes.Add(node);
                }
            }
            treeView.EndUpdate();
        }
        void FillTreeViewWithChildrenTasks(TaskContainer tc, TreeNode node, bool highlight)
        {
            foreach (TaskContainer childContainer in tc.Children)
            {
                TreeNode childNode = new TreeNode(childContainer.TaskName);
                Core.Self.CalculateTaskSummaryProgress(childContainer.Task);                
                childNode.Tag = childContainer.Index;
                node.Nodes.Add(childNode);
                node.ExpandAll();
                if (highlight)
                    setNodeFontAndColor(childNode, childContainer.Task);

                FillTreeViewWithChildrenTasks(childContainer, childNode, highlight);
            }
        }

        /// <summary>
        ////Собирает в плоский массив все ноды дерева. Нужно для подсвечивания нужной.
        /// </summary>
        public List<TreeNode> getAllNodes(TreeView tree)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode node in tree.Nodes)
            {
                result.Add(node);
                getChildren(result, node);
            }
            return result;
        }
        private void getChildren(List<TreeNode> list, TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                list.Add(child);
                getChildren(list, child);
            }
        }

        /// <summary>
        /// Подсвечивает ноду дерева в зависимости от того, назначен ли таску набор и введены ли данные.
        /// </summary>
        public void setNodeFontAndColor(TreeNode node, TimelinerTask task)
        {
            if (!task.Selection.IsClear)
            {
                node.BackColor = System.Drawing.Color.Green;
                if(hasDataAttached(task))
                {
                    node.NodeFont = new Font(TreeView.DefaultFont, FontStyle.Bold | FontStyle.Underline);
                }
            }
            else if(hasDataAttached(task))
            {
                node.ForeColor = System.Drawing.Color.SkyBlue;
                node.NodeFont = new Font(TreeView.DefaultFont, FontStyle.Bold | FontStyle.Underline);                                
            }
        }

        bool hasDataAttached(TimelinerTask task)
        {
            if (string.IsNullOrEmpty(task.User1))
                return false;
            else
                return true;
        }

        public void MSProjectExport()
        {
            if (TasksOK())
            {
                ProgressForm pForm = new ProgressForm(this.tasks.Count);
                MSProjectInterop msp = new MSProjectInterop(pForm);
                msp.AddTasks(this.Tasks);
            }
        }
    }
}
