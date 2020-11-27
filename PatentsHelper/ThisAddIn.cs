using PatentsHelperUi;
using System;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace PatentsHelper
{
    public partial class ThisAddIn
    {
        readonly HotkeyManager hotkeyManager = new HotkeyManager();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            var app = new App();
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            ModernWpfThemeManager.StartTheme();

            hotkeyManager.Subscribe(ActionsManager.ReferenceNumeralAction);

            Application.DocumentOpen += Application_DocumentOpen;


        }

        private void Application_DocumentOpen(Word.Document Doc)
        {
            ActionsManager.UpdateCaseLastUsedTime();
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            hotkeyManager.Unsubscribe();
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
