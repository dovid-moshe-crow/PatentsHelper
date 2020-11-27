using Microsoft.Office.Interop.Word;

namespace PatentsHelperWord
{
    public class RangeManager
    {
        public Range SelectedRange { get; set; }

        public void SetTextInRange(string text)
        {
            SelectedRange.Text = text;
        }


        public void CollapseCursorToEnd()
        {
            SelectedRange.Collapse(WdCollapseDirection.wdCollapseEnd);
            SelectedRange.Select();
        }

        public void CollapseCursorToStart()
        {
            SelectedRange.Collapse(WdCollapseDirection.wdCollapseStart);
            SelectedRange.Select();
        }

        public bool IsTextSelected()
        {
            return !string.IsNullOrEmpty(SelectedRange.Text);
        }

        public string SelectAndGetLastUnselectedWord()
        {
            SelectedRange.StartOf(WdUnits.wdWord, WdMovementType.wdExtend);
            return SelectedRange.Text;
        }
    }
}
