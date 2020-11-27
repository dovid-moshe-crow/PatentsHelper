using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;

namespace PatentsHelperExcel
{
    public interface IExcelWorkbook
    {
        void AddRow(string sheetName, List<object> rowContent);
        void AddRow(string tableName, DataRow dataRow);
        object GetCell(string sheetName, int row, int col);
        List<object> GetColumn(string sheetName, int index);
        ExcelDataTable GetDataTable(string tableName);
        object GetRange(string sheetName, IXLRangeAddress excelRange);
        List<object> GetRow(string sheetName, int index);
    }
}