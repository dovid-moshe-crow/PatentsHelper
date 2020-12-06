using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PatentsHelperExcel
{
   public static class TypesHelper
    {
       
            public static Types GetFieldType(DataColumn d, [Optional] string[] emailFields)
            {
                var type = d.DataType;

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
                        if (emailFields.ToList().ConvertAll(s => s?.ToLower()).Contains(d.ColumnName?.Trim()?.ToLower()))
                        {
                            return Types.Email;
                        }
                    }
                    return Types.Text;
                }
            }
        }
    }

