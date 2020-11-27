using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PatentsHelperDeadlines
{
    public partial class ExcelDeadlines
    {
        public ExcelDataTable DeadlinesTable { get; set; }

        public ExcelDeadlines(ExcelDataTable excelDataTable)
        {
            DeadlinesTable = excelDataTable;
        }

        public List<Deadline> Deadlines(string caseId)
        {
            if (string.IsNullOrWhiteSpace(caseId))
            {
                return null;
            }

            DeadlinesTable.CaseSensitive = false;

            if (!DeadlinesTable.Columns.Contains("Deadline") || !DeadlinesTable.Columns.Contains("Title"))
            {
                return null;
            }


            return DeadlinesTable?.AsEnumerable()
                .DefaultIfEmpty()?.Where(d => d?[0]?.ToString()?.Trim()?.Equals(caseId, StringComparison.OrdinalIgnoreCase) == true)
                .DefaultIfEmpty()?.ToList().ConvertAll(d =>
                {
                    if (DateTime.TryParse(d?["Deadline"]?.ToString(), out DateTime deadlineDate))
                    {
                        return new Deadline { Title = d?["Title"]?.ToString(), DeadlineDate = deadlineDate };
                    }
                    else
                    {
                        return null;
                    }

                }).Where(d => d != null && !string.IsNullOrWhiteSpace(d?.Title)).ToList();




        }
    }
}
