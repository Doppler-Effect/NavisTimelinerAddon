using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisTimelinerPlugin
{
    [Serializable()]
    class TaskContainer
    {
        public Collection<int> Index
        {
            get
            {
                return index;
            }
        }
        public TimelinerTask Task
        {
            get
            {
                return task;
            }
        }
        //public bool OKstatus
        //{
        //    get
        //    {
        //        int id = 0;
        //        if(int.TryParse(this.task.DisplayId, out id))
        //        {
        //            if (id == this.index)
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //}

        Collection<int> index;
        TimelinerTask task;

        public TaskContainer(TimelinerTask task, Collection<int> i)
        {
            this.index = i;
            this.task = task;            
        }        
    }
}
