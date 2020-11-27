using System.Windows;
using System.Windows.Controls;

namespace PatentsHelperUi.ContentDialogs
{
    public class TypesDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBoxTemplate { get; set; }
        public DataTemplate NumberBoxTemplate { get; set; }
        public DataTemplate ToggleButtonTemplate { get; set; }
        public DataTemplate DatePickerTemplate { get; set; }
        public DataTemplate TimeSpanTemplate { get; set; }
        public DataTemplate EmailInputTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Field f = (Field)item;

            switch (f.Type)
            {
                case Types.Bool:
                    return ToggleButtonTemplate;
                case Types.DateTime:
                    return DatePickerTemplate;
                case Types.Number:
                    return NumberBoxTemplate;
                case Types.Text:
                    return TextBoxTemplate;
                case Types.TimeSpan:
                    return TimeSpanTemplate;
                case Types.Email:
                    return EmailInputTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
