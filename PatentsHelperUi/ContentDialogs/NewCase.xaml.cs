using ClosedXML.Excel;
using ModernWpf.Controls;
using PatentsHelperCases;
using PatentsHelperSettings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace PatentsHelperUi.ContentDialogs
{
    /// <summary>
    /// Interaction logic for NewCase.xaml
    /// </summary>
    public partial class NewCase : ContentDialog
    {
        public NewCase()
        {
            InitializeComponent();
            DataContext = NewCaseVM;
        }

        public NewCaseVM NewCaseVM { get; set; } = new NewCaseVM();

        public string CaseNumber => NewCaseVM.CaseNumber;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            NewCaseVM.SaveCase(args);
        }
    }

    public class NewCaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string CaseNumber => Fields?.ElementAt(0)?.Data?.ToString();

        public void SaveCase(ContentDialogButtonClickEventArgs args)
        {


            if (string.IsNullOrEmpty(CaseNumber))
            {
                MessageBox.Show("Case number is empty");
                args.Cancel = true;
                return;
            }

            if (ExcelCases.CaseExists(CaseNumber))
            {
                MessageBox.Show("Case number already exists");
                args.Cancel = true;
                return;
            }
            CasesManager.AddCaseToExcel(Fields.ConvertAll(f => f.Data).ToArray());
        }

        private List<Field> fields;

        public List<Field> Fields
        {
            get
            {
                if (fields == null)
                {
                    var emailFields = new UserSettings().CasesEmailsColumns?.Split(',');
                    fields = CasesDataTable.Columns.Cast<DataColumn>().ToList().ConvertAll(d => new Field
                    {
                        Name = d.ColumnName,
                        Type = TypesHelper.GetFieldType(d, CasesDataTable.ColumnDataTypes[d], emailFields)
                    });
                }

                return fields;
            }
        }



        public DataRow DataRow => CasesDataTable?.NewRow();

        public ExcelCases ExcelCases { get; set; } = CasesManager.GetCasesDataTable();

        public ExcelDataTable CasesDataTable => ExcelCases.CasesTable;
    }
}
