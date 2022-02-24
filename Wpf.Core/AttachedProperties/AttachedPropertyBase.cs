using System;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Core
{
    public abstract class AttachedPropertyBase<TExtendingClass, T> where TExtendingClass : class, new()
    {
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged;

        public event Action<DependencyObject, object> ValueChanging;

        public static TExtendingClass Instance { get; private set; } = (TExtendingClass)new WeakReference(new TExtendingClass()).Target;

        public static T GetValue(DependencyObject obj) => (T)obj.GetValue(ValueProperty);

        public static void SetValue(DependencyObject obj, T value) => obj.SetValue(ValueProperty, value);

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(T), typeof(AttachedPropertyBase<TExtendingClass, T>),
                new FrameworkPropertyMetadata(default(T), OnValuePropertyChanged, OnValuePropertyChanging)
                {
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Instance as AttachedPropertyBase<TExtendingClass, T>)?.ValueChanged?.Invoke(d, e);
            (Instance as AttachedPropertyBase<TExtendingClass, T>)?.OnValueChanged(d, e);
        }

        private static object OnValuePropertyChanging(DependencyObject d, object baseValue)
        {
            (Instance as AttachedPropertyBase<TExtendingClass, T>)?.ValueChanging?.Invoke(d, baseValue);
            (Instance as AttachedPropertyBase<TExtendingClass, T>)?.OnValueChanging(d, baseValue);
            return baseValue;
        }

        protected virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }

        protected virtual void OnValueChanging(DependencyObject d, object value) { }
    }
}
