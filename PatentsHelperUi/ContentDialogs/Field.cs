using System;
using System.ComponentModel;

namespace PatentsHelperUi.ContentDialogs
{
    public class Field : INotifyPropertyChanged
    {


        public string Name { get; set; }

        public Types Type { get; set; }


        public string Text { get; set; }

        public double? Number { get; set; }

        public bool? Bool { get; set; }

        public DateTime? DateTime { get; set; }

        public TimeSpan? TimeSpan { get; set; }

        public object Data
        {
            get
            {
                if (Type == Types.Text || Type == Types.Email)
                {
                    return Text;
                }

                if (Type == Types.Bool)
                {
                    return Bool;
                }

                if (Type == Types.DateTime)
                {
                    return DateTime;
                }

                if (Type == Types.TimeSpan)
                {
                    return TimeSpan;
                }

                if (Type == Types.Number)
                {
                    return Number;
                }

                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
