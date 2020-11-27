using FuzzySharp;
using PatentsHelperDeadlines;
using PatentsHelperFileSystem;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                    DataContext = c.Result;
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
            return deadlinesVm;
        }
        
        public string Filter { get; set; }

        public ExcelDeadlines ExcelDeadlines { get; set; }

        public DataView DeadlinesList => ExcelDeadlines.DeadlinesTable?.AsEnumerable()?.DefaultIfEmpty()
            ?.Where(dr => dr.ItemArray.Any(s => string.IsNullOrEmpty(Filter) || Fuzz.PartialRatio(s.ToString()?.ToLower() ?? string.Empty, Filter?.ToLower()) > 90)).CopyToDataTable().AsDataView();

        public ICommand OpenInExcelCommand => new RelayCommand<object>(DeadlinesManager.OpenInExcel);

        public ICommand RefreshCommand => new RelayCommand<object>(() =>
        {
            ExcelDeadlines = DeadlinesManager.GetDeadlinesDataTable();
        });

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
