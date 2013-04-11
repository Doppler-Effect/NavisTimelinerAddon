using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Timeliner;
using Autodesk.Navisworks.Api.Plugins;

namespace NavisTimelinerPlugin
{
    [Plugin("TimelinerPlugin",
            "TKACH",
            ToolTip = "Plugin for associating model selections with timeliner tasks", 
            DisplayName="TimelinerPlugin")]

    public class TimelinerPlugin : AddInPlugin
    {
        static Document nDoc = Autodesk.Navisworks.Api.Application.ActiveDocument;

        public override int Execute(params string[] parameters)
        {
            UIform form = Core.Self.activeUIForm;
            if (form != null)
            {
                if (form.Visible == true)
                    form.Visible = false;
                else
                    form.Visible = true;
            }
            else
            {
                if (!nDoc.IsClear)
                {
                    form = new UIform();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Для работы с программой откройте проект", "Нет данных для работы", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }                
            }
            return 0;
        }
    }
}