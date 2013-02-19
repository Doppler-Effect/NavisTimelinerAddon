using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NavisTimelinerPlugin
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(int taskCount)
        {
            InitializeComponent();
            this.progressBar1.Maximum = taskCount;
            this.progressBar1.Step = 1;
        }

        public void Step()
        {            
            this.progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum)
                this.Close();
        }

        
    }
}
