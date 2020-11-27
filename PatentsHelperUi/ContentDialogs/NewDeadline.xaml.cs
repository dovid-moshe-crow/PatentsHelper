using ClosedXML.Excel;
using ModernWpf.Controls;
using PatentsHelperDeadlines;
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
        public NewDeadline([Optional] object caseId)
        {
            InitializeComponent();
            NewDeadlineVM = new NewDeadlineVM { CaseId = caseId };
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

        public object CaseId { get; set; }

        private List<Field> fields;

        public List<Field> Fields
        {
            get
            {
                if (fields == null)
                {
                    fields = DeadlinesDataTable.Columns.Cast<DataColumn>().ToList().ConvertAll(x => new Field { Name = x.ColumnName, Type = TypesHelper.GetFieldType(x, DeadlinesDataTable.ColumnDataTypes[x]) });
                    var firstField = fields.ElementAtOrDefault(0);
                    if (firstField.Type == Types.Text)
                    {
                        firstField.Text = CaseId?.ToString();
                    }
                    else if (firstField.Type == Types.Number)
                    {
                        if (double.TryParse(CaseId?.ToString(), out double caseId))
                        {
                            firstField.Number = caseId;
                        }
                    }
                }

                return fields;
            }
        }

        public DataRow DataRow => DeadlinesDataTable?.NewRow();

        public ExcelDeadlines ExcelDeadlines { get; set; } = DeadlinesManager.GetDeadlinesDataTable();

        public ExcelDataTable DeadlinesDataTable => ExcelDeadlines.DeadlinesTable;

    }
}
