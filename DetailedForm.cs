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
        private static bool FreezeTreeUpdate;//нужно для запрета перерисовки TreeView в момент назначения таску набора.
        
        public DetailedForm(DocumentTimeliner timeliner, Document nDoc)
        {
            FreezeTreeUpdate = false;

            InitializeComponent();
            this.timeliner = timeliner;
            this.nDoc = nDoc;
            
            FillSelectionSets();
            Core.Self.FillTreeViewWithTasks(this.treeView1);

            timeliner.Changed += new EventHandler<SavedItemChangedEventArgs>(this.Timeliner_Changed);
        }

        /// <summary>
        /// Заполняет ListView названиями селекшн сетов документа.
        /// </summary>
        void FillSelectionSets()
        {
            this.listBox1.Items.Add("NULL");
            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                this.listBox1.Items.Add(item.DisplayName);
            }
        }
        
        /// <summary>
        /// По клику вносит изменения в Timeliner.
        /// </summary>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null)
            {
                Collection<int> index = node.Tag as Collection<int>;
                string selSet = listBox1.SelectedItem.ToString();
                if (selSet == "NULL")
                {
                    selSet = null;
                }

                FreezeTreeUpdate = true;
                Core.Self.WriteTaskToTimeliner(index, selSet);
                FreezeTreeUpdate = false;

                node.BackColor = System.Drawing.Color.LawnGreen;
                treeView1.SelectedNode = null;
            }
        }

        /// <summary>
        /// Если у таска есть прикреплённый набор - он подсвечивается при выборе таска.
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            Collection<int> index = node.Tag as Collection<int>;
            TimelinerTask task = timeliner.TaskResolveIndexPath(index);
            if (task.Selection.IsClear)
            {
                listBox1.SelectedItem = "NULL";
            }
            else
            {
                string SetName = Core.Self.FindSelectionSetName(task);
                listBox1.SelectedItem = SetName;
            }
        }

        private void Timeliner_Changed(object sender, EventArgs e)
        {
            if (!FreezeTreeUpdate)
                this.Close();
        }

        private void DetailedForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UIform.Instance.Visible = true;
        }
    }
}
