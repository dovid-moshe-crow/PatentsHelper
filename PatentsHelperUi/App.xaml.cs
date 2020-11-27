using ModernWpf;
using ModernWpf.Controls;
using System.Windows;
namespace PatentsHelperUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly ElementTheme b = ElementTheme.Dark;
        readonly AnimationContext a = ModernWpf.Controls.AnimationContext.CollectionChangeAdd;

        public App()
        {
            InitializeComponent();
        }
    }
}
