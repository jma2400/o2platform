// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using O2.External.WinFormsUI.O2Environment;

namespace O2.Tool.ViewAssessmentRun
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            //SpringExec.loadDefaultConfigFile();
            new O2DockPanel(typeof (Ascx.ascx_ViewAssessmentRun));
        }
    }
}
