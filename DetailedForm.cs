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
    public partial class DetailedForm : Form
    {
        DocumentTimeliner timeliner;
        TimelinerTask RootTask;
        Document nDoc;

        public DetailedForm(DocumentTimeliner timeliner, Document nDoc, TimelinerTask RootTask)
        {
            InitializeComponent();
            this.timeliner = timeliner;
            this.nDoc = nDoc;
            this.RootTask = RootTask;
            FillGridSelections();
            FillTasks();
            FillSelections();
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
            if (RootTask != null)
            {
                foreach (SavedItem item in RootTask.Children)
                {
                    this.dataGridView1.Rows.Add(item.DisplayName);
                }
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
                int taskNo = RootTask.Children.IndexOfDisplayName(taskName);
                if (taskNo != -1)
                {
                    string selection = UIform.Instance.findSelectionSetName(RootTask.Children[taskNo] as TimelinerTask);
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
                string taskName = row.Cells[0].Value.ToString();
                int taskindex = RootTask.Children.IndexOfDisplayName(taskName);

                if (taskindex != -1)
                {
                    if (row.Cells[1].Value != null)
                    {
                        string setName = row.Cells[1].Value.ToString();
                        SelectionSourceCollection collection = UIform.Instance.getSelectionSourceByName(setName);
                        if (collection.Count != 0)
                        {
                            TimelinerTask task = RootTask.Children[taskindex].CreateCopy() as TimelinerTask;
                            task.Selection.CopyFrom(collection);
                            timeliner.TaskEdit(RootTask, taskindex, task);
                        }
                    }
                    else
                    {
                        TimelinerTask task = RootTask.Children[taskindex].CreateCopy() as TimelinerTask;
                        task.Selection.Clear();
                        timeliner.TaskEdit(RootTask, taskindex, task);
                    }
                }
            }
            this.Close();
        }
    }
}
