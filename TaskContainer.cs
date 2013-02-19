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
    public class TaskContainer
    {
        static Document nDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;
        static Autodesk.Navisworks.Api.DocumentParts.IDocumentTimeliner Itimeliner = nDoc.Timeliner;
        static DocumentTimeliner timeliner = (DocumentTimeliner)Itimeliner;

        static int maxHierarchyDepth = 0;
        static int minHierarchyDepth = int.MaxValue;
        public static int MaxHierarchyDepth
        {
            get { return maxHierarchyDepth; }
        }
        public static int MinHierarchyDepth
        {
            get { return minHierarchyDepth; }
        }
        
        public List<TaskContainer> Children;

        public Collection<int> Index
        {
            get { return index; }
        }
        public string TaskName
        {
            get { return taskname; }
        }
        public TimelinerTask Task
        {
            get
            {
                TimelinerTask task = timeliner.TaskResolveIndexPath(this.index);
                return task;
            }
        }        
        public int HierarchyLevel
        {
            get
            {
                return this.index.Count;
            }
        }//"глубина" расположения таска в иерархии - нужно для последующего создания иерархии treeview.

        private Collection<int> index;
        private string taskname;

        public TaskContainer(string task, Collection<int> i)
        {
            this.index = i;
            this.taskname = task;
            if (this.HierarchyLevel > maxHierarchyDepth)
            {
                maxHierarchyDepth = this.HierarchyLevel;
            }
            if (this.HierarchyLevel < minHierarchyDepth)
            {
                minHierarchyDepth = this.HierarchyLevel;
            }
            Children = new List<TaskContainer>();
        }        
    }
}
