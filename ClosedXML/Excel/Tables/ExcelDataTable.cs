using System.Collections.Generic;
using System.Data;

namespace ClosedXML.Excel
{
    public class ExcelDataTable : DataTable
    {
        public ExcelDataTable(string tableName) : base(tableName)
        {
        }


        public Dictionary<DataColumn, ColumnDataType> ColumnDataTypes { get; set; } = new Dictionary<DataColumn, ColumnDataType>();

    }
}
