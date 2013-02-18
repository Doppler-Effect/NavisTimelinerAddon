using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace MSProjectLib
{
    public class MSProjectInterop
    {
        MSProject.Application application;

        public void LaunchProject()
        {
            this.application = new MSProject.Application();
            bool launched = false;

            for (int timerSec = 0; timerSec < 20 && !launched; timerSec++)
            {
                try
                {
                    this.application.WorkOffline(false);
                    launched = true;
                    break;
                }
                catch
                {
                }
            }

            if (!launched)
            {
                application.DocClose();
                System.Threading.Thread.Sleep(1500);
                application.Quit(MSProject.PjSaveType.pjDoNotSave);
                throw new ApplicationException("Unable to start an instance of MS Project");
            }
        }
    }
}
