using ClosedXML.Excel;
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PatentsHelperUi.Pages
{
    /// <summary>
    /// Interaction logic for CasesPage.xaml
    /// </summary>
    public partial class CasesPage : Page
    {
        public CasesPage(Window window)
        {
            InitializeComponent();

            var task = Task.Run(() => CasesVM.New(window));

            task.ContinueWith(c =>
            {
                Dispatcher.Invoke(() =>
                {
                    DataContext = c.Result;
                });
            });
        }
    }

    public enum SortOptions
    {
        [Description("By Last Used")]
        LAST_USED,
        [Description("By Newest")]
        NEWEST
    }



    public class CasesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static CasesVM New(Window window)
        {
            var vm = new CasesVM();
            vm.Window = window;
            vm.NewCaseCommand = new RelayCommand<object>(vm.NewCase);
            vm.OpenCasesInExcelCommand = new RelayCommand<object>(CasesManager.OpenInExcel);
            vm.ChangeCaseLocationCommand = new RelayCommand<object>(vm.ChangeCaseLocation);
            vm.RefreshCommand = new RelayCommand<object>(vm.Refresh);
            vm.CasesData = new CasesData();
            vm.ExcelCases = CasesManager.GetCasesDataTable();
            vm.EmailTemplates = OutlookManager.EmailTemplates;
            vm.SelectedEmailTemplate = vm.EmailTemplates?.FirstOrDefault();
            return vm;
        }

        public CasesVM()
        {

        }

        public void NewCase()
        {
            var cd = new NewCase() { Owner = Window };
            cd.ShowAsync().ContinueWith(res =>
            {
                if (res.IsCompleted)
                {
                    if (res.Result == ModernWpf.Controls.ContentDialogResult.Primary)
                    {
                        Window.Dispatcher.Invoke(() =>
                        {
                            Refresh();
                            ChangeCaseLocation(cd.CaseNumber);
                        });
                    }
                }

            });

        }



        public void Refresh()
        {
            ExcelCases = CasesManager.GetCasesDataTable();
            RefreshDeadlines();
        }

        public Window Window { get; set; }

        public ICommand NewCaseCommand { get; set; }

        public ICommand OpenCasesInExcelCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand ChangeCaseLocationCommand { get; set; }

     

        public ICommand CreateNewDeadlineCommand => new RelayCommand<object>(() =>
        {
            var caseName = SelectedCase?[0];
            if (!string.IsNullOrWhiteSpace(caseName?.ToString()))
            {
                new NewDeadline(caseName).ShowAsync().ContinueWith(res => 
                {
                    Window.Dispatcher.Invoke(() =>
                    {
                        RefreshDeadlines();
                    });
                });
                CasesData.LastTimeUsedCase(caseName?.ToString());
            }
        });

        public ICommand CreateSearchFolderCommand => new RelayCommand<object>(() =>
        {
            if (SelectedCase != null)
            {
                OutlookManager.AddSearchFolder(SelectedCase);
                CasesData.LastTimeUsedCase(SelectedCase?[0]?.ToString());
            }
        });

        public ICommand NewEmailCommand => new RelayCommand<object>(() =>
        {
            if (SelectedCase != null && SelectedEmailTemplate != null)
            {
                OutlookManager.EmailFromTemplate(SelectedCase, SelectedEmailTemplate.Path);
                CasesData.LastTimeUsedCase(SelectedCase?[0]?.ToString());
            }
        });

        public ICommand OpenCurrentCaseInFileExplorerCommand => new RelayCommand<object>(() =>
        {
            if (SelectedCase != null)
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

        public List<EmailTemplate> EmailTemplates { get; set; }

        public EmailTemplate SelectedEmailTemplate { get; set; }

        public void ChangeCaseLocation(string caseName)
        {
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
        }

        public void ChangeCaseLocation()
        {
            var caseName = SelectedCase?[0]?.ToString();
            ChangeCaseLocation(caseName);
        }

        public DataRow SelectedCase { get; set; }

        public string RootFolder => CasesData.GetCaseByName(SelectedCase?[0]?.ToString())?.Location;

        public CasesData CasesData { get; set; }

        public SortOptions SortOptions { get; set; }

        public string Filter { get; set; }

        public ExcelCases ExcelCases { get; set; }

        public ExcelDataTable CasesDataTable => ExcelCases.CasesTable;

        public IEnumerable<DataRow> CasesList
        {
            get
            {
                var filteredRows = CasesDataTable?.AsEnumerable().DefaultIfEmpty()
                    ?.Where(dr => dr.ItemArray.Any(s => string.IsNullOrEmpty(Filter) || Fuzz.PartialRatio(s.ToString()?.ToLower() ?? string.Empty, Filter?.ToLower()) > 90));

                switch (SortOptions)
                {
                    case SortOptions.LAST_USED:
                        filteredRows = filteredRows
                            .OrderByDescending(x => CasesData
                            .Where(c => c.Name.Equals(x[0]?.ToString(), StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault()?.LastTimeAccessed ?? DateTime.MinValue);
                        break;
                    case SortOptions.NEWEST:
                        filteredRows = filteredRows
                            .OrderByDescending(x => CasesData
                            .Where(c => c.Name.Equals(x[0]?.ToString(), StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault()?.DateCreated ?? DateTime.MinValue);
                        break;
                }

                return filteredRows;
            }
        }


        private void RefreshDeadlines()
        {
            ExcelDeadlines = DeadlinesManager.GetDeadlinesDataTable();
        }

        public ICommand RefreshDeadlinesCommand => new RelayCommand<object>(RefreshDeadlines);

        public ExcelDeadlines ExcelDeadlines { get; set; } = DeadlinesManager.GetDeadlinesDataTable();

        public List<Deadline> Deadlines => ExcelDeadlines.Deadlines(SelectedCase?[0]?.ToString());

        public Visibility DeadlinesOverlayVisibilty => Deadlines?.Any() == true ? Visibility.Collapsed : Visibility.Visible;

      
    }
}
