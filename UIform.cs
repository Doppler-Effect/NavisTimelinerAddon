using System;
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
                        //addCustomDataSource();
                        //RootTask = timeliner.Tasks[0] as TimelinerTask;
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
        List<TimelinerTask> tasks = new List<TimelinerTask>(); //массив тасков проекта. Используется в ручном назначении тасков.
        CurrentTask currentTask = new CurrentTask();//Контейнер для хранения обрабатываемого в данный момент таска.
        SerializableDataHolder DataHolder;//массив для сохранения обработанных тасков в файл.
        bool RootTaskIsAccessible()
        {
            if (timeliner.Tasks.Count != 0)
            {
                RootTask = timeliner.Tasks[0] as TimelinerTask;
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
            MakeAllModelItemsVisible();
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
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
            if (RootTaskIsAccessible())
            {
                //убираем текущие селекшны у тасков.
                ClearSelections();

                DataHolder = new SerializableDataHolder();
                tasks.Clear();
                foreach (TimelinerTask task in RootTask.Children)
                {
                    tasks.Add(task);
                }

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
                Serializer.serialize(DataHolder);
            }
        }

        private void AcceptCurrentSelectionButton_Click(object sender, EventArgs e)
        {
            if (RootTaskIsAccessible())
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

                DataHolder.Add(task.DisplayName, this.findSelectionSetName(selection));                
                nextTaskToAssociate();                
            }
        }

        /// <summary>
        /// Поиск имени списка выбора по имеющемуся селекшну
        /// </summary>
        /// <param name="sel">Выбранные в Navisworks элементы</param>
        /// <returns>Имя списка выбора, который выбирает указанный селекшн</returns>
        string findSelectionSetName(Selection sel)
        {
            if (sel.HasSelectionSources == true)
            {
                SelectionSource sSource = sel.SelectionSources[0];
                SavedItem sSet = nDoc.SelectionSets.ResolveSelectionSource(sSource);
                return sSet.DisplayName;
            }
            return null;
        }

        private void SkipCurrentTaskButton_Click(object sender, EventArgs e)
        {
            if (RootTaskIsAccessible())
            {
                nextTaskToAssociate();
            }
        }
        
        private void SaveAssocNowButton_Click(object sender, EventArgs e)
        {
            if (DataHolder != null)
            {
                if (DataHolder.Data.Count != 0)
                {
                    Serializer.serialize(DataHolder);
                }
            }
        }

        /// <summary>
        /// Убирает селекшны у всех тасков в таймлайнере.
        /// </summary>
        void ClearSelections()
        {
            for(int i = 0; i < RootTask.Children.Count; i++)
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

        #endregion

        #region Загрузка данных об ассоциированных тасках из файла

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            if (RootTaskIsAccessible())
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

        /// <summary>
        /// Возвращает выборку элементов модели по имени списка выбора.
        /// </summary>
        /// <param name="Name">Имя списка выбора (Selextion Set), для которого нужно получить выборку.</param>
        SelectionSourceCollection getSelectionSourceByName(string Name)
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
                
        #region Просмотр тасков и отображение только тех элементов модели, которые с ними ассоциированы

        /// <summary>
        /// Обработчик клика по кнопке "начать". Заполняет коллекции и инициализирует счётчики, загружает первый таск.
        /// </summary>
        private void StartDataInputButton_Click(object sender, EventArgs e)
        {
            if (RootTaskIsAccessible())
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
            string value = CompletionTextBox.Text;
            string units = UnitsComboBox.Text;

            TimelinerTask task = currentTask.Task.CreateCopy();
            int index = RootTask.Children.IndexOfDisplayName(task.DisplayName);

            task.SetUserFieldByIndex(0, value);
            task.SetUserFieldByIndex(1, units);

            timeliner.TaskEdit(RootTask, index, task);

            nextTaskToDataInput();
        }

        void MakeAllModelItemsVisible()
        {
            nDoc.CurrentSelection.SelectAll();
            nDoc.Models.OverridePermanentTransparency(nDoc.CurrentSelection.SelectedItems, 0);
            nDoc.CurrentSelection.Clear();
        }
        
        #endregion

        #region Добавление в Таймлайнер данных из .csv MS Project
        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
