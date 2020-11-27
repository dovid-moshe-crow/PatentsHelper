using ModernWpf.Controls;
using PatentsHelperFileSystem;
using PatentsHelperSettings;
using System.ComponentModel;
using System.Windows.Input;

namespace PatentsHelperUi.ContentDialogs
{
    /// <summary>
    /// Interaction logic for FolderPicker.xaml
    /// </summary>
    public partial class FolderPicker : ContentDialog
    {
        public FolderPicker(string caseName)
        {
            InitializeComponent();

            FolderPickerVM = new FolderPickerVM(caseName); ;

            DataContext = FolderPickerVM;
        }


        public FolderPickerVM FolderPickerVM { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            FolderPickerVM.Save(args);
        }
    }

    public class FolderPickerVM : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FolderPickerVM(string caseName)
        {
            CaseName = caseName;
            CasesData = new CasesData();
            Location = CasesData.GetCaseByName(CaseName)?.Location;
        }

        public CasesData CasesData { get; set; }

        public string CaseName { get; set; }

        public string Location { get; set; }

        public bool HasError { get; set; }

        public ICommand PickFolderCommand => new RelayCommand<object>(() =>
        {
            Location = FsUtils.PickFolder(new UserSettings().CasesRootFolder);
        });

        public void Save(ContentDialogButtonClickEventArgs args)
        {
            if (HasError)
            {
                args.Cancel = true;
                return;
            }
            CasesData.AddOrUpdateCase(CaseName, Location);
        }


        private string Validate(string propertyName)
        {
            string result = string.Empty;
            HasError = true;
            if (propertyName == "Location")
            {
                if (string.IsNullOrWhiteSpace(Location))
                {
                    result = "Path cant be empty";
                }
                else if (!FsUtils.IsFilePathValid(Location))
                {
                    result = "Path is not valid";
                }
                else if (CasesData.LocationExistsInDiffrentCase(CaseName, Location))
                {
                    result = "Path already exists in a diffrent case";
                }
                else
                {
                    HasError = false;
                }

            }

            return result;
        }

        public string Error => null;

        public string this[string name]
        {
            get
            {
                return Validate(name);
            }
        }
    }
}
