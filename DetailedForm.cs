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
            TreeNode nullNode = new TreeNode("БЕЗ НАБОРА");
            nullNode.Tag = "NULL";
            treeView2.Nodes.Add(nullNode);

            foreach (SavedItem item in nDoc.SelectionSets.Value)
            {
                TreeNode node = new TreeNode(item.DisplayName);
                node.Tag = item.DisplayName;
                this.treeView2.Nodes.Add(node);
                FillSelectionSetChildren(item, node);
            }
        }
        private void FillSelectionSetChildren(SavedItem parentItem, TreeNode parentNode)
        {
            if (parentItem.IsGroup)
            {
                foreach (SavedItem item in ((GroupItem)parentItem).Children)
                {
                    TreeNode node = new TreeNode(item.DisplayName);
                    node.Tag = item.DisplayName;
                    parentNode.Nodes.Add(node);
                    FillSelectionSetChildren(item, node);
                }
            }
        }
        
        /// <summary>
        /// По клику вносит изменения в Timeliner.
        /// </summary>
        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node != null)
            {
                Collection<int> index = node.Tag as Collection<int>;
                //string selSet = listBox1.SelectedItem.ToString();
                string selSet = treeView2.SelectedNode.Tag.ToString();
                if (selSet == "NULL")
                {
                    selSet = null;
                }

                FreezeTreeUpdate = true;
                Core.Self.WriteTaskToTimeliner(index, selSet);
                FreezeTreeUpdate = false;

                node.BackColor = System.Drawing.Color.LawnGreen;

                if (treeView1.SelectedNode.NextVisibleNode != null)
                    treeView1.SelectedNode = treeView1.SelectedNode.NextVisibleNode;
                else
                    treeView1.SelectedNode = null;
            }
        }

        /// <summary>
        /// Если у таска есть прикреплённый набор - он подсвечивается при выборе таска.
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode n in Core.Self.getAllNodes(treeView2))
            {
                n.BackColor = System.Drawing.Color.White;
            }

            TreeNode node = treeView1.SelectedNode;
            Collection<int> index = node.Tag as Collection<int>;
            TimelinerTask task = timeliner.TaskResolveIndexPath(index);
            if (task.Selection.IsClear)
            {
                SelectTreeNode("NULL");
            }
            else
            {
                string SetName = Core.Self.FindSelectionSetName(task);
                SelectTreeNode(SetName);
            }

            treeView2.SelectedNode.BackColor = System.Drawing.Color.Orange;
        }

        /// <summary>
        /// Подсвечивает нужную ноду в дереве по тэгу ноды
        /// </summary>
        private void SelectTreeNode(string tag)
        {
            foreach (TreeNode node in Core.Self.getAllNodes(treeView2))
            {
                if (node.Tag.ToString() == tag)
                {
                    treeView2.SelectedNode = node;
                    break;
                }
            }
        }

        private void Timeliner_Changed(object sender, EventArgs e)
        {
            if (!FreezeTreeUpdate)
                this.Close();
        }

        private void DetailedForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.Self.activeUIForm.Visible = true;
        }
    }
}
