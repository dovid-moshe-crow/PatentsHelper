using Microsoft.Office.Interop.Outlook;
using PatentsHelperSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace PatentsHelperOutlook
{
    public static class OutlookManager
    {
        public static string EMAIL_TEMPLATES_PATH = Path.Combine(Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory), @"patents-helper-files\email-templates");


        public static List<EmailTemplate> EmailTemplates
        {
            get
            {
                try
                {
                    Directory.CreateDirectory(EMAIL_TEMPLATES_PATH);
                    if (!File.Exists(Path.Combine(EMAIL_TEMPLATES_PATH, "default.oft")))
                    {
                        File.WriteAllBytes(Path.Combine(EMAIL_TEMPLATES_PATH, "default.oft"), Properties.Resources._default);
                    }
                }
                catch { }

                try
                { 
                    return Directory.GetFiles(EMAIL_TEMPLATES_PATH, "*.oft").ToList().ConvertAll(p => new EmailTemplate { Path = p, Name = Path.GetFileNameWithoutExtension(p) });
                }
                catch
                {
                    return null;
                }
            }
        }

        public static void EmailFromTemplate(DataRow dataRow, string path)
        {
            var app = OutlookApp;

            var data = dataRow.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => dataRow[c]);

            MailItem mail = app.CreateItemFromTemplate(path);
            mail.To = ParseTemplateString(mail.To, data);
            mail.CC = ParseTemplateString(mail.CC, data);
            mail.Body = ParseTemplateString(mail.Body, data);
            mail.Subject = ParseTemplateString(mail.Subject, data);
            mail.BCC = ParseTemplateString(mail.BCC, data);
            mail.Display(mail);

            ReleaseObj(app);
        }

        public static void AddSearchFolder(DataRow dataRow)
        {
            Thread t = new Thread(() =>
            {
                if (Type.GetTypeFromProgID("Outlook.Application") == null)
                {
                    MessageBox.Show("Outlook is not installed");
                    return;
                }

                var caseId = dataRow.ItemArray?[0]?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(caseId))
                {
                    return;
                }
                var outlookApp = OutlookApp;
                if (outlookApp == null)
                {
                    MessageBox.Show("Failed to open Outlook");
                    return;
                }

                MAPIFolder fldContacts = (Folder)outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
                string filter = "urn:schemas:mailheader:subject LIKE \'%" + caseId + "%\'";

                string scope = new UserSettings().SearchFolderScope;

                var sfolder = outlookApp.AdvancedSearch(scope, filter, true, caseId);
                var a = sfolder.Save(caseId);
               
                ReleaseObj(outlookApp);
            });
            t.Start();
        }

        private static string ParseTemplateString(string template, Dictionary<string, object> data)
        {
            if (data == null || template == null) return template;
            foreach (var keyValuePair in data)
            {
                template = Regex.Replace(template, $"{{{{{keyValuePair.Key?.Trim()}}}}}", keyValuePair.Value?.ToString() ?? string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return template;
        }


        public static Application OutlookApp
        {
            get
            {
                if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
                {
                    return Marshal.GetActiveObject("Outlook.Application") as Application;
                }
                else
                {
                    return new Application();
                } 
            }
        }

        public static void ReleaseObj(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                Marshal.ReleaseComObject(obj);
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
            }
            catch { }


        }
    }
}
