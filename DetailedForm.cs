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
        }

        void FillGridSelections()
        {
            List<string> dataSource = new List<string>();
            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                dataSource.Add(item.DisplayName);
            }
            this.SET.DataSource = dataSource;
        }

        void FillTasks()
        {
            List<string> dataSource = new List<string>();
            if (RootTask != null)
            {
                foreach (SavedItem item in RootTask.Children)
                {
                    dataSource.Add(item.DisplayName);
                }
                foreach (string str in dataSource)
                {
                    this.dataGridView1.Rows.Add(str);
                }
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    string taskName = row.Cells[0].Value.ToString();
                    string setName = row.Cells[1].Value.ToString();

                    int taskindex = RootTask.Children.IndexOfDisplayName(taskName);
                    if (taskindex != -1)
                    {
                        SelectionSourceCollection collection = UIform.Instance.getSelectionSourceByName(setName);
                        if (collection.Count != 0)
                        {
                            TimelinerTask task = RootTask.Children[taskindex].CreateCopy() as TimelinerTask;
                            task.Selection.CopyFrom(collection);
                            timeliner.TaskEdit(RootTask, taskindex, task);
                        }
                    }
                }
            }
        }
    }
}
