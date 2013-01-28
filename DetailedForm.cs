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
            FillSelectionSets();
            FillTasks();
            //FillSelections();
        }

        /// <summary>
        /// Заполняет комбобоксы второй колонки значениями (названиями селекшн сетов документа).
        /// </summary>
        void FillSelectionSets()
        {
            List<string> dataSource = new List<string>();
            dataSource.Add("");
            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                dataSource.Add(item.DisplayName);
            }

            foreach (string SetName in dataSource)
            {
                this.listBox1.Items.Add(SetName);
            }
        }

        /// <summary>
        /// Заполняет первую колонку именами тасков.
        /// </summary>
        void FillTasks()
        {
            treeView1.Nodes.Add("Root");
            treeView1.SelectedNode = treeView1.Nodes["Root"];
            //foreach (TaskContainer tc in Core.Self.Tasks)
            //{  
            //    treeView1.Nodes.Add(tc.Index.ToString(), tc.TaskName);
            //}
            foreach (TaskContainer tc in Core.Self.Tasks)
            {

            }
        }

        /// <summary>
        /// Выбирает во второй колонке уже назначенные селекшны, если у соответствующих тасков они есть.
        /// </summary>
        void FillSelections()
        {
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        string taskName = row.Cells[1].Value.ToString();
        //        if (row.Cells[0].Value != null)
        //        {
        //            Collection<int> taskIndex = row.Cells[0].Value as Collection<int>;
        //            TimelinerTask task = timeliner.TaskResolveIndexPath(taskIndex);
        //            string selection = Core.Self.findSelectionSetName(task);
        //            if (selection != null)
        //            {
        //                List<string> list = SET.DataSource as List<string>;
        //                if (list.Contains(selection))
        //                {
        //                    row.Cells[2].Value = selection;
        //                }
        //            }
        //        }
        //    }
        }

        /// <summary>
        /// Заносит выбранные (или обнулённые) селекшны к таскам в таймлайнер.
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
        //    this.UseWaitCursor = true;
        //    this.progressBar1.Refresh();
        //    this.progressBar1.Style = ProgressBarStyle.Continuous;
        //    this.progressBar1.Maximum = dataGridView1.Rows.Count;
        //    foreach(DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        Collection<int> taskindex = row.Cells[0].Value as Collection<int>;
        //        if (row.Cells[2].Value != null)
        //        {
        //            string setName = row.Cells[2].Value.ToString();
        //            Core.Self.WriteTaskToTimeliner(taskindex, setName);
        //            this.progressBar1.PerformStep();
        //        }
        //        else
        //        {
        //            Core.Self.WriteTaskToTimeliner(taskindex);
        //            this.progressBar1.PerformStep();
        //        }
        //    }
        //    this.Close();
        //    this.UseWaitCursor = false;
        }
    }
}
