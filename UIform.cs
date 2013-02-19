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
        
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            StopDataInput();
        }        
        
        #region Загрузка и сохранение данных об ассоциированных тасках из файла

        private void SaveTaskButton_Click(object sender, EventArgs e)
        {
            StopDataInput();
            if (Core.Self.TasksOK())
            {
                Core.Self.SaveTasksToFile();
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            StopDataInput();
            if (Core.Self.TasksOK())
            {
                Core.Self.LoadTasksAssocFromFile();
            }
        }
        
        #endregion
                
        #region Просмотр тасков и отображение только тех элементов модели, которые с ними ассоциированы
        
        private void StartDataInputButton_Click(object sender, EventArgs e)
        {            
            if (Core.Self.TasksOK())
            {
                FillTaskList(true);
                groupBox2.Enabled = true;
                nDoc.CurrentSelection.Changed += CurrentSelection_Changed;
            }
        }

        private void StartDataInputWithoutSelectedButton_Click(object sender, EventArgs e)
        {
            if (Core.Self.TasksOK())
            {
                FillTaskList(false);
                groupBox2.Enabled = true;
            }
        }

        void CurrentSelection_Changed(object sender, EventArgs e)
        {
            if (nDoc.CurrentSelection.SelectedItems.Count == 1)
            {
                MessageBox.Show(Core.Self.GetElementUniqueID(nDoc.CurrentSelection.SelectedItems.First));
            }
        }        

        private void StopDataInput()
        {
            TasksView.Nodes.Clear();
            Core.Self.MakeAllModelItemsVisible();
            groupBox2.Enabled = false;
            nDoc.CurrentSelection.Changed -= this.CurrentSelection_Changed;
        }

        /// <summary>
        ////Наполняет тасклист теми тасками, у которых есть назначенные наборы элементов.
        /// </summary>
        private void FillTaskList(bool WithSelection)
        {
            TasksView.BeginUpdate();
            TasksView.Nodes.Clear();
            foreach (TaskContainer taskC in Core.Self.Tasks)
            {
                if (WithSelection)
                {
                    if (!taskC.Task.Selection.IsClear)
                    {
                        TreeNode node = new TreeNode(taskC.TaskName);
                        node.Tag = taskC.Index;
                        TasksView.Nodes.Add(node);
                    }
                }
                else if (taskC.Task.Selection.IsClear)
                {
                    TreeNode node = new TreeNode(taskC.TaskName);
                    node.Tag = taskC.Index;
                    TasksView.Nodes.Add(node);
                }
            }
            if (TasksView.Nodes.Count != 0)
            {
                TasksView.SelectedNode = TasksView.Nodes[0];
                TasksView.SelectedNode = TasksView.Nodes[0];
            }
            TasksView.EndUpdate();
        }
        
        private void TasksView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Collection<int> index = TasksView.SelectedNode.Tag as Collection<int>;
            TimelinerTask task = timeliner.TaskResolveIndexPath(index);
            Core.Self.HideAllExceptTaskSelection(task);

            //заполнение полей выполнения и единиц измерения
            CompletionTextBox.Text = task.User1;
            UnitsComboBox.Text = task.User2;
        }
        
        private void ButtonAcceptCompletionProgress_Click(object sender, EventArgs e)
        {
            WriteCompletionProgress();
        }

        private void buttonUP_Click(object sender, EventArgs e)
        {
            taskUp();
        }

        private void buttonDOWN_Click(object sender, EventArgs e)
        {
            taskDown();
        }

        /// <summary>
        /// Перелистывание таска назад
        /// </summary>
        private void taskUp()
        {
            if (TasksView.SelectedNode != null)
            {
                if (TasksView.SelectedNode.PrevNode != null)
                    TasksView.SelectedNode = TasksView.SelectedNode.PrevNode;
                else
                    TasksView.SelectedNode = TasksView.Nodes[TasksView.Nodes.Count - 1];
            }
        }

        /// <summary>
        /// Перелистывание таска вперёд
        /// </summary>
        private void taskDown()
        {
            if (TasksView.SelectedNode != null)
            {
                if (TasksView.SelectedNode.NextNode != null)
                    TasksView.SelectedNode = TasksView.SelectedNode.NextNode;
                else
                    TasksView.SelectedNode = TasksView.Nodes[0];
            }
        }

        /// <summary>
        /// Запись введённой информации и единиц измерения в таск.
        /// </summary>
        private void WriteCompletionProgress()
        {
            string value = removeLetters(CompletionTextBox.Text);
            string units = UnitsComboBox.Text;

            try
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(units))
                {
                    Collection<int> index = TasksView.SelectedNode.Tag as Collection<int>;
                    TimelinerTask task = timeliner.TaskResolveIndexPath(index).CreateCopy();
                    GroupItem parent = timeliner.TaskResolveIndexPath(index).Parent;
                    int id = parent.Children.IndexOfDisplayName(task.DisplayName);

                    task.SetUserFieldByIndex(0, value);
                    task.SetUserFieldByIndex(1, units);

                    timeliner.TaskEdit(parent, id, task);

                    taskDown();
                }
                else
                    MessageBox.Show("Введите значения");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        
        /// <summary>
        /// Убирает буквы из полученной строки.
        /// </summary>
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
            StopDataInput();
            if (Core.Self.TasksOK())
            {
                DetailedForm form = new DetailedForm(timeliner, nDoc);
                form.Show();
                this.Visible = false;
            }
        }

        private void buttonMSProject_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Core.Self.MSProjectExport();
        }

    }
}
