using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public override int Execute(params string[] parameters)
        {
            UIform form = UIform.Instance;
            if (form != null)
            {
                if (form.Visible == true)
                    form.Visible = false;
                else
                    form.Visible = true;                
            }
            return 0;
        }
    }
}
