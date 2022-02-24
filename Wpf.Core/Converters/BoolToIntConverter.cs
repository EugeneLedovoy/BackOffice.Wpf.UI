using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf.Core
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
