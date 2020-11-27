namespace PatentsHelperRn
{
    public class RnOptions
    {
        public string RootFolder { get; set; }
        public string FileName { get; set; } = "rn.xlsx";
        public string SheetName { get; set; } = "sheet1";
        public int RnIncrement { get; set; }

    }
}
