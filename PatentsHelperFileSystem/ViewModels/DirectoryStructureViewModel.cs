using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PatentsHelperFileSystem.ViewModels
{
    public class DirectoryStructureViewModel : INotifyPropertyChanged
    {

        public DirectoryStructureViewModel(string rootFolder)
        {
            RootFolder = rootFolder;
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();

                watcher.Path = RootFolder;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Created += Watcher_Created;
                watcher.Deleted += Watcher_Deleted;
                watcher.Renamed += Watcher_Renamed;

                watcher.EnableRaisingEvents = true;
            }
            catch { }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            OnPropertyChanged(nameof(Items));
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            OnPropertyChanged(nameof(Items));
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            OnPropertyChanged(nameof(Items));
        }

        public ObservableCollection<DirectoryItemViewModel> Items => new ObservableCollection<DirectoryItemViewModel>() { new DirectoryItemViewModel(RootFolder, DirectoryItemType.Folder) };

        public string RootFolder { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
