using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        
        private void groupBox2_EnabledChanged(object sender, EventArgs e)
        {
            Core.Self.MakeAllModelItemsVisible();
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Core.Self.MakeAllModelItemsVisible();
        }        
        
        #region Загрузка и сохранение данных об ассоциированных тасках из файла

        private void SaveTaskButton_Click(object sender, EventArgs e)
        {
            if (Core.Self.AreTasksAccessible())
            {
                Core.Self.SaveTasksToFile();
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            if (Core.Self.AreTasksAccessible())
            {
                Core.Self.LoadTasksAssocFromFile();
            }
        }
        
        #endregion
                
        #region Просмотр тасков и отображение только тех элементов модели, которые с ними ассоциированы

        private int CurrentTaskNumber;

        /// <summary>
        /// Обработчик клика по кнопке "начать". Заполняет коллекции и инициализирует счётчики, загружает первый таск.
        /// </summary>
        private void StartDataInputButton_Click(object sender, EventArgs e)
        {            
            if (Core.Self.AreTasksAccessible())
            {
                CurrentTaskNumber = 0;
                groupBox2.Enabled = true;
                viewCurrentTask();
            }
        }

        void viewCurrentTask()
        {
            if (CurrentTaskNumber >= Core.Self.Tasks.Count)
                CurrentTaskNumber = 0;
            if (CurrentTaskNumber < 0)
                CurrentTaskNumber = Core.Self.Tasks.Count - 1;
            TimelinerTask task = Core.Self.Tasks[CurrentTaskNumber].Task;
            CurrentViewTaskBox.Text = task.DisplayName;
            Core.Self.hideAllExceptTaskSelection(task);
            CompletionTextBox.Text = task.User1;
            UnitsComboBox.Text = task.User2;

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            CurrentTaskNumber++;
            viewCurrentTask();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            CurrentTaskNumber--;
            viewCurrentTask();
        }
        
        private void ButtonAcceptCompletionProgress_Click(object sender, EventArgs e)
        {
            WriteCompletionToCurrentTask();
        }

        /// <summary>
        /// Запись введённой информации и единиц измерения в таск.
        /// </summary>
        private void WriteCompletionToCurrentTask()
        {
            string value = removeLetters(CompletionTextBox.Text);
            string units = UnitsComboBox.Text;

            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(units))
            {
                TimelinerTask task = Core.Self.Tasks[CurrentTaskNumber].Task.CreateCopy();
                Collection<int> index = timeliner.TaskCreateIndexPath(task);
                GroupItem parent = timeliner.TaskResolveIndexPath(index).Parent;
                int id = parent.Children.IndexOfDisplayName(task.DisplayName);

                task.SetUserFieldByIndex(0, value);
                task.SetUserFieldByIndex(1, units);

                timeliner.TaskEdit(parent, id, task);

                CurrentTaskNumber++;
                viewCurrentTask();
            }
            else
                MessageBox.Show("Введите значения");
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

        private void ManualAssocButton_Click(object sender, EventArgs e)
        {
            if (Core.Self.AreTasksAccessible())
            {
                DetailedForm form = new DetailedForm(timeliner, nDoc);
                form.Show();
                this.Visible = false;
            }
        }
    }
}
