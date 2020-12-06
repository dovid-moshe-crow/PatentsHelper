using ExcelNumberFormat;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PatentsHelperExcel
{
    public class ExcelWorkbook
    {

        public ExcelWorkbook(FileStream fileStream)
        {
            ExcelFileStream = fileStream;
        }

        private FileStream ExcelFileStream { get; set; }

        private static Type GetDataTypeFromCell(int numberFormatId, string format)
        {

            if (numberFormatId == 46U)
                return typeof(TimeSpan);
            else if ((numberFormatId >= 14 && numberFormatId <= 22) ||
                     (numberFormatId >= 45 && numberFormatId <= 47))
                return typeof(DateTime);
            else if (numberFormatId == 49)
                return typeof(string);
            else
                return GetDataTypeFromFormat(format);

        }

        private static Type GetDataTypeFromFormat(string format)
        {

            string f = format.ToLower();
            if (f.Contains("mm") ||
                f.Contains("dd") ||
                f.Contains("yy"))
            {
                return typeof(DateTime);
            }
            else if (f.Contains("0"))
            {
                return typeof(double);
            }
            else
            {
                return typeof(string);
            }
        }


        private int GetLastUsedRow(ExcelWorksheet sheet)
        {
            return GetLastUsedRow(sheet, sheet.Dimension.End.Column);
        }

        private int GetLastUsedRow(ExcelWorksheet sheet, int lastColumn)
        {
            if (sheet.Dimension == null) { return 0; }
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, lastColumn];
                if (range.Any(c => !string.IsNullOrWhiteSpace(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        public List<List<object>> GetColumns(int firstColumn, int lastColumn)
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                List<List<object>> rows = new List<List<object>>();
                var ws = xl.Workbook.Worksheets[1];
                var lastUsedRow = GetLastUsedRow(ws, lastColumn);
                for (int i = firstColumn; i <= lastColumn; i++)
                {
                    var col = new List<object>();
                    for (int j = 1; j <= lastUsedRow; j++)
                    {
                        col.Add(ws.Cells[j, i]?.Value);
                    }
                    rows.Add(col);
                }
                return rows;
            }
        }

        public object GetCell(int row, int col)
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                var ws = xl.Workbook.Worksheets[1];
                return ws.Cells[row, col].Value;
            }
        }

        public void SetCell(object data, int row, int col)
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                var ws = xl.Workbook.Worksheets[1];

                ws.Cells[row, col].Value = data;

                ExcelFileStream.SetLength(0);
                var s = xl.GetAsByteArray();
                ExcelFileStream.Write(s, 0, s.Length);
            }
        }

        public void AddRow(object[] data)
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                var ws = xl.Workbook.Worksheets[1];

                var row = GetLastUsedRow(ws, data.Length) + 1;

                for (int i = 0; i < data.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(data[i]?.ToString()))
                    {
                        ws.Cells[row, i + 1].Value = data[i];
                    }
                }
                ExcelFileStream.SetLength(0);
                ExcelFileStream.Flush();
                var s = xl.GetAsByteArray();
                ExcelFileStream.Write(s, 0, s.Length);
            }
        }

        public List<Field> GetFields([Optional] string[] emailFields)
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                var ws = xl.Workbook.Worksheets[1];
                var endColumn = ws.Dimension.End.Column;
                var endRow = GetLastUsedRow(ws);
                var dt = new DataTable();

                if (endRow > 0)
                {
                    for (int i = 1; i <= endColumn; i++)
                    {
                        var numberFormat = ws.Column(i).Style.Numberformat;
                        var header = ws.Cells[1, i];
                        if (header?.Value == null)
                        {
                            var col = dt.Columns.Add();
                            col.DataType = GetDataTypeFromCell(numberFormat.NumFmtID, numberFormat.Format);
                        }
                        else
                        {

                            dt.Columns.Add(header.Value.ToString(), GetDataTypeFromCell(numberFormat.NumFmtID, numberFormat.Format));
                        }
                    }
                }

                return dt.Columns.Cast<DataColumn>().ToList().ConvertAll(d => new Field
                {
                    Name = d.ColumnName,
                    Type = TypesHelper.GetFieldType(d, emailFields)
                });
            }
        }

        public DataTable GetDataTable()
        {
            using (var xl = new ExcelPackage(ExcelFileStream))
            {
                var ws = xl.Workbook.Worksheets[1];
                var endColumn = ws.Dimension.End.Column;
                var endRow = GetLastUsedRow(ws);
                var dt = new DataTable();

                if (endRow > 0)
                {
                    for (int i = 1; i <= endColumn; i++)
                    {
                        var numberFormat = ws.Column(i).Style.Numberformat;
                        var header = ws.Cells[1, i];
                        if (header?.Value == null)
                        {
                            var col = dt.Columns.Add();
                            col.ExtendedProperties.Add("numberFormat", numberFormat.Format);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(header.Value.ToString()) || dt.Columns.Contains(header.Value.ToString().Trim()))
                            {
                                var dc = dt.Columns.Add();
                                dc.ExtendedProperties.Add("numberFormat", numberFormat.Format);
                            }
                            else
                            {
                                var dc = dt.Columns.Add(header.Value.ToString().Trim());
                                dc.ExtendedProperties.Add("numberFormat", numberFormat.Format);
                            }
                        }
                    }
                }

                if (endRow > 1)
                {
                    for (int i = 2; i <= endRow; i++)
                    {
                        var row = new List<object>();
                        for (int j = 1; j <= endColumn; j++)
                        {
                            var value = ws.Cells[i, j].Value;
                            if (value == null)
                            {
                                row.Add(null);
                            }
                            else
                            {
                                var numberformat = new NumberFormat(dt.Columns[j - 1].ExtendedProperties["numberFormat"]?.ToString());
                                row.Add(numberformat.Format(value, CultureInfo.CurrentCulture));
                            }
                        }
                        dt.Rows.Add(row.ToArray());
                    }
                }
                return dt;
            }
        }
    }
}
