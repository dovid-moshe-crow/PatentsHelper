using PatentsHelperRn;
using PatentsHelperUi.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PatentsHelperUi.Popups
{
    /// <summary>
    /// Interaction logic for RefrenceNumeralsInput.xaml
    /// </summary>
    public partial class RefrenceNumeralsInput : Window
    {
        public RefrenceNumeralsInput(string rootFolder, string rnValue, [Optional] Action<string> addRnToDocument)
        {
            InitializeComponent();

            Loaded += (object sender, RoutedEventArgs e) =>
             {
                 var task = Task.Run(() => ReferenceNumeralsVM.New(rootFolder, rnValue, new Action(Close), addRnToDocument));

                 task.ContinueWith((r) =>
                 {
                     Dispatcher.Invoke(() =>
                     {
                         DataContext = r.Result;
                     });
                 });
             };

        }


    }

    internal class ReferenceNumeralsVM : INotifyPropertyChanged
    {

        public static ReferenceNumeralsVM New(string rootFolder, string rnValue, Action closeWindow, Action<string> addRnToDocument)
        {
            var r = new ReferenceNumeralsVM();
            r.RnValue = rnValue;
            r.CloseWindow = closeWindow;
            r.AddRnToDocument = addRnToDocument;
            r.ReferenceNumeralsManager = new RnManager(new RnOptions { RootFolder = rootFolder });
            r.ReferenceNumerals = r.ReferenceNumeralsManager.GetReferenceNumerals();
            r.rnNumber = r.ReferenceNumerals.NewNumberByLast;
            r.HighestNumberMessage = $"Highest Reference Numeral: {r.ReferenceNumerals.HighestNumber}";
            r.SaveRnCommand = new RelayCommand(o => r.SaveRn());
            return r;
        }

        public RnManager ReferenceNumeralsManager { get; set; }
        public ReferenceNumerals ReferenceNumerals { get; set; }
        public ICommand SaveRnCommand { get; set; }

        public void SaveRn()
        {
            if (RnExists)
            {
                return;
            }
            Thread t = new Thread(() =>
            {
                ReferenceNumeralsManager.AddReferenceNumeral(RnNumber, RnValue);
            });

            t.Start();

            CloseWindow();
            AddRnToDocument(RnValue.Trim() + "\u00A0" + RnNumber);
        }

        public Action CloseWindow { get; set; }
        public Action<string> AddRnToDocument { get; set; }

        public Visibility LoadingOverlayVisibility { get; set; } = Visibility.Hidden;

        public bool RnExists => ReferenceNumerals.ReferenceNumeralExists(RnNumber);

        public Visibility RnErrorVisiblity
        {
            get => RnExists ? Visibility.Visible : Visibility.Hidden;
        }



        private int rnNumber;
        public int RnNumber
        {
            get => rnNumber;
            set
            {
                rnNumber = value;
                OnPropertyChanged(nameof(RnNumber));
                OnPropertyChanged(nameof(RnErrorVisiblity));
            }
        }

        private string rnValue;
        public string RnValue
        {
            get => rnValue;
            set
            {
                rnValue = value;
                OnPropertyChanged(nameof(rnValue));
            }
        }

        private string highestNumberMessage;
        public string HighestNumberMessage
        {
            get => highestNumberMessage;
            set
            {
                highestNumberMessage = value;
                OnPropertyChanged(nameof(HighestNumberMessage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
