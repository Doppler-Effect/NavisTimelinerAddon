using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace NavisTimelinerPlugin
{
    public class MSProjectInterop
    {
        MSProject.Application application;
        MSProject.Project project;
        private bool launched = false;
        private ProgressForm pForm;
        private short taskDepth;

        public MSProjectInterop(ProgressForm progressForm)
        {
            this.LaunchMSProject();
            if (launched)
            {
                this.pForm = progressForm;
                this.pForm.Show();
            }
        }

        private void LaunchMSProject()
        {
            try
            {
                this.application = new MSProject.Application();
                application.Visible = true;
                System.Threading.Thread.Sleep(5000);
                application.Projects.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing);
                this.project = this.application.ActiveProject;
                this.launched = true;
                #region Example code from http://social.technet.microsoft.com/Forums/ru-RU/project2010custprog/thread/0d4b2b34-3051-4b27-bb51-e3cae64ac9f9
                //bool launched = false;
                //for (int timerSec = 0; timerSec < 20 && !launched; timerSec++)
                //{
                //    try
                //    {
                //        this.application.WorkOffline(false);
                //        launched = true;
                //        break;
                //    }
                //    catch
                //    {
                //    }
                //}

                //if (!launched)
                //{
                //    application.DocClose();
                //    System.Threading.Thread.Sleep(1500);
                //    application.Quit(MSProject.PjSaveType.pjDoNotSave);
                //    throw new ApplicationException("Unable to start an instance of MS Project");
                //} 
                #endregion
                               
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        
        public void AddTasks(List<TaskContainer> tasks)
        {
            this.taskDepth += 1;

            foreach (TaskContainer TC in tasks)
            {
                Autodesk.Navisworks.Api.Timeliner.TimelinerTask task = TC.Task;
                this.taskDepth = (short)(TC.HierarchyLevel - TaskContainer.MinHierarchyDepth + 1);

                MSProject.Task newTask = this.project.Tasks.Add(task.DisplayName, System.Type.Missing);

                //Задаём параметры свежесозданного таска
                if (task.PlannedStartDate != null)
                    newTask.Start = task.PlannedStartDate;
                if (task.PlannedEndDate != null)
                    newTask.Finish = task.PlannedEndDate;

                if (task.ActualStartDate != null)
                    newTask.ActualStart = task.ActualStartDate;
                if (task.ActualEndDate != null)
                    newTask.ActualFinish = task.ActualEndDate;

                if (task.User1 != null)
                    newTask.Text8 = task.User1;
                if (task.User2 != null)
                    newTask.Text9 = task.User2;
                if (task.User3 != null)
                    newTask.Text10 = task.User3;
                if (task.User9 != null)
                    newTask.Text11 = task.User9;
                if (task.User10 != null)
                    newTask.Text12 = task.User10;
                if (task.Selection != null)
                    newTask.Text4 = task.Selection.DisplayString;
                if (task.ProgressPercent != null)
                    newTask.PercentComplete = task.ProgressPercent;

                //

                newTask.OutlineLevel = this.taskDepth;
                pForm.Step();
            }
        }
    }
}
