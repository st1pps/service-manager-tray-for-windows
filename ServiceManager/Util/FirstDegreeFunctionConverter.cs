using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServiceManager.Util
{
    public class FirstDegreeFunctionConverter : IValueConverter
    {
        public double A { get; set; }
        public double B { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var a = GetDoubleValue(parameter, A);
            var x = GetDoubleValue(value, 0.0);

            return (a * x) + B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            var a = GetDoubleValue(parameter, A);
            var y = GetDoubleValue(value, 0.0);
            return (y - B) /a;
        }

        private static double GetDoubleValue(object parameter, double defaultValue)
        {
            double a;
            if (parameter != null)
            {
                try
                {
                    a = System.Convert.ToDouble(parameter);
                }
                catch
                {
                    a = defaultValue;
                }
            }
            else
            {
                a = defaultValue;
            }

            return a;
        }
    }
}
