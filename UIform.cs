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


        #region Конструктор и обработчики закрытия формы
        public UIform()
        {
            InitializeComponent();
            Core.Self.activeUIForm = this;
            this.FormClosed += UIform_FormClosed;
        }

        void UIform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.Self.activeUIForm = null;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            StopDataInput();
            this.Close();
        }   
        #endregion      
        
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
        private void DrawInputArea()
        {
            if (Core.Self.TasksOK())
            {
                FillTaskList();
                groupBox2.Enabled = true;
                groupBoxElem.Enabled = true;
                buttonDOWN.Enabled = true;
                buttonUP.Enabled = buttonDOWN.Enabled;
                //radioButtonAll.Enabled = true;
                //radioButtonElem.Enabled = true;
                //radioButtonAll.Checked = false;
                //radioButtonAll.Checked = true;
            }
        }
        
        private void StartDataInput_Click(object sender, EventArgs e)
        {
            DrawInputArea();
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
        private void FillTaskList()
        {
            TasksView.BeginUpdate();
            TasksView.Nodes.Clear();
            
            Core.Self.FillTreeViewWithTasks(this.TasksView);

            if (TasksView.Nodes.Count != 0)
            {
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
            if (string.IsNullOrEmpty(maxCompletionTextBox.Text))
                checkMaxCompletionAndUnits();
        }

        /// <summary>
        /// Выставляет предустановленное максивальное значение (или вычисляет его) в зависимости от выбранных единиц измерения.
        /// </summary>
        void checkMaxCompletionAndUnits()
        {
            if (UnitsComboBox.SelectedItem != null)
            {
                string sel = UnitsComboBox.SelectedItem.ToString();
                switch (sel)
                {
                    case "%":
                        maxCompletionTextBox.Text = "100";
                        break;
                    default:
                        maxCompletionTextBox.Text = null;
                        break;
                }
            }
        }        
        private void UnitsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkMaxCompletionAndUnits();
        }   
      
        /// <summary>
        /// Перелистывание таска назад
        /// </summary>
        private void taskUp()
        {
            if (TasksView.SelectedNode != null)
            {
                if (TasksView.SelectedNode.PrevVisibleNode != null)
                    TasksView.SelectedNode = TasksView.SelectedNode.PrevVisibleNode;
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
                if (TasksView.SelectedNode.NextVisibleNode != null)
                    TasksView.SelectedNode = TasksView.SelectedNode.NextVisibleNode;
            }
        }
        private void buttonDOWN_Click(object sender, EventArgs e)
        {
            taskDown();
        }
         
        /// <summary>
        /// Отображает tooltip при наведении указателя на мышь
        /// </summary>
        private void TasksView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            int itemsNum;
            TreeNode node = e.Node;
            Collection<int> index = node.Tag as Collection<int>;
            TimelinerTask task = timeliner.TaskResolveIndexPath(index);
            if (task.Selection.IsClear)
                itemsNum = 0;
            else
                itemsNum = task.Selection.GetSelectedItems(nDoc).Count;

            string SelSetName = Core.Self.FindSelectionSetName(task);

            node.ToolTipText = string.Format("Набор: {0}, {1} элемент(ов) в выборке", SelSetName, itemsNum.ToString());
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
            if (this.checkInputIsOK(value, units, maxValue))
            {
                if (units == "%")
                {
                    percent = double.Parse(value);
                }
                else if (maxValue != null)
                {
                    double maxV = double.Parse(maxValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture);
                    double val = double.Parse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture);
                    percent = val * 100 / maxV;
                }

                //DialogResult res = MessageBox.Show( "Удалить данные об отдельных элементах и внести данные для всего набора?", "Внимание!", MessageBoxButtons.OKCancel);
                //if (res == DialogResult.OK)
                {
                    TreeNode node = TasksView.SelectedNode;                    
                    Collection<int> index = node.Tag as Collection<int>;
                    TimelinerTask task = timeliner.TaskResolveIndexPath(index);
                    //Core.Self.ClearTaskItemsFromDB(task);
                    if (Core.Self.WriteCompletionToTask(index, value, units, maxValue, percent))
                    {
                        Core.Self.setNodeFontAndColor(node, task); 
                        TasksView.Refresh();
                        taskDown();                        
                    }
                }
            }            
        }

        /// <summary>
        /// Проверяет введённые данные на null и прочие ограничения
        /// </summary>
        /// <returns></returns>
        bool checkInputIsOK(string value, string units, string maxValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(units))
            {
                MessageBox.Show("Введите прогресс выполнения и единицы измерения!");
                return false;
            }
            
            if (string.IsNullOrEmpty(maxValue))
            {
                MessageBox.Show("Введите максимальное значение!");
                return false;
            }
            
            double maxV;
            double val;

            if (double.TryParse(maxValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out maxV))
            {
                if (double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out val) && maxV != 0)
                {
                    if (val <= maxV)
                    {
                        return true;
                    }
                    else
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

        /// <summary>
        /// Вызывает окошко ввода данных для конкретного элемента при клике.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
