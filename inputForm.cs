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
    public partial class inputForm : Form
    {
        private TimelinerTask Task;
        string UniqueID;
        UIform thisParent;

        public inputForm(TimelinerTask Task, string ID, UIform parent)
        {
            this.Task = Task;
            this.UniqueID = ID;
            this.thisParent = parent;
            InitializeComponent();

            Dictionary<string,string> data = Core.Self.filesDB.Select(ID);
            if (data != null)
            {
                this.textBox1.Text = data["Value"];
                this.textBox2.Text = data["MaxValue"];
                this.comboBox1.Text = data["Units"];
            }
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(comboBox1.Text))
            {
                string value = textBox1.Text.removeLetters();
                string maxValue = textBox2.Text.removeLetters();
                string units = comboBox1.Text;

                Core.Self.filesDB.Insert(this.UniqueID, value, maxValue, units);
                Core.Self.CalculateTaskSummaryProgress(this.Task);

                thisParent.fillDataFromTask(Task);

                this.Close();
            }
        }

        private void inputForm_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Enter)
            {
                this.OKbutton_Click(sender, e);
            }
        }
    }
}
