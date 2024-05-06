using PatentsHelperSettings;
using System.Threading;
using System.Windows.Controls;

namespace PatentsHelperUi.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            DataContext = new UserSettings();

        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var themeId = (sender as ModernWpf.Controls.RadioButtons).SelectedIndex;

      
                // ModernWpfThemeManager.StartTheme(themeId);

               





        }
    }
}
