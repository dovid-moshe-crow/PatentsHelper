using FuzzySharp;
using PatentsHelperCases;
using PatentsHelperDeadlines;
using PatentsHelperFileSystem;
using PatentsHelperOutlook;
using PatentsHelperSettings;
using PatentsHelperUi.ContentDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PatentsHelperUi.Pages
{

    public partial class DeadlinesPage : Page
    {
        public DeadlinesPage()
        {
            InitializeComponent();

            var task = Task.Run(() => DeadlinesVM.New());

            task.ContinueWith(c =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (c.IsFaulted)
                    {
                        MessageBox.Show(c.Exception?.ToString());
                        return;
                    }
                    try
                    {
                        DataContext = c.Result;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                });
            });
        }
    }

    public class DeadlinesVM : INotifyPropertyChanged
    {

        public static DeadlinesVM New()
        {
            var deadlinesVm = new DeadlinesVM();
            deadlinesVm.ExcelDeadlines = DeadlinesManager.GetDeadlinesDataTable();
            deadlinesVm.ExcelCases = CasesManager.GetCasesDataTable();
            deadlinesVm.CasesData = new CasesData();
            deadlinesVm.EmailTemplates = OutlookManager.EmailTemplates;
            deadlinesVm.SelectedEmailTemplate = deadlinesVm.EmailTemplates?.FirstOrDefault();
            return deadlinesVm;
        }

        public string Filter { get; set; }

        public ExcelDeadlines ExcelDeadlines { get; set; }

        private DataTable filterdDeadlinesDataTable;

        public DataView DeadlinesList
        {
            get
            {
                if (ExcelDeadlines?.DeadlinesTable == null) return null;

                if (filterdDeadlinesDataTable == null)
                {
                    var deadlinesColumnsToShow = UserSettings.DeadlinesColumnsToShow;
                    string[] headers = ExcelDeadlines.DeadlinesTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).Where((c, i) => deadlinesColumnsToShow.Contains(c) || i == 0).ToArray();
                    filterdDeadlinesDataTable = ExcelDeadlines?.DeadlinesTable.DefaultView.ToTable(false, headers);
                }


                var rows = filterdDeadlinesDataTable?.AsEnumerable()
                    ?.DefaultIfEmpty()
                    ?.Where(dr => dr.ItemArray.Any(s => string.IsNullOrEmpty(Filter) || Fuzz.PartialRatio(s.ToString()?.ToLower() ?? string.Empty, Filter?.ToLower()) > 90));
                if (rows?.Any() == true)
                {
                    return rows.CopyToDataTable().AsDataView();
                }
                else
                {
                    return null;
                }
            }
        }

        public ExcelCases ExcelCases { get; set; }

        public DataTable CasesDataTable => ExcelCases?.CasesTable;

        public DataRow CurrentCase
        {
            get
            {
                if(SelectedDeadline == null)
                {
                    return null;
                }
                else
                {
                    var caseId = SelectedDeadline.Row[0]?.ToString();
                    return CasesDataTable.AsEnumerable().Where(r => r[0]?.ToString()?.Equals(caseId) == true).FirstOrDefault();
                }
            }
        }

        public ICommand NewEmailCommand => new RelayCommand<object>(() =>
        {
            if (CurrentCase != null && SelectedEmailTemplate != null)
            {
                OutlookManager.EmailFromTemplate(CurrentCase, SelectedEmailTemplate.Path);
                CasesData.LastTimeUsedCase(CurrentCase?[0]?.ToString());
            }
        });

        public ICommand OpenCurrentCaseInFileExplorerCommand => new RelayCommand<object>(() =>
        {
            if (CurrentCase != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(RootFolder);
                }
                catch
                {

                }
            }
        });

        public ICommand ChangeCaseLocationCommand => new RelayCommand<object>(() =>
        {
            var caseName = CurrentCase?[0]?.ToString();
            if (!string.IsNullOrWhiteSpace(caseName))
            {
                new FolderPicker(caseName).ShowAsync().ContinueWith(res =>
                {
                    if (res.IsCompleted)
                    {
                        if (res.Result == ModernWpf.Controls.ContentDialogResult.Primary)
                        {
                            CasesData = new CasesData();
                        }
                    }
                });
            }
        });

        public bool OpenCaseInFileExplorerEnabled => CurrentCase != null && Directory.Exists(RootFolder);

        public List<EmailTemplate> EmailTemplates { get; set; }

        public EmailTemplate SelectedEmailTemplate { get; set; }

        public string RootFolder => CasesData.GetCaseByName(CurrentCase?[0]?.ToString())?.Location;

        public CasesData CasesData { get; set; }


        public DataRowView SelectedDeadline { get; set; }

        public ICommand OpenInExcelCommand => new RelayCommand<object>(DeadlinesManager.OpenInExcel);

        public ICommand RefreshCommand => new RelayCommand<object>(() =>
        {
            filterdDeadlinesDataTable = null;
            ExcelDeadlines = DeadlinesManager.GetDeadlinesDataTable();
            ExcelCases = CasesManager.GetCasesDataTable();
            CasesData = new CasesData();
        });

        public bool OutlookActionsEnabled => CurrentCase != null;
        public bool LinkCaseButtonEnabled => CurrentCase != null;

        public List<ColumnVisiblity> ColumnsVisiblity => ExcelDeadlines.DeadlinesTable.Columns.Cast<DataColumn>().Skip(1).ToList()
            .ConvertAll(c => new ColumnVisiblity
            {
                Name = c.ColumnName,
                UserSettings = UserSettings,
            });

        public class ColumnVisiblity : INotifyPropertyChanging
        {
            public string Name { get; set; }
            public UserSettings UserSettings { get; set; }
            public bool IsChecked
            {
                get
                {
                    return UserSettings.DeadlinesColumnsToShow.Contains(Name);
                }
                set
                {
                    if (value == true)
                    {
                        if (!UserSettings.DeadlinesColumnsToShow.Contains(Name))
                        {
                            UserSettings.DeadlinesColumnsToShow.Add(Name);
                            UserSettings.Save();
                        }
                    }
                    else
                    {
                        UserSettings.DeadlinesColumnsToShow.Remove(Name);
                        UserSettings.Save();
                    }
                }
            }


            public event PropertyChangingEventHandler PropertyChanging;
        }

        public static UserSettings UserSettings { get; } = new UserSettings();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
