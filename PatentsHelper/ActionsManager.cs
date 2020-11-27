using PatentsHelperCases;
using PatentsHelperDeadlines;
using PatentsHelperRn;
using PatentsHelperSettings;
using PatentsHelperUi;
using PatentsHelperUi.Popups;
using PatentsHelperWord;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PatentsHelper
{
    public static class ActionsManager
    {
        private static bool IsWindowOpen { get; set; } = false;

        public static void OpenCases()
        {
            if (IsWindowOpen) return;
            IsWindowOpen = true;
            var window = new MainWindow();
            window.Closed += Window_Closed;
            window.Show();
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            IsWindowOpen = false;
        }

        public static void OpenDeadlines()
        {
            if (IsWindowOpen) return;
            IsWindowOpen = true;

            var window = new MainWindow("Deadlines");
            window.Closed += Window_Closed;
            window.Show();
        }

        public static void OpenSettings()
        {
            if (IsWindowOpen) return;
            IsWindowOpen = true;
            var window = new MainWindow("Settings");
            window.Closed += Window_Closed;
            window.Show();
        }

        public static void OpenCurrentCaseInFileExplorer()
        {
            try
            {
                Process.Start(RootPath());
            }
            catch
            {
                MessageBox.Show("Cant open Case in file explorer");
            }
        }

        public static void OpenCasesInExcel()
        {
            try
            {
                CasesManager.OpenInExcel();
            }
            catch
            {
                var res = MessageBox.Show("Cases file not found, do you want to create it?", "file not found", MessageBoxButton.YesNoCancel);

                if (res == MessageBoxResult.Yes)
                {
                    CasesManager.CreateFile();
                    CasesManager.OpenInExcel();
                }
            }
        }

        public static void OpenDeadlinesInExcel()
        {
            try
            {
                DeadlinesManager.OpenInExcel();
            }
            catch
            {
                var res = MessageBox.Show("Deadlines file not found, do you want to create it?", "file not found", MessageBoxButton.YesNoCancel);

                if (res == MessageBoxResult.Yes)
                {
                    DeadlinesManager.CreateFile();
                    DeadlinesManager.OpenInExcel();
                }
            }
        }

        public static void OpenCurrentRNsInExcel()
        {
            var rnManger = new RnManager(new RnOptions { RootFolder = RootPath() });
            try
            {
                rnManger.OpenInExcel();
            }
            catch
            {
                var res = MessageBox.Show("File not found, Do you want to Create it?", "file not found", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes)
                {
                    rnManger.CreateFile();
                    rnManger.OpenInExcel();
                }
            }
        }

        public static void ReferenceNumeralAction()
        {
            var RangeManager = new RangeManager() { SelectedRange = Globals.ThisAddIn.Application.Selection.Range };

            if (!IsDocumentLoaded())
            {
                MessageBox.Show("A local Document is not loaded");
                return;
            }
            if (RangeManager.IsTextSelected())
            {
                new RefrenceNumeralsInput(RootPath(), RangeManager.SelectedRange.Text.Trim(), text =>
                 {
                     RangeManager.SetTextInRange(text);
                     RangeManager.CollapseCursorToEnd();
                 }).ShowDialog();
            }
            else
            {
                if (int.TryParse(RangeManager.SelectAndGetLastUnselectedWord(), out int rnNumber))
                {
                    var rnManger = new RnManager(new RnOptions { RootFolder = RootPath() });
                    var referenceNumerals = rnManger.GetReferenceNumerals();

                    if (referenceNumerals.ReferenceNumeralExists(rnNumber))
                    {
                        RangeManager.SetTextInRange(referenceNumerals.KeyValuePairs[rnNumber] + "\u00A0" + rnNumber);
                        RangeManager.CollapseCursorToEnd();
                    }

                }

            }
        }

        public static void UpdateCaseLastUsedTime()
        {
            if (IsDocumentLoaded())
            {
                CasesData casesData = new CasesData();
                casesData.LastTimeUsedCaseByLocation(RootPath());
            }
        }

        private static string RootPath()
        {
            try
            {
                return Globals.ThisAddIn.Application.ActiveDocument.Path;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool IsDocumentLoaded()
        {
            return Path.IsPathRooted(RootPath());
        }
    }
}
