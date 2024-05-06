using PatentsHelperExcel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PatentsHelperSettings;

namespace PatentsHelperDeadlines
{
    public static class DeadlinesManager
    {
        public static string FileName { get; set; } = "deadlines.xlsx";
        public static string FullPath => Path.Combine(new UserSettings().PatentsHelperRootFolder, FileName);
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
                var deadlines = wb.GetDataTable();
                return new ExcelDeadlines(deadlines);
            }
        }

        public static List<Field> GetFields()
        {
            using (var fs = ExcelApp.ReadStream(FullPath, ExcelFileTypes.Deadlines))
            {
                var wb = new ExcelWorkbook(fs);
                return wb.GetFields();
            }
        }
        public static void AddDeadlineToExcel(object[] row)
        {
            try
            {
                using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Deadlines))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(row);
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
                    wb.AddRow(row);
                }

                ExcelApp.RestoreWorkbookAccess(openWb);
            }
        }

    }
}
