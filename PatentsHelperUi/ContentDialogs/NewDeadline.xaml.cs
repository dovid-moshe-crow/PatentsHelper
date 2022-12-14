using ModernWpf.Controls;
using PatentsHelperDeadlines;
using PatentsHelperExcel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace PatentsHelperUi.ContentDialogs
{
    /// <summary>
    /// Interaction logic for NewDeadline.xaml
    /// </summary>
    public partial class NewDeadline : ContentDialog
    {
        public NewDeadline([Optional] object caseId, [Optional] object title)
        {
            InitializeComponent();
            NewDeadlineVM = new NewDeadlineVM(caseId, title);
            DataContext = NewDeadlineVM;
        }

        public NewDeadlineVM NewDeadlineVM { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            NewDeadlineVM.SaveDeadline(args);
        }
    }

    public class NewDeadlineVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void SaveDeadline(ContentDialogButtonClickEventArgs args)
        {
            DeadlinesManager.AddDeadlineToExcel(Fields.ConvertAll(f => f.Data).ToArray());
        }

        public NewDeadlineVM(object caseId, object title)
        {
            var fields = DeadlinesManager.GetFields();
            var firstField = fields.FirstOrDefault();
            firstField.Data = caseId;

            var titleField = fields.Where(f => f.Name.ToLower().Trim().Equals("title")).FirstOrDefault();
            if (titleField != null)
            {
                titleField.Data = title;
            }
            Fields = fields;
        }


        public List<Field> Fields { get; }

    }
}
