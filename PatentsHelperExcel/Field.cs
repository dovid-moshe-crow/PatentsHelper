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
                switch (Type)
                {
                    case Types.Text:
                    case Types.Email:
                        return Text;
                    case Types.Bool:
                        return Bool;
                    case Types.DateTime:
                        return DateTime;
                    case Types.TimeSpan:
                        return TimeSpan;
                    case Types.Number:
                        return Number;
                    default:
                        return null;
                }
            }
            set
            {
                switch (Type)
                {
                    case Types.Text:
                    case Types.Email:
                        Text = value?.ToString();
                        break;
                    case Types.Bool:
                        Bool = (bool?)value;
                        break;
                    case Types.DateTime:
                        DateTime = (DateTime?)value;
                        break;
                    case Types.TimeSpan:
                        TimeSpan = (TimeSpan?)value;
                        break;
                    case Types.Number:
                        Number = (double?)value;
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
