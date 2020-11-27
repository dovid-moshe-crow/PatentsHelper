﻿using PatentsHelperFileSystem;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace PatentsHelperSettings
{

    public class UserSettings : INotifyPropertyChanged
    {
      
        public int ReferenceNumeralIncrement
        {
            get => Properties.Settings.Default.ReferenceNumeralIncrement;
            set
            {
                Properties.Settings.Default.ReferenceNumeralIncrement = value;
                OnPropertyChanged(nameof(ReferenceNumeralIncrement));
            }
        }

        public ICommand SelectCasesRootFolderCommand => new RelayCommand<object>(() =>
        {
            var path = FsUtils.PickFolder(CasesRootFolder);
            if (!string.IsNullOrEmpty(path))
            {
                CasesRootFolder = path + @"\";
                OnPropertyChanged(nameof(CasesRootFolder));
            }
        });

        public List<string> DateFormats
        {
            get
            {
                List<string> dateFormats = new List<string>();

                foreach (var region in CultureInfo.GetCultures(CultureTypes.SpecificCultures).ToList())
                {
                    CultureInfo dateFormat = new CultureInfo(region.Name);
                    if (!dateFormats.Contains(dateFormat.DateTimeFormat.ShortDatePattern))
                    {
                        dateFormats.Add(dateFormat.DateTimeFormat.ShortDatePattern);
                    }
                }

                return dateFormats;
            }
        }


        public string DateFormat
        {
            get => Properties.Settings.Default.DateFormat;
            set
            {
                Properties.Settings.Default.DateFormat = value;
                OnPropertyChanged(nameof(DateFormat));
            }
        }


        public string SearchFolderScope
        {
            get => Properties.Settings.Default.SearchFolderScope;
            set
            {
                Properties.Settings.Default.SearchFolderScope = value;
                OnPropertyChanged(nameof(SearchFolderScope));
            }
        }

        public List<string> ShortcutsFirst
        {
            get { return new List<string> { Keys.Control.ToString(), Keys.Alt.ToString() }; }
        }

        public List<string> ShortcutsLast
        {
            get { return Enumerable.Range('A', 26).Select((num) => ((char)num).ToString()).ToList(); }
        }

        public string RnShortcutFirst
        {
            get => Properties.Settings.Default.RnShortcutFirst;
            set
            {
                Properties.Settings.Default.RnShortcutFirst = value;
                OnPropertyChanged(nameof(RnShortcutFirst));
            }
        }

        public string RnShortcutLast
        {
            get => Properties.Settings.Default.RnShortcutLast;
            set
            {
                Properties.Settings.Default.RnShortcutLast = value;
                OnPropertyChanged(nameof(RnShortcutLast));
            }
        }

        public string CasesRootFolder
        {
            get => Properties.Settings.Default.CasesRootFolder;
            set
            {
                Properties.Settings.Default.CasesRootFolder = value;
                OnPropertyChanged(nameof(CasesRootFolder));
            }
        }

        public string CasesEmailsColumns
        {
            get => Properties.Settings.Default.CasesEmailsColumns;
            set
            {
                Properties.Settings.Default.CasesEmailsColumns = value;
                OnPropertyChanged(nameof(CasesEmailsColumns));
            }
        }

        /// <summary>
        /// 0: light
        /// 1: dark
        /// </summary>
        public int AppTheme
        {
            get => Properties.Settings.Default.AppTheme;
            set
            {
                Properties.Settings.Default.AppTheme = value;
                OnPropertyChanged(nameof(AppTheme));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Properties.Settings.Default.Save();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
