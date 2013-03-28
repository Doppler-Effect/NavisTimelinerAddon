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
            }
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string value = textBox1.Text.removeLetters();
                string maxValue = "100";
                string units = "%";
                double dValue;
                if (double.TryParse(value, out dValue))
                {
                    if (dValue > 100)
                    {
                        MessageBox.Show("Введённое значение больше 100 %");
                    }
                    else if (dValue < 0)
                    {
                        MessageBox.Show("Введено отрицательное значение!");
                    }
                    else
                    {
                        Core.Self.filesDB.Insert(this.UniqueID, value, maxValue, units);
                        Core.Self.CalculateTaskSummaryProgress(this.Task);
                        thisParent.fillDataFromTask(Task);
                        this.Close();
                    }                        
                }
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
