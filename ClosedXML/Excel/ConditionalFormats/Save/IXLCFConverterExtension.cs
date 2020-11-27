
/* Unmerged change from project 'ClosedXML (net40)'
Before:
using System;
After:
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
*/

/* Unmerged change from project 'ClosedXML (net46)'
Before:
using System;
After:
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
*/
using 
/* Unmerged change from project 'ClosedXML (net40)'
Before:
using System.Text;
using DocumentFormat.OpenXml.Office2010.Excel;
After:
using System.Text;
*/

/* Unmerged change from project 'ClosedXML (net46)'
Before:
using System.Text;
using DocumentFormat.OpenXml.Office2010.Excel;
After:
using System.Text;
*/
DocumentFormat.OpenXml.Office2010.Excel;

namespace ClosedXML.Excel
{
    internal interface IXLCFConverterExtension
    {
        ConditionalFormattingRule Convert(IXLConditionalFormat cf, XLWorkbook.SaveContext context);
    }
}
