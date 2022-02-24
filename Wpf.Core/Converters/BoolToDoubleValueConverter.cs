using System;
using System.Globalization;
using System.Windows.Data;

namespace Wpf.Core
{
    public class BoolToDoubleValueConverter : ValueConverterBase<BoolToDoubleValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag && parameter != null && double.TryParse(parameter.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                return flag ? val : 0;
            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
