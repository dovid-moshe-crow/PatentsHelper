using System;
/* Unmerged change from project 'ClosedXML (net40)'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq;
using System.Text;
*/

/* Unmerged change from project 'ClosedXML (net46)'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq;
using System.Text;
*/


namespace ClosedXML.Excel
{
    internal class XLRichText : XLFormattedText<IXLRichText>, IXLRichText
    {

        public XLRichText(IXLFontBase defaultFont)
            : base(defaultFont)
        {
            Container = this;
        }

        public XLRichText(XLFormattedText<IXLRichText> defaultRichText, IXLFontBase defaultFont)
            : base(defaultRichText, defaultFont)
        {
            Container = this;
        }

        public XLRichText(String text, IXLFontBase defaultFont)
            : base(text, defaultFont)
        {
            Container = this;
        }

    }
}
