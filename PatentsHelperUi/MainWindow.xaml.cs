using ModernWpf.Controls;
using PatentsHelperUi.Pages;
using System.Runtime.InteropServices;
using System.Windows;

namespace PatentsHelperUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow([Optional] string openPage)
        {
            InitializeComponent();
            if (openPage != null)
            {
                GoToPage(openPage);
            }
            else  
            {
                GoToPage(CASES_TAG);
            }
        }

        private const string CASES_TAG = "Cases";
        private const string DEADLINES_TAG = "Deadlines";
        private const string SETTINGS_TAG = "Settings";

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            sender.IsPaneOpen = false;
            if (args.IsSettingsSelected)
            {
                mainFrame.Navigate(new Pages.SettingsPage());
            }
            else
            {
                string tag = ((NavigationViewItem)args.SelectedItem)?.Tag?.ToString();

                GoToPage(tag);
            }
        }

        private CasesPage CasesPage;
        private DeadlinesPage DeadlinesPage;
        private SettingsPage SettingsPage;

        private void GoToPage(string page)
        {
            switch (page)
            {
                case CASES_TAG:
                    if(CasesPage == null)
                    {
                        CasesPage = new CasesPage(this);
                    }
                    mainFrame.Navigate(CasesPage);
                    break;
                case DEADLINES_TAG:
                    if (DeadlinesPage == null)
                    {
                        DeadlinesPage = new DeadlinesPage();
                    }
                    mainFrame.Navigate(DeadlinesPage);
                    break;
                case SETTINGS_TAG:
                    if (SettingsPage == null)
                    {
                        SettingsPage = new SettingsPage();
                    }
                    mainFrame.Navigate(SettingsPage);
                    break;
                default:
                    break;
            }
        }
    }
}
