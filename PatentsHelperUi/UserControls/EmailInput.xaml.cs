using FuzzySharp;
using PatentsHelperFileSystem;
using PatentsHelperOutlook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PatentsHelperUi.UserControls
{
    /// <summary>
    /// Interaction logic for EmailInput.xaml
    /// </summary>
    public partial class EmailInput : UserControl
    {
        public EmailInput()
        {
            InitializeComponent();
            EmailInputVM = new EmailInputVM();
            EmailInputVM.EmailsChanged = () =>
            {
                Text = EmailInputVM.EmailsString;
                emailsItemRepeater.Measure(Size.Empty);
            };
            DataContext = EmailInputVM;

        }

        public EmailInputVM EmailInputVM { get; set; }

        private void AutoSuggestBox_QuerySubmitted(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            EmailInputVM.AddEmail(args.QueryText);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(EmailInput), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(HeaderChangedCallback)) { Inherits = true, BindsTwoWayByDefault = true, });

        private static void HeaderChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EmailInput)d).EmailInputVM.Header = e.NewValue.ToString();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EmailInput), new FrameworkPropertyMetadata(string.Empty) { DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, BindsTwoWayByDefault = true, Inherits = true });




    }

    public class EmailInputVM : INotifyPropertyChanged
    {
        public EmailInputVM()
        {
            AllEmails = AutoCompleteEmails.GetEmails();
        }

        public ICommand DeleteEmailCommand => new RelayCommand<string>(email =>
        {
            Emails.Remove(email.Trim());
            EmailsChanged();
        });

        public string Header { get; set; }

        public Action EmailsChanged { get; set; }

        public string EmailsString => string.Join(";", Emails);

        public void AddEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && !Emails.ToList().ConvertAll(s => s.ToLower()).Contains(email?.ToLower()))
            {
                Emails.Add(email.Trim());
                FilterEmails = "";
            }
            EmailsChanged();
        }
        public string FilterEmails { get; set; }

        private List<string> allEmails;

        public List<string> AllEmails
        {
            get => allEmails?.Where(s => !Emails.Contains(s) && Fuzz.PartialRatio(s?.ToLower() ?? string.Empty, FilterEmails?.ToLower() ?? string.Empty) > 90)?.ToList();
            set => allEmails = value;
        }

        public ObservableCollection<string> Emails { get; set; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

