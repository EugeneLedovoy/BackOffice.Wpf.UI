using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Core
{
    public class StringToVisibilityConverter : ValueConverterBase<StringToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                var result = parameter is bool isInverted && isInverted ? string.IsNullOrEmpty(s) : !string.IsNullOrEmpty(s);
                return result ? Visibility.Visible : Visibility.Collapsed;
            }

            return Binding.DoNothing;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
