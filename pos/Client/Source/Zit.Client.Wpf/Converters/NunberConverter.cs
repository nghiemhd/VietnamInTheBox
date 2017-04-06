using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Zit.Client.Wpf.Converters
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            if (value is System.Decimal)
            {
                decimal val = (decimal)value;
                if (val == 0) return "";
                return val.ToString("N0");
            }
            else if (value is System.Int32)
            {
                int val = (int)value;
                if (val == 0) return "";
                return val.ToString("N0");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null) value = "0";

            if (targetType == typeof(System.Decimal))
            {
                decimal val = 0;
                if (decimal.TryParse((string)value, out val))
                {
                    return val;
                }
                return 0;
            }
            else if (targetType == typeof(System.Int32))
            {
                int val = 0;
                if (int.TryParse((string)value, out val))
                {
                    return val;
                }
                return 0;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
