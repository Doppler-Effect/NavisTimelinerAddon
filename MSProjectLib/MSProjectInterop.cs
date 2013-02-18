using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace MSProjectLib
{
    public class MSProjectInterop
    {
        MSProject.Application application;

        public MSProjectInterop()
        {
            this.LaunchMSProject();
        }

        private void LaunchMSProject()
        {
            try
            {
                this.application = new MSProject.Application();
                application.Visible = true;
                application.Projects.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing);
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
    }
}
