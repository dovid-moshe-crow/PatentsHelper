using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace PatentsHelperExcel
{

    public static class ExcelFileTypes
    {
        public static byte[] Blank { get; } = Properties.Resources.blank;
        public static byte[] Rn { get; } = Properties.Resources.rn;
        public static byte[] Cases { get; } = Properties.Resources.cases;
        public static byte[] Deadlines { get; } = Properties.Resources.deadlines;
    }

    public static class ExcelApp
    {

        public static void CreateXlsxFile(string excelFilePath, byte[] fileContent)
        {
            File.WriteAllBytes(excelFilePath, fileContent);
        }


        private static FileStream WriteFile(string excelFilePath)
        {
            return File.Open(excelFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        }

        private static FileStream ReadFile(string excelFilePath)
        {
            return File.Open(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public static FileStream ReadStream(string excelFilePath, byte[] fileContent)
        {
            try
            {
                return ReadFile(excelFilePath);
            }
            catch (FileNotFoundException)
            {
                CreateXlsxFile(excelFilePath, fileContent);
                return ReadFile(excelFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(excelFilePath));
                CreateXlsxFile(excelFilePath, fileContent);
                return ReadFile(excelFilePath);
            }
        }

     
        public static FileStream WriteStream(string excelFilePath, byte[] fileContent)
        {

           
            try
            {
                return WriteFile(excelFilePath);
            }
            catch (FileNotFoundException)
            {
                CreateXlsxFile(excelFilePath, fileContent);
                return WriteFile(excelFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(excelFilePath));
                CreateXlsxFile(excelFilePath, fileContent);
                return WriteFile(excelFilePath);
            }
        }
        /// <summary>
        /// make workbook open in excel read only
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static bool ReleaseWorkbookFromExcel(Workbook workbook)
        {
            try
            {
                workbook.Save();
                workbook.ChangeFileAccess(XlFileAccess.xlReadOnly, false, false);
                return true;
            }
            catch
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.FinalReleaseComObject(workbook);
                return false;
            }
        }

        /// <summary>
        /// return workbook access to excel
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static bool RestoreWorkbookAccess(Workbook workbook)
        {
            try
            {
                workbook.ChangeFileAccess(XlFileAccess.xlReadWrite, false, false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.FinalReleaseComObject(workbook);
                workbook = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Workbook GetOpenedWB_ByPath(string wbPath)
        {
            return (Workbook)GetRunningObjects().FirstOrDefault(x => (x.Path == wbPath) && (x.Obj is Workbook)).Obj;
        }

        private static List<RunningObject> GetRunningObjects()
        {
            // Get the table.
            List<RunningObject> roList = new List<RunningObject>();
            IBindCtx bc;
            CreateBindCtx(0, out bc);
            IRunningObjectTable runningObjectTable;
            bc.GetRunningObjectTable(out runningObjectTable);
            IEnumMoniker monikerEnumerator;
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            // Enumerate and fill list
            IMoniker[] monikers = new IMoniker[1];
            IntPtr numFetched = IntPtr.Zero;
            List<object> names = new List<object>();
            List<object> books = new List<object>();
            while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
            {
                RunningObject running;
                monikers[0].GetDisplayName(bc, null, out running.Path);
                runningObjectTable.GetObject(monikers[0], out running.Obj);
                roList.Add(running);
            }
            return roList;
        }

        private struct RunningObject
        {
            public string Path;
            public object Obj;
        }

        [DllImport("ole32.dll")]
        static extern void CreateBindCtx(int a, out IBindCtx b);
    }
}
