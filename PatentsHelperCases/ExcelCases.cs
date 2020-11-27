using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PatentsHelperCases
{
    public class ExcelCases
    {

        public ExcelCases(ExcelDataTable excelDataTable)
        {
            CasesTable = excelDataTable;
        }

        public ExcelDataTable CasesTable { get; set; }

        public bool CaseExists(string caseNumber)
        {
            return CasesTable.AsEnumerable().DefaultIfEmpty().Where(dr => dr.Field<object>(0).ToString().Trim().Equals(caseNumber.Trim(), System.StringComparison.OrdinalIgnoreCase)).Count() > 0;
        }



        public ColumnDataType GetColumnDataType(DataColumn dataColumn)
        {
            if (CasesTable.ColumnDataTypes.TryGetValue(dataColumn, out ColumnDataType columnType))
            {
                if (columnType.Type != typeof(string) || !string.IsNullOrEmpty(columnType.FormatString))
                {
                    return columnType;
                }
            }

            return new ColumnDataType
            {
                Type = CasesTable.Columns[dataColumn.ColumnName].DataType,
                FormatString = null,
                Name = dataColumn.ColumnName
            };
        }

        public Dictionary<string, object> GetRowAsDict(DataRow dataRow)
        {
            return dataRow.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dataRow[c]);
        }
    }
}
