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
                Core.Self.LoadTasksFromFile();
            }
        }
        
        #endregion
                
        #region Просмотр тасков и отображение только тех элементов модели, которые с ними ассоциированы

        /// <summary>
        /// Отрисовывает интерфейс программы, отвечающий за внесение данных в NavisWorks
        /// </summary>
        /// <param name="withSelections">Обрабатывать таски с присоединёнными наборами или нет</param>
        private void DrawInputArea(bool withSelections)
        {
            if (Core.Self.TasksOK())
            {
                FillTaskList(withSelections);
                groupBox2.Enabled = true;
                groupBoxElem.Enabled = !withSelections;
                buttonDOWN.Enabled = withSelections;
                buttonUP.Enabled = buttonDOWN.Enabled;
                radioButtonAll.Enabled = withSelections;
                radioButtonElem.Enabled = withSelections;
                radioButtonAll.Checked = true;
                radioButtonAll.Checked = false;
                radioButtonAll.Checked = withSelections;
            }
        }

        private void StartDataInputButton_Click(object sender, EventArgs e)
        {
            DrawInputArea(true);
        }

        private void StartDataInputWithoutSelectedButton_Click(object sender, EventArgs e)
        {
            DrawInputArea(false);
        }
        
        private void StopDataInput()
        {
            TasksView.Nodes.Clear();
            Core.Self.MakeAllModelItemsVisible();
            radioButtonAll.Checked = false;
            radioButtonElem.Checked = false;
            groupBox2.Enabled = false;
        }

        /// <summary>
        ////Наполняет тасклист теми тасками, у которых есть назначенные наборы элементов.
        /// </summary>
        private void FillTaskList(bool WithSelection)
        {
            TasksView.BeginUpdate();
            TasksView.Nodes.Clear();
            if (WithSelection)
            {
                foreach (TaskContainer taskC in Core.Self.Tasks)
                {
                    Core.Self.CalculateTaskSummaryProgress(taskC.Task);
                    if (!taskC.Task.Selection.IsClear)
                    {
                        TreeNode node = new TreeNode(taskC.TaskName);
                        node.Tag = taskC.Index;
                        TasksView.Nodes.Add(node);
                    }
                }
            }
            else
            {
                Core.Self.FillTreeViewWithTasks(this.TasksView);
            }

            if (TasksView.Nodes.Count != 0)
            {
                TasksView.SelectedNode = TasksView.Nodes[0];
                TasksView.SelectedNode = TasksView.Nodes[0];
            }
            TasksView.EndUpdate();
        }
        
        /// <summary>
        /// Событие, происходящее при изменении выбора таска в списке.
        /// </summary>
        private void TasksView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Collection<int> index = TasksView.SelectedNode.Tag as Collection<int>;
            TimelinerTask task = timeliner.TaskResolveIndexPath(index);
            Core.Self.HideAllExceptTaskSelection(task);

            //заполнение полей выполнения и единиц измерения
            fillDataFromTask(task);
        }

        /// <summary>
        /// Заполняет поля на форме значениями. Берёт данные из полей таймлайнера.
        /// </summary>
        public void fillDataFromTask(TimelinerTask Task)
        {
            CompletionTextBox.Text = Task.User1;
            UnitsComboBox.Text = Task.User2;
            maxCompletionTextBox.Text = Task.User3;
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
        private void buttonUP_Click(object sender, EventArgs e)
        {
            taskUp();
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
        private void buttonDOWN_Click(object sender, EventArgs e)
        {
            taskDown();
        }
        #endregion

        #region Внесение данных о выполнении тасков
        private void ButtonAcceptCompletionProgress_Click(object sender, EventArgs e)
        {
            WriteCompletionFromUI();
        }
        /// <summary>
        /// Запись введённой информации и единиц измерения в таск.
        /// </summary>
        private void WriteCompletionFromUI()
        {
            string value = CompletionTextBox.Text.removeLetters();
            string units = UnitsComboBox.Text;
            string maxValue = maxCompletionTextBox.Text.removeLetters();
            
            //проверяем введённые значения и расчитываем процент выполнения:
            double percent = 0;
            if (this.inputIsCorrect(value, units, maxValue))
            {
                if (units == "%")
                {
                    percent = double.Parse(value);
                }
                else if (maxValue != null)
                {
                    double maxV = double.Parse(maxValue);
                    double val = double.Parse(value);
                    percent = val * 100 / maxV;
                }

                DialogResult res = MessageBox.Show( "Удалить данные об отдельных элементах и внести данные для всего набора?", "Внимание!", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    Collection<int> index = TasksView.SelectedNode.Tag as Collection<int>;
                    Core.Self.ClearTaskItemsFromDB(timeliner.TaskResolveIndexPath(index));
                    if (Core.Self.WriteCompletionToTask(index, value, units, maxValue, percent))
                        taskDown();
                }
            }            
        }

        /// <summary>
        /// Проверяет введённые данные на null и прочие ограничения
        /// </summary>
        /// <returns></returns>
        bool inputIsCorrect(string value, string units, string maxValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(units))
            {
                MessageBox.Show("Введите необходимые значения!");
                return false;
            }
            if (!string.IsNullOrEmpty(maxValue))
            {
                double maxV;
                double val;

                if (double.TryParse(maxValue, out maxV))
                {
                    if (double.TryParse(value, out val) && maxV != 0)
                    {
                        if (val > maxV)
                        {
                            MessageBox.Show("Указанное значение выполнения больше максимального!");
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            return true;
        }
        
        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonElem.Checked)
            {
                nDoc.CurrentSelection.Changed += CurrentNavisSelection_Changed;
                groupBoxElem.Enabled = false;
            }
            else
            {
                nDoc.CurrentSelection.Changed -= this.CurrentNavisSelection_Changed;
                groupBoxElem.Enabled = true;
            }
        }

        void CurrentNavisSelection_Changed(object sender, EventArgs e)
        {
            if (nDoc.CurrentSelection.SelectedItems.Count == 1)
            {
                string id = Core.Self.GetElementUniqueID(nDoc.CurrentSelection.SelectedItems.First);
                Collection<int> index = TasksView.SelectedNode.Tag as Collection<int>;
                TimelinerTask task = timeliner.TaskResolveIndexPath(index);

                inputForm input = new inputForm(task, id, this);
                if (Core.Self.activeInputForm != null)
                {
                    Core.Self.activeInputForm.Close();
                }
                Core.Self.activeInputForm = input;
                input.Show();
            }
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
