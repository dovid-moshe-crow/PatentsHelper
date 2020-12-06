using System.Data;
using System.Linq;

namespace PatentsHelperCases
{
    public class ExcelCases
    {

        public ExcelCases(DataTable excelDataTable)
        {
            CasesTable = excelDataTable;
        }

        public DataTable CasesTable { get; set; }

        public bool CaseExists(string caseNumber)
        {
            return CasesTable.AsEnumerable().DefaultIfEmpty().Where(dr => dr[0]?.ToString()?.Trim()?.Equals(caseNumber.Trim(), System.StringComparison.OrdinalIgnoreCase) == true).Count() > 0;
        }
    }
}
