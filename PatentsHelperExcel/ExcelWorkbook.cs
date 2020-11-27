using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace PatentsHelperExcel
{

    public class ExcelWorkbook : IExcelWorkbook
    {

        public ExcelWorkbook(FileStream fileStream)
        {
            ExcelFileStream = fileStream;
        }

        private FileStream ExcelFileStream { get; set; }

        public ExcelDataTable GetDataTable(string tableName)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var table = wb.Table(tableName);
                return table.AsExcelDataTable();
            }
        }

        public void AddRow(string tableName, DataRow dataRow)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var table = wb.Table(tableName);

                if (!table.LastRow().IsEmpty(XLCellsUsedOptions.AllContents))
                {
                    table.InsertRowsBelow(1);
                }
                var row = table.LastRow().AsRange();
                row.FirstCell().InsertData(dataRow.ItemArray, true);
                wb.Save();
            }
        }

        public void AddRow(string tableName, object[] items)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var table = wb.Table(tableName);

                if (!table.LastRow().IsEmpty(XLCellsUsedOptions.AllContents))
                {
                    table.InsertRowsBelow(1);
                }
                var row = table.LastRow().AsRange();
                row.FirstCell().InsertData(items, true);
                wb.Save();
            }
        }

        public void UpdateRow(string tableName, DataRow dataRow, int rowIndex)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var table = wb.Table(tableName);
                table.Row(rowIndex).FirstCell().InsertData(dataRow.ItemArray, true);
                wb.Save();
            }
        }

        public List<object> GetRow(string sheetName, int index)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var ws = wb.Worksheet(sheetName);
                var lastCellUsed = ws.Row(index)?.LastCellUsed()?.Address?.ColumnNumber;

                if (lastCellUsed == null)
                {
                    throw new Exception("Row doesn't exist");
                }

                return ws.Row(index).Cells(1, (int)lastCellUsed).ToList().ConvertAll(cell => cell?.Value);
            }
        }

        public List<object> GetColumn(string sheetName, int index)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                var ws = wb.Worksheet(sheetName);
                var lastCellUsed = ws.Column(index)?.LastCellUsed()?.Address?.RowNumber;
                if (lastCellUsed == null)
                {
                    throw new Exception("Column doesn't exist");
                }

                return ws.Column(index).Cells(1, (int)lastCellUsed).ToList().ConvertAll(cell => cell?.Value);
            }
        }

        public List<List<object>> GetRows(string sheetName, int firstRow, int lastRow)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {

                List<List<object>> rows = new List<List<object>>();
                var ws = wb.Worksheet(sheetName);
                for (int i = firstRow; i <= lastRow; i++)
                {
                    rows.Add(ws.Row(i).Cells(1, ws.Row(i).LastCellUsed().Address.ColumnNumber).ToList().ConvertAll(cell => cell?.Value));
                }
                return rows;
            }
        }

        public List<List<object>> GetColumns(string sheetName, int firstColumn, int lastColumn)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {

                List<List<object>> rows = new List<List<object>>();
                var ws = wb.Worksheet(sheetName);
                for (int i = firstColumn; i <= lastColumn; i++)
                {
                    rows.Add(ws.Column(i).Cells(1, ws.Column(i).LastCellUsed().Address.RowNumber).ToList().ConvertAll(cell => cell?.Value));
                }
                return rows;
            }
        }

        public void SetCell(string sheetName, object data, int row, int column)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                wb.Worksheet(sheetName).Cell(row, column).Value = data;
                wb.Save();
            }
        }



        public object GetCell(string sheetName, int row, int col)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                return wb.Worksheet(sheetName).Cell(row, col)?.Value;
            }
        }

        public object GetRange(string sheetName, IXLRangeAddress excelRange)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                return wb.Worksheet(sheetName).Range(excelRange);
            }
        }

        public void AddRow(string sheetName, List<object> rowContent)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                int lastRowUsed = 1;

                var ws = wb.Worksheet(sheetName);
                for (int i = 1; i <= rowContent.Count; i++)
                {
                    lastRowUsed = Math.Max(lastRowUsed, ws.Column(i).LastCellUsed().Address.RowNumber);
                }

                var row = ws.Row(lastRowUsed).RowBelow();

                var cells = row.Cells(1, rowContent.Count);
                foreach (var (cell, i) in cells.WithIndex())
                {
                    cell.Value = rowContent[i];
                }
                wb.Save();
            }
        }

        public void AdjustColumns(string sheetName)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                wb.Worksheet(sheetName).Columns().AdjustToContents();
                wb.Save();
            }
        }

        public void AdjustRows(string sheetName)
        {
            using (var wb = new XLWorkbook(ExcelFileStream))
            {
                wb.Worksheet(sheetName).Rows().AdjustToContents();
                wb.Save();
            }
        }

    }
}
