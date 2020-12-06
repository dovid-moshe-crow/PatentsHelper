using PatentsHelperExcel;
using PatentsHelperSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PatentsHelperRn
{
    public class RnManager
    {
        private readonly RnOptions rnOptions;

        private readonly string fullPath;
        public RnManager(RnOptions rnOptions)
        {
            this.rnOptions = rnOptions;
            fullPath = Path.Combine(rnOptions.RootFolder, rnOptions.FileName);
        }

        public ReferenceNumerals GetReferenceNumerals()
        {
            if (!FileExists())
            {
                CreateFile();
            }
            using (var fs = ExcelApp.ReadStream(fullPath, ExcelFileTypes.Rn))
            {
                var wb = new ExcelWorkbook(fs);

                var columns = wb.GetColumns(1, 2);
                var numbers = columns[0];
                var values = columns[1];

                var rns = new ReferenceNumerals();

                if (int.TryParse(wb.GetCell(2, 3)?.ToString(), out int increment))
                {
                    rns.Increment = increment;
                }
                else
                {
                    rns.Increment = new UserSettings().ReferenceNumeralIncrement;
                }

                for (int i = 1; i < numbers.Count && i < values.Count; i++)
                {
                    if (int.TryParse(numbers[i]?.ToString(), out int number))
                    {
                        if (!rns.KeyValuePairs.ContainsKey(number))
                        {
                            rns.KeyValuePairs.Add(number, values[i]?.ToString());
                        }
                    }
                }
                return rns;
            }
        }

        public void AddReferenceNumeral(int number, string value)
        {
            if (!FileExists())
            {
                CreateFile();
            }
            try
            {
                using (var fs = ExcelApp.WriteStream(fullPath, ExcelFileTypes.Rn))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(new object[]{ number, value });
                }
            }
            catch
            {
                var openWb = ExcelApp.GetOpenedWB_ByPath(fullPath);
                var isReleassed = ExcelApp.ReleaseWorkbookFromExcel(openWb);

                if (!isReleassed)
                {
                    throw new Exception("Cant write to the Excel file");
                }

                using (var fs = ExcelApp.WriteStream(fullPath, ExcelFileTypes.Rn))
                {
                    var wb = new ExcelWorkbook(fs);
                    wb.AddRow(new object[]{ number, value });
                }

                ExcelApp.RestoreWorkbookAccess(openWb);
            }
        }

        public bool FileExists()
        {
            return File.Exists(fullPath);
        }

        public void CreateFile()
        {
            using (var fs = ExcelApp.WriteStream(Path.Combine(rnOptions.RootFolder, rnOptions.FileName), ExcelFileTypes.Rn))
            {
                var wb = new ExcelWorkbook(fs);
                rnOptions.RnIncrement = new UserSettings().ReferenceNumeralIncrement;
                wb.SetCell(rnOptions.RnIncrement, 2, 3);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        public void OpenInExcel()
        {
            Process.Start(fullPath);
        }
    }
}
