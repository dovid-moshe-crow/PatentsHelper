﻿using PatentsHelperExcel;
using PatentsHelperSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace PatentsHelperCases
{
    public static class CasesManager
    {
        public static string FileName { get; set; } = "cases.xlsx";
        public static string FullPath => Path.Combine(new UserSettings().PatentsHelperRootFolder, FileName);
        public static string TableName { get; set; } = "cases";

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
            using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Cases)) { };
        }

        public static ExcelCases GetCasesDataTable()
        {
            using (var fs = ExcelApp.ReadStream(FullPath, ExcelFileTypes.Cases))
            {
                var wb = new ExcelWorkbook(fs);
                var cases = wb.GetDataTable();
                return new ExcelCases(cases);
            }
        }

        public static List<Field> GetFields()
        {
            using (var fs = ExcelApp.ReadStream(FullPath, ExcelFileTypes.Cases))
            {
                var wb = new ExcelWorkbook(fs);
                var emailFields = new UserSettings().CasesEmailsColumns?.Split(',');
                return wb.GetFields(emailFields);
            }
        }

        public static void AddCaseToExcel(object[] row)
        {
            try
            {
                using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Cases))
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

                using (var fs = ExcelApp.WriteStream(FullPath, ExcelFileTypes.Cases))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(row);
                }

                ExcelApp.RestoreWorkbookAccess(openWb);
            }
        }
    }
}
