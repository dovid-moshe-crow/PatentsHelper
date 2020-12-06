using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatentsHelperExcel
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
            set
            {
                if (Type == Types.Text || Type == Types.Email)
                {
                    Text = value?.ToString();
                }

                if (Type == Types.Bool)
                {
                    Bool = (bool?)value;
                }

                if (Type == Types.DateTime)
                {
                   DateTime = (DateTime?)value;
                }

                if (Type == Types.TimeSpan)
                {
                    TimeSpan = (TimeSpan?)value;
                }

                if (Type == Types.Number)
                {
                    Number = (double?)value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
