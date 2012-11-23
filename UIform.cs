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
        TimelinerTask RootTimelinerTask;

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
            Core.Self.SaveTasks();
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

        /// <summary>
        /// Обработчик клика по кнопке "начать". Заполняет коллекции и инициализирует счётчики, загружает первый таск.
        /// </summary>
        private void StartDataInputButton_Click(object sender, EventArgs e)
        {
            if (Core.Self.AreTasksAccessible())
            {
                groupBox2.Enabled = true;
                nexTimelinerTaskToDataInput();
            }
        }

        private void WriteCompletionToTask()
        {
            string value = removeLetters(CompletionTextBox.Text);
            string units = UnitsComboBox.Text;
            CompletionTextBox.Text = value;

            if (!string.IsNullOrEmpty(value))
            {
                TimelinerTask task = Core.Self.currenTimelinerTask.Task.CreateCopy();
                int index = RootTimelinerTask.Children.IndexOfDisplayName(task.DisplayName);

                task.SetUserFieldByIndex(0, value);
                task.SetUserFieldByIndex(1, units);

                timeliner.TaskEdit(RootTimelinerTask, index, task);

                nexTimelinerTaskToDataInput();
            }
            else
                MessageBox.Show("Введите значение");
        }

        /// <summary>
        /// Отображает на модели следующий по порядку таск.
        /// </summary>
        void nexTimelinerTaskToDataInput()
        {
            if (Core.Self.Tasks.Count != 0)
            {
                Core.Self.currenTimelinerTask.update(Core.Self.Tasks.First().Task, CurrentViewTaskBox);
                Core.Self.Tasks.RemoveAt(0);
                Core.Self.hideAllExcepTimelinerTaskSelection(Core.Self.currenTimelinerTask.Task, CurrentViewTaskBox);
                CompletionTextBox.Text = Core.Self.currenTimelinerTask.Task.User1;
                UnitsComboBox.Text = Core.Self.currenTimelinerTask.Task.User2;
            }
            else
            {
                groupBox2.Enabled = false;
            }

        }        

        private void buttonNext_Click(object sender, EventArgs e)
        {
            nexTimelinerTaskToDataInput();
        }
        
        private void ButtonAcceptCompletionProgress_Click(object sender, EventArgs e)
        {
            WriteCompletionToTask();
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
            }
        }

    }
}
