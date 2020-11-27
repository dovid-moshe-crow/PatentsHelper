using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConsoleTestApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            Console.WriteLine(typeof(DateTime) == typeof(object));
            Console.ReadLine();
            /*    SettingsStorage.Start();

            

                SettingsStorage.Shutdown();
    */
            // Console.ReadLine();

            // ExcelApp.CreateXlsxFile(@"C:\Users\dovid\Desktop\cases\cases.xlsx", ExcelFileTypes.Blank);
            //RnManager rnManager = new RnManager(new RnOptions { RootFolder = @"C:\Users\dovid\Desktop\cases"});
            //rnManager.AddRn(4,"dovid moshe crow");
            //var b = rnManager.GetReferenceNumerals();

            /* Console.WriteLine("increment: " + b.Increment);

             foreach (var a in b.KeyValuePairs)
             {
                 Console.WriteLine(a.Key + " ::: " + a.Value);
             }
             Console.ReadLine();*/
            /* var app = new App();
             System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
             new RefrenceNumeralsInput(@"C:\Users\dovid\Desktop\cases\", "lololo").ShowDialog();
             //new MainWindow().ShowDialog();
             if (System.Windows.Application.Current != null)
             { 
                 System.Windows.Application.Current.Shutdown();
             }*/
            //app.Run(new MainWindow());

            /* using (FileStream fs = File.Open(@"C:\Users\dovid\Documents\patents-helper-files\cases.xlsx", FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
             {
                 var wb = new ExcelWorkbook(fs);
                 var t = wb.GetDataTable("Sheet1", "cases");

                 var r = t.NewRow();
                 r["docketNumber"] = "678";
                 r["email"] = "dovidmoshcrow@gmail.com";
                 r["password"] = "12345678";

                 t.ColumnDataTypes.ForEach((e) =>
                 {
                     Console.WriteLine(t.ColumnDataTypes.Where(h => h.Name == e.Name).FirstOrDefault().FormatString);
                     Console.WriteLine(t.ColumnDataTypes.Where(h => h.Name == e.Name).FirstOrDefault().Type);
                 });

                 wb.AddRow("Sheet1", "cases", r);
                 PrintDataTable(t);
                 Console.ReadLine();
             }*/
        }

        static void PrintDataTable(DataTable data)
        {
            Console.WriteLine();
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            foreach (DataColumn col in data.Columns)
            {
                Console.Write(col.ColumnName);
                var maxLabelSize = data.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 10; i++) Console.Write(" ");
            }

            Console.WriteLine();

            foreach (DataRow dataRow in data.Rows)
            {
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    Console.Write(dataRow.ItemArray[j]);
                    for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 10; i++) Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
