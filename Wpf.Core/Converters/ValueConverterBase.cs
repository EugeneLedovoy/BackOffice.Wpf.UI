using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.Core
{
    public abstract class ValueConverterBase<TConverter> : MarkupExtension, IValueConverter where TConverter : class, new()
    {
        private static TConverter Instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance ??= (TConverter)new WeakReference(new TConverter()).Target;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
