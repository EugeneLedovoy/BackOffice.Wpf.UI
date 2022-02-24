using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyWpfApp
{
    public class BoolToIntConverter : ValueConverterBase<BoolToIntConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                _ = int.TryParse(parameter?.ToString(), out int parsed);
                return b ? parsed : 0;
            }

            return Binding.DoNothing;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
