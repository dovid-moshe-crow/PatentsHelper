using PatentsHelperUi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace PatentsHelper
{
    public partial class ThisAddIn
    {
        readonly HotkeyManager hotkeyManager = new HotkeyManager();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {  
             
            Thread thread = new Thread(() =>
            {
                var app = new App();
                app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

              /*  var w = new MainWindow();

                w.Visibility = Visibility.Collapsed;
                w.WindowState = WindowState.Minimized;
                w.ShowInTaskbar = false;

                w.Show();
                w.Close();*/

                ModernWpfThemeManager.StartTheme();

            });

            hotkeyManager.Subscribe(ActionsManager.ReferenceNumeralAction);

            Application.DocumentOpen += Application_DocumentOpen;

            thread.ApartmentState = ApartmentState.STA;
            thread.Start();

        }


        private void Application_DocumentOpen(Word.Document Doc)
        {
            ActionsManager.UpdateCaseLastUsedTime();
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            try
            {
                hotkeyManager.Unsubscribe();
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Shutdown();
                }
            }
            catch
            {

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
