using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace PatentsHelperDeadlines
{
    public partial class ExcelDeadlines
    {
        public DataTable DeadlinesTable { get; set; }

        public ExcelDeadlines(DataTable excelDataTable)
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
                    var deadline = d?["Deadline"];
                    var title = d?["Title"];
                    if (deadline != null && title != null)
                    {
                        return new Deadline
                        {
                            Title = title.ToString(),
                            DeadlineDate = deadline.ToString()
                        };
                    }
                    else
                    {
                        return null;
                    }

                }).Where(d => d != null && !string.IsNullOrWhiteSpace(d?.Title)).ToList();




        }
    }
}
