using PatentsHelperExcel;
using System;
using System.Diagnostics;
using System.IO;

namespace PatentsHelperDeadlines
{
    public static class DeadlinesManager
    {
        public static string RootFolder { get; set; } = Path.Combine(Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory), @"patents-helper-files");
        public static string FileName { get; set; } = "deadlines.xlsx";
        public static string FullPath { get; set; } = Path.Combine(RootFolder, FileName);
        public static string TableName { get; set; } = "deadlines";

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        public static void OpenInExcel()
        {
            Process.Start(FullPath);
        }

        public static bool FileExists()
        {
            return File.Exists(FullPath);
        }

        public static void CreateFile()
        {
            using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Deadlines)) { };
        }

        public static ExcelDeadlines GetDeadlinesDataTable()
        {
            using (var fs = ExcelApp.ReadStream(FullPath, ExcelFileTypes.Deadlines))
            {
                var wb = new ExcelWorkbook(fs);
                var deadlines = wb.GetDataTable(TableName);
                return new ExcelDeadlines(deadlines);
            }
        }

        public class Deadline
        {
            public object CaseId { get; set; }
            public DateTime DeadlineDate { get; set; }
            public string Title { get; set; }
        }

        public static void AddDeadlineToExcel(object[] row)
        {
            try
            {
                using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Deadlines))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(TableName, row);
                }
            }
            catch
            {
                var openWb = ExcelApp.GetOpenedWB_ByPath(FullPath);
                var isReleassed = ExcelApp.ReleaseWorkbookFromExcel(openWb);

                if (!isReleassed)
                {
                    throw new Exception("Cant write to the Excel file");
                }

                using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Deadlines))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(TableName, row);
                }

                ExcelApp.RestoreWorkbookAccess(openWb);
            }
        }

    }
}
