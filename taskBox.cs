using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisTimelinerPlugin
{
    class taskBox : System.Windows.Forms.TextBox
    {
        public TimelinerTask Task
        {
            get
            {
                return task;
            }
        }
        TimelinerTask task;

        public int TaskIndex
        {
            get
            {
                return taskIndex;
            }
        }
        int taskIndex;

        public void addTask(TimelinerTask input, int index)
        {
            this.task = input;
            this.taskIndex = index;
            this.Text = input.DisplayName;
        }
    }
}
