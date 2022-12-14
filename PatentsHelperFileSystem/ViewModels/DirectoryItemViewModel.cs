using PatentsHelperFileSystem.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace PatentsHelperFileSystem.ViewModels
{
    public class DirectoryItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DirectoryItemType Type { get; set; }

        public ImageSource Icon
        {
            get
            {
                if (Type == DirectoryItemType.File)
                {
                    return System.Drawing.Icon.ExtractAssociatedIcon(FullPath).ToImageSource();
                }
                else
                {
                    if (IsExpanded)
                    {
                        return Properties.Resources.icons8_opened_folder_50.ToImageSource();
                    }
                    else
                    {
                        return Properties.Resources.icons8_folder_50.ToImageSource();
                    }
                }
            }
        }


        public string FullPath { get; set; }

        public string Name { get; set; }

        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }


        public bool CanExpand => Type == DirectoryItemType.Folder && !IsDirectoryEmpty;

        public bool IsDirectoryEmpty
        {
            get
            {
                try
                {
                    return !Directory.EnumerateFileSystemEntries(FullPath).Any();
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool IsExpanded
        {
            get
            {
                return Children?.Count(f => f != null) > 0;
            }
            set
            {
                if (value == true)
                {
                    Expand();
                }
                else
                {
                    ClearChildren();
                }
            }
        }

        public ICommand ExpandCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            ExpandCommand = new RelayCommand<object>(Expand);
            OpenCommand = new RelayCommand<object>(Open);
            FullPath = fullPath;
            Type = type;
            Name = Path.GetFileName(FullPath);
            ClearChildren();
        }

        private void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemViewModel>();

            if (Type != DirectoryItemType.File)
                Children.Add(null);
        }

        private void Expand()
        {
            if (Type == DirectoryItemType.File)
            {
                return;
            }

            var children = DirectoryStructure.GetDirectoryItems(FullPath);

            Children = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
        }

        private void Open()
        {
            try
            {
                Process.Start(FullPath);
            }
            catch { }
        }
    }
}
