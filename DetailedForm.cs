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
    public partial class DetailedForm : Form
    {
        DocumentTimeliner timeliner;
        Document nDoc;
        TimelinerTask RooTimelinerTask;

        public DetailedForm(DocumentTimeliner timeliner, Document nDoc)
        {
            InitializeComponent();
            this.timeliner = timeliner;
            this.nDoc = nDoc;
            FillGridSelections();
            FillTasks();
            //FillSelections();
        }

        /// <summary>
        /// Заполняет комбобоксы второй колонки значениями (названиями селекшн сетов документа).
        /// </summary>
        void FillGridSelections()
        {
            List<string> dataSource = new List<string>();
            dataSource.Add("");
            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                dataSource.Add(item.DisplayName);
            }
            this.SET.DataSource = dataSource;
        }

        /// <summary>
        /// Заполняет первую колонку именами тасков.
        /// </summary>
        void FillTasks()
        {
            foreach (TaskContainer tc in Core.Self.Tasks)
            {
                this.dataGridView1.Rows.Add(tc.Index, tc.Task.DisplayName);
            }
        }

        /// <summary>
        /// Выбирает во второй колонке уже назначенные селекшны, если у соответствующих тасков они есть.
        /// </summary>
        void FillSelections()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string taskName = row.Cells[0].Value.ToString();
                int taskNo = 0;
                if (taskNo != -1)
                {
                    string selection = Core.Self.findSelectionSetName(RooTimelinerTask.Children[taskNo] as TimelinerTask);
                    if (selection != null)
                    {
                        List<string> list = SET.DataSource as List<string>;
                        if (list.Contains(selection))
                        {
                            row.Cells[1].Value = selection;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Заносит выбранные (или обнулённые) селекшны к таскам в таймлайнер.
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                Collection<int> taskindex = row.Cells[0].Value as Collection<int>;
                if (row.Cells[2].Value != null)
                {
                    string setName = row.Cells[2].Value.ToString();
                    Core.Self.WriteTaskToTimeliner(taskindex, setName);
                }
                else
                {
                    Core.Self.WriteTaskToTimeliner(taskindex);
                }
            }
            this.Close();
        }
    }
}
