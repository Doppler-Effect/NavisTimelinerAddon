using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;


namespace NavisTimelinerPlugin
{
    /// <summary>
    ////Контейнер для хранения обрабатываемого в данный момент таска и его отображения в интерфейсе.
    /// </summary>
    struct CurrenTimelinerTask
    {
        public TimelinerTask Task
        {
            get
            {
                return task;
            }
        }
        TimelinerTask task;

        public void update(TimelinerTask task, System.Windows.Forms.TextBox box)
        {
            box.Text = task.DisplayName;
            this.task = task;
        }
    }
}
