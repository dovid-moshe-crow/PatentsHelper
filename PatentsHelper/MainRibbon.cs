using Microsoft.Office.Tools.Ribbon;

namespace PatentsHelper
{
    public partial class MainRibbon
    {
        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void MainButton_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenCases();
        }

        private void CasesSplitButton_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenCases();
        }

        private void OpenCasesInExcel_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenCasesInExcel();
        }

        private void OpenCurrentCaseInFileExplorer_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenCurrentCaseInFileExplorer();
        }

        private void OpenDeadlinesInExcel_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenDeadlinesInExcel();
        }

        private void DeadlinesSplitButton_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenDeadlines();
        }

        private void OpenCurrentRnInExcel_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenCurrentRNsInExcel();
        }

        private void SettingsButton_Click(object sender, RibbonControlEventArgs e)
        {
            ActionsManager.OpenSettings();
        }
    }
}
