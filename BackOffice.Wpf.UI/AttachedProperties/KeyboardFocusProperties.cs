using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyWpfApp
{
    public class IsFocusedProperty : AttachedPropertyBase<IsFocusedProperty, bool?>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus += FrameworkElement_GotFocus;
            }

            if (!fe.IsVisible)
            {
                fe.IsVisibleChanged += FrameworkElement_IsVisibleChanged;
            }

            if (e.NewValue != null && (bool)e.NewValue)
            {
                SetFocus(fe);
            }
        }

        private static void SetFocus(FrameworkElement fe)
        {
            if (!fe.Focus())
                Keyboard.Focus(fe);
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            SetValue(fe, true);
            fe.LostFocus += FrameworkElement_LostFocus;

        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            SetValue(fe, false);
            fe.LostFocus -= FrameworkElement_LostFocus;
        }

        private static void FrameworkElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            var isFocused = (bool?)GetValue(fe);
            if (fe.IsVisible && isFocused.HasValue && isFocused.Value)
            {
                fe.IsVisibleChanged -= FrameworkElement_IsVisibleChanged;
                SetFocus(fe);
            }
        }
    }

    public class KeyboardFocusOnProperty : AttachedPropertyBase<KeyboardFocusOnProperty, FrameworkElement>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)d;
            frameworkElement.Loaded += new RoutedEventHandler(OnLoaded);
            frameworkElement.Unloaded += new RoutedEventHandler(OnUnloaded);
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            var target = GetValue(fe);
            if (target == null)
                return;
            Keyboard.Focus(target);
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            fe.Loaded -= OnLoaded;
            fe.Unloaded -= OnUnloaded;
        }
    }

    #region KeyboardFocusHelper.IsFocused oldschool declaration

    //public static class KeyboardFocusHelper
    //{
    //    public static bool? GetIsFocused(DependencyObject obj)
    //    {
    //        return (bool?)obj.GetValue(IsFocusedProperty);
    //    }

    //    public static void SetIsFocused(DependencyObject obj, bool? value)
    //    {
    //        obj.SetValue(IsFocusedProperty, value);
    //    }

    //    public static readonly DependencyProperty IsFocusedProperty =
    //        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(KeyboardFocusHelper),
    //            new FrameworkPropertyMetadata(default, OnValueChanged)
    //            {
    //                BindsTwoWayByDefault = true,
    //                DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
    //            });

    //    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)d;

    //        if (e.OldValue == null)
    //        {
    //            fe.GotFocus += FrameworkElement_GotFocus;
    //        }

    //        if (!fe.IsVisible)
    //        {
    //            fe.IsVisibleChanged += FrameworkElement_IsVisibleChanged;
    //        }

    //        if (e.NewValue != null && (bool)e.NewValue)
    //        {
    //            SetFocus(fe);
    //        }
    //    }

    //    private static void SetFocus(FrameworkElement fe)
    //    {
    //        if (!fe.Focus())
    //            Keyboard.Focus(fe);
    //    }

    //    private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)sender;
    //        SetIsFocused(fe, true);
    //        fe.LostFocus += FrameworkElement_LostFocus;

    //    }

    //    private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)sender;
    //        SetIsFocused(fe, false);
    //        fe.LostFocus -= FrameworkElement_LostFocus;
    //    }

    //    private static void FrameworkElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)sender;
    //        var isFocused = (bool?)GetIsFocused(fe);
    //        if (fe.IsVisible && isFocused.HasValue && isFocused.Value)
    //        {
    //            fe.IsVisibleChanged -= FrameworkElement_IsVisibleChanged;
    //            SetFocus(fe);
    //        }
    //    }
    //}

    #endregion

    #region KeyboardFocus.On oldschool declaration

    //public static class KeyboardFocus
    //{
    //    public static readonly DependencyProperty OnProperty;

    //    public static void SetOn(UIElement element, FrameworkElement value)
    //    {
    //        element.SetValue(OnProperty, value);
    //    }

    //    public static FrameworkElement GetOn(UIElement element)
    //    {
    //        return (FrameworkElement)element.GetValue(OnProperty);
    //    }

    //    static KeyboardFocus()
    //    {
    //        OnProperty = DependencyProperty.RegisterAttached("On", typeof(FrameworkElement), typeof(KeyboardFocus), new PropertyMetadata(OnSetCallback));
    //    }

    //    private static void OnSetCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var frameworkElement = (FrameworkElement)d;
    //        frameworkElement.Loaded += new RoutedEventHandler(OnLoaded);
    //        frameworkElement.Unloaded += new RoutedEventHandler(OnUnloaded);
    //    }

    //    private static void OnLoaded(object sender, RoutedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)sender;
    //        var target = GetOn(fe);
    //        if (target == null)
    //            return;
    //        Keyboard.Focus(target);
    //    }

    //    private static void OnUnloaded(object sender, RoutedEventArgs e)
    //    {
    //        var fe = (FrameworkElement)sender;
    //        fe.Loaded -= OnLoaded;
    //        fe.Unloaded -= OnUnloaded;
    //    }
    //}

    #endregion

}
