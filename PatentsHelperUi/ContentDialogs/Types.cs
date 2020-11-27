using ClosedXML.Excel;
using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace PatentsHelperUi.ContentDialogs
{
    public enum Types
    {
        Text,
        Number,
        Bool,
        DateTime,
        TimeSpan,
        Email
    }

    public static class TypesHelper
    {
        public static Types GetFieldType(DataColumn d, ColumnDataType columnDataType, [Optional] string[] emailFields)
        {
            var type = (d.DataType == typeof(string) || d.DataType == typeof(object)) ? columnDataType.Type : d.DataType;

            if (type == typeof(bool))
            {
                return Types.Bool;
            }
            else if (type == typeof(double))
            {
                return Types.Number;
            }
            else if (type == typeof(DateTime))
            {
                return Types.DateTime;
            }
            else if (type == typeof(TimeSpan))
            {
                return Types.TimeSpan;
            }
            else
            {
                if (emailFields?.Any() == true)
                {
                    if (emailFields.Contains(d.ColumnName.Trim().ToLower()))
                    {
                        return Types.Email;
                    }
                }
                return Types.Text;
            }
        }
    }

}
