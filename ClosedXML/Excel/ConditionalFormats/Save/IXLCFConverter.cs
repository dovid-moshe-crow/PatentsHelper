
/* Unmerged change from project 'ClosedXML (net40)'
Before:
using System;
After:
using DocumentFormat.OpenXml.Spreadsheet;
using System;
*/

/* Unmerged change from project 'ClosedXML (net46)'
Before:
using System;
After:
using DocumentFormat.OpenXml.Spreadsheet;
using System;
*/
using DocumentFormat.OpenXml.Spreadsheet;
using System
/* Unmerged change from project 'ClosedXML (net40)'
Before:
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
After:
using System.Text;
*/

/* Unmerged change from project 'ClosedXML (net46)'
Before:
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
After:
using System.Text;
*/
;

namespace ClosedXML.Excel
{
    internal interface IXLCFConverter
    {
        ConditionalFormattingRule Convert(IXLConditionalFormat cf, Int32 priority, XLWorkbook.SaveContext context);
    }
}
