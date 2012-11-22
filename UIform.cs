﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisTimelinerPlugin
{    
    public partial class UIform : Form //Singleton.
    {
        static Document nDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        static Autodesk.Navisworks.Api.DocumentParts.IDocumentTimeliner Itimeliner = nDoc.Timeliner;
        static DocumentTimeliner timeliner = (DocumentTimeliner)Itimeliner;
        static TimelinerTask RootTask;

        #region логика конструктора - синглтона
        private static UIform instance;
        private UIform()
        {
            InitializeComponent();
        }
        public static UIform Instance
        {
            get
            {
                if (!nDoc.IsClear)
                {
                    if (instance == null)
                    {
                        instance = new UIform();
                    }
                    return instance;
                }
                else
                {
                    MessageBox.Show("Для работы с программой откройте проект", "Нет данных для работы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }
            }
        }
        #endregion

        #region счётчики, контейнеры для данных итд.
        public List<TimelinerTask> Tasks
        {
            get
            {
                return tasks;
            }
        }
        List<TimelinerTask> tasks = new List<TimelinerTask>(); //массив тасков проекта. Используется в процедуре перебора тасков.
        CurrentTask currentTask = new CurrentTask();//Контейнер для хранения обрабатываемого в данный момент таска.
        SerializableDataHolder DataHolder;//массив для сохранения обработанных тасков в файл.
        bool TasksAreAccessible()
        {
            if (timeliner.Tasks.Count != 0)
            {
                tasks.Clear();
                TimelinerTask RootTask = timeliner.Tasks[0] as TimelinerTask;
                this.getTasks(RootTask);
                return true;
            }
            else
            {
                MessageBox.Show("Отсутствуют задания в Timeliner", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        private void groupBox2_EnabledChanged(object sender, EventArgs e)
        {
            this.MakeAllModelItemsVisible();
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.MakeAllModelItemsVisible();
        }
        #endregion

        #region Общие методы и св-ва

        /// <summary>
        /// Рекурсивный обход дерева тасков и сбор всех тасков в один массив.
        /// </summary>
        void getTasks(TimelinerTask task)
        {
            foreach (TimelinerTask child in task.Children)
            {
                this.tasks.Add(child);
                getTasks(child);
            }
        }

        /// <summary>
        /// Убирает селекшны у всех тасков в таймлайнере.
        /// </summary>
        public void ClearSelections()
        {
            for (int i = 0; i < RootTask.Children.Count; i++)
            {
                TimelinerTask task = RootTask.Children[i] as TimelinerTask;
                if (!task.Selection.IsClear)
                {
                    TimelinerTask tmpTask = task.CreateCopy();
                    tmpTask.Selection.Clear();
                    timeliner.TaskEdit(RootTask, i, tmpTask);
                }
            }
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

        #endregion

        #region Механизм ручного назначения выборок таскам.
        /// <summary>
        /// Запускает механизм назначения селекшнов таскам.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAssocButton_Click(object sender, EventArgs e)
        {
            if (TasksAreAccessible())
            {
                //tasks.Clear();
                //foreach (TimelinerTask task in RootTask.Children)
                //{
                //    tasks.Add(task);
                //}

                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
                //заполнение окошка первым таском. 
                nextTaskToAssociate();
            }
        }

        /// <summary>
        /// Берёт нулевой элемент локальной копии коллекции тасков, помещает в окошко и удаляет его из коллекции.
        /// </summary>
        /// <returns>False if task collection is empty.</returns>
        void nextTaskToAssociate()
        {
            if (tasks.Count != 0)
            {
                currentTask.update(tasks.First(), CurrentAssocTaskBox);
                tasks.RemoveAt(0);
            }
            else                
            {
                //таски закончились
                groupBox1.Enabled = false;
                CurrentAssocTaskBox.Text = "Done.";

                //сохраняем набор ассоциаций в файл
                SaveTasks();
            }
        }

        private void AcceptCurrentSelectionButton_Click(object sender, EventArgs e)
        {
            if (TasksAreAccessible())
            {
                addSelectionToTask();
            }
        }

        /// <summary>
        ////Добавляет к таску, находящемуся в окошке, текущий селекшн.
        /// </summary>
        void addSelectionToTask()
        {
            if (currentTask.Task != null)
            {                
                TimelinerTask task = currentTask.Task.CreateCopy();
                Selection selection = nDoc.CurrentSelection.Value;
                task.Selection.CopyFrom(selection);
                int index = RootTask.Children.IndexOfDisplayName(task.DisplayName); 
                timeliner.TaskEdit(RootTask, index, task);                  
                nextTaskToAssociate();                
            }
        }        

        private void SkipCurrentTaskButton_Click(object sender, EventArgs e)
        {
            if (TasksAreAccessible())
            {
                nextTaskToAssociate();
            }
        }
        
        private void SaveTaskButton_Click(object sender, EventArgs e)
        {
            SaveTasks();
        }

        public void SaveTasks()
        {
            DataHolder.Clear();
            foreach (TimelinerTask task in RootTask.Children)
            {
                string name = task.DisplayName;
                string sel = findSelectionSetName(task);
                DataHolder.Add(name, sel);
            }
            Serializer.serialize(DataHolder);
        }
        
        #endregion

        #region Загрузка данных об ассоциированных тасках из файла

        private void LoadButton_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            if (TasksAreAccessible())
            {
                LoadTasksAssocFromFile();
            }
        }
        /// <summary>
        /// Загрузка пар "порядковый номер" - "Selection set name" из бинарного файла и назначение селекшнов таскам.
        /// </summary>
        private void LoadTasksAssocFromFile()
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
                        int taskIndex = RootTask.Children.IndexOfDisplayName(pair.Key);
                        if (taskIndex != -1)
                        {
                            TimelinerTask task = ((TimelinerTask)RootTask.Children[taskIndex]).CreateCopy();
                            task.Selection.CopyFrom(selectionSource);
                            timeliner.TaskEdit(RootTask, taskIndex, task);
                        }
                    }
                }
            }
        }

        #endregion
                
        #region Просмотр тасков и отображение только тех элементов модели, которые с ними ассоциированы

        /// <summary>
        /// Обработчик клика по кнопке "начать". Заполняет коллекции и инициализирует счётчики, загружает первый таск.
        /// </summary>
        private void StartDataInputButton_Click(object sender, EventArgs e)
        {
            if (TasksAreAccessible())
            {
                tasks.Clear();
                foreach (TimelinerTask task in RootTask.Children)
                {
                    tasks.Add(task);
                }

                groupBox1.Enabled = false;
                groupBox2.Enabled = true;
                nextTaskToDataInput();
            }
        }

        /// <summary>
        /// Отображает на модели следующий по порядку таск.
        /// </summary>
        void nextTaskToDataInput()
        {
            if (tasks.Count != 0)
            {
                currentTask.update(tasks.First(), CurrentViewTaskBox);
                tasks.RemoveAt(0);
                hideAllExceptTaskSelection(currentTask.Task);
                CompletionTextBox.Text = currentTask.Task.User1;
                UnitsComboBox.Text = currentTask.Task.User2;
            }
            else
            {
                groupBox2.Enabled = false;
            }

        }

        /// <summary>
        /// Показывает на модели только выборку элементов, остальное делает прозрачным.
        /// </summary>
        /// <param name="task">Таск, выборка которого отображается на модели.</param>
        void hideAllExceptTaskSelection(TimelinerTask task)
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
                    CurrentViewTaskBox.Text += " /не назначена выбрка/"; 
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            nextTaskToDataInput();
        }
        
        private void ButtonAcceptCompletionProgress_Click(object sender, EventArgs e)
        {
            WriteCompletionToTask();
        }

        private void WriteCompletionToTask()
        {
            string value = removeLetters(CompletionTextBox.Text);
            string units = UnitsComboBox.Text;
            CompletionTextBox.Text = value;

            if (!string.IsNullOrEmpty(value))
            {
                TimelinerTask task = currentTask.Task.CreateCopy();
                int index = RootTask.Children.IndexOfDisplayName(task.DisplayName);

                task.SetUserFieldByIndex(0, value);
                task.SetUserFieldByIndex(1, units);

                timeliner.TaskEdit(RootTask, index, task);

                nextTaskToDataInput();
            }
            else
                MessageBox.Show("Введите значение");
        }

        void MakeAllModelItemsVisible()
        {
            nDoc.CurrentSelection.SelectAll();
            nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 0);
            nDoc.CurrentSelection.Clear();
        }

        string removeLetters(string str)
        {
            string result = null;
            foreach (char c in str)
            {
                if (char.IsDigit(c) || char.IsPunctuation(c))
                    result += c;
            }
            return result;
        }
        
        #endregion

        #region Добавление в Таймлайнер данных из .csv MS Project
                
        //private TimelinerTask makeTask()
        //{
        //    //for (int i = 0; i < 20; i++)
        //    //{
        //    //    TimelinerTask task = new TimelinerTask();
        //    //    task.DisplayName = "Foo" + i;
        //    //    task.ActualStartDate = DateTime.Now;
        //    //    task.ActualEndDate = DateTime.Now;
        //    //    //root.Children.Add(task);
        //    //}

        //    //timeliner.TaskAddCopy(timeliner.TasksRoot, root);
        //}

        //private bool openCSVFile(ref string filename)
        //{
        //    OpenFileDialog opfile = new OpenFileDialog();
        //    opfile.AddExtension = true;
        //    opfile.CheckFileExists = true;
        //    opfile.Multiselect = false;
        //    opfile.RestoreDirectory = true;
        //    opfile.DefaultExt = "csv";
        //    opfile.Filter = "Comma separated values (*.csv)|*.csv";
        //    opfile.RestoreDirectory = true;
        //    opfile.Title = "Выберите файл...";

        //    DialogResult res = opfile.ShowDialog(UIform.Instance);
        //    if (res == DialogResult.OK)
        //    {
        //        filename = opfile.FileName;
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        #endregion

        private void ManualAssocButton_Click(object sender, EventArgs e)
        {
            if (TasksAreAccessible())
            {
                DetailedForm form = new DetailedForm(timeliner, nDoc, RootTask);
                form.Show();
            }
        }

    }
}
