using PatentsHelperFileSystem;
using PatentsHelperFileSystem.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PatentsHelperUi.UserControls
{
    /// <summary>
    /// Interaction logic for FolderTreeView.xaml
    /// </summary>
    public partial class FolderTreeView : UserControl, INotifyPropertyChanged
    {
        public FolderTreeView()
        {
            InitializeComponent();
            DataContext = this;
           
        }

        public void Load()
        {
            DirectoryStructureViewModel = new DirectoryStructureViewModel(RootFolder);
            OnPropertyChanged(nameof(OverlayVisibility));
            OnPropertyChanged(nameof(CreateFolderVisibility));
            OnPropertyChanged(nameof(TreeViewVisibility));
        }

        public DirectoryStructureViewModel DirectoryStructureViewModel { get; set; }


        public string Title => Path.GetFileName(RootFolder);

        public Visibility OverlayVisibility => string.IsNullOrWhiteSpace(RootFolder) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility CreateFolderVisibility => !string.IsNullOrEmpty(RootFolder) && !Directory.Exists(RootFolder) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility TreeViewVisibility => CreateFolderVisibility == Visibility.Collapsed && Directory.Exists(RootFolder) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand CreateFolderCommand => new RelayCommand<object>(() =>
        {
            try
            {
                Directory.CreateDirectory(RootFolder);
            }
            catch
            {
             
            }
            OnPropertyChanged(nameof(CreateFolderVisibility));
            OnPropertyChanged(nameof(TreeViewVisibility));

        });

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(FolderTreeView), new PropertyMetadata(string.Empty, new PropertyChangedCallback(CB)));



        public string RootFolder
        {
            get { return (string)GetValue(RootFolderProperty); }
            set
            {
                SetValue(RootFolderProperty, value);
            }
        }

        public static readonly DependencyProperty RootFolderProperty =
            DependencyProperty.Register("RootFolder", typeof(string), typeof(FolderTreeView), new PropertyMetadata(string.Empty, new PropertyChangedCallback(CB)));

        private static void CB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FolderTreeView)d).Load();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}




