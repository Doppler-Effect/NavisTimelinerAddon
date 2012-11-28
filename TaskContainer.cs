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
    class TaskContainer
    {
        static Document nDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        static Autodesk.Navisworks.Api.DocumentParts.IDocumentTimeliner Itimeliner = nDoc.Timeliner;
        static DocumentTimeliner timeliner = (DocumentTimeliner)Itimeliner;

        public Collection<int> Index
        {
            get
            {
                return index;
            }
        }
        public string TaskName
        {
            get
            {
                return taskname;
            }
        }
        public TimelinerTask Task
        {
            get
            {
                TimelinerTask task = timeliner.TaskResolveIndexPath(this.index);
                return task;
            }
        }

        Collection<int> index;
        string taskname;

        public TaskContainer(string task, Collection<int> i)
        {
            this.index = i;
            this.taskname = task;            
        }        
    }
}
