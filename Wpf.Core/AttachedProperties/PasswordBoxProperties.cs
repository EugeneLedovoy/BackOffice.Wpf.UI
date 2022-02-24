using System.Windows;
using System.Windows.Controls;

namespace Wpf.Core
{
    public class PasswordProperty : AttachedPropertyBase<PasswordProperty, string>
    {
        private bool _isUpdating;

        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isUpdating)
                return;

            if (d is PasswordBox pb)
            {
                pb.PasswordChanged -= OnPasswordChanged;
                pb.Password = e.NewValue?.ToString();
                pb.PasswordChanged += OnPasswordChanged;
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;

            _isUpdating = true;
            SetValue(pb, pb.Password);
            _isUpdating = false;
        }
    }

    public class HasTextProperty : AttachedPropertyBase<HasTextProperty, bool>
    {
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, ((PasswordBox)sender).Password.Length > 0);
        }
    }

    public class HookOnPasswordChangingProperty : AttachedPropertyBase<HookOnPasswordChangingProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox p)
            {
                p.PasswordChanged -= OnPasswordValueChanged;

                if ((bool)e.NewValue)
                {
                    HasTextProperty.SetValue(p);
                    p.PasswordChanged += OnPasswordValueChanged;
                }
            }
        }
        private void OnPasswordValueChanged(object sender, RoutedEventArgs e)
        {
            HasTextProperty.SetValue((PasswordBox)sender);
        }
    }

    #region oldschool declarations

    //public static class PasswordBoxHelper
    //{
    //    #region PasswordProperty
    //    //public static string GetPassword(DependencyObject dp)
    //    //{
    //    //    return (string)dp.GetValue(PasswordProperty);
    //    //}

    //    //public static void SetPassword(DependencyObject dp, string value)
    //    //{
    //    //    dp.SetValue(PasswordProperty, value);
    //    //}

    //    //public static readonly DependencyProperty PasswordProperty =
    //    //    DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper),
    //    //        new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged)
    //    //        {
    //    //            BindsTwoWayByDefault = true,
    //    //            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
    //    //        });

    //    //static bool _isUpdating;

    //    //private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    //{
    //    //    if (_isUpdating)
    //    //        return;

    //    //    if (d is PasswordBox pb)
    //    //    {
    //    //        pb.PasswordChanged -= OnPasswordChanged;
    //    //        pb.Password = e.NewValue?.ToString();
    //    //        pb.PasswordChanged += OnPasswordChanged;
    //    //    }
    //    //}

    //    //private static void OnPasswordChanged(object sender, RoutedEventArgs e)
    //    //{
    //    //    var pb = (PasswordBox)sender;

    //    //    _isUpdating = true;
    //    //    SetPassword(pb, pb.Password);
    //    //    _isUpdating = false;
    //    //}
    //    #endregion

    //    #region HasText
    //    //public static bool GetHasText(DependencyObject obj)
    //    //{
    //    //    return (bool)obj.GetValue(HasTextProperty);
    //    //}

    //    //public static void SetHasText(DependencyObject obj, bool value)
    //    //{
    //    //    obj.SetValue(HasTextProperty, value);
    //    //}

    //    //public static readonly DependencyProperty HasTextProperty =
    //    //    DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));


    //    //public static bool GetHookOnPasswordChanging(DependencyObject obj)
    //    //{
    //    //    return (bool)obj.GetValue(HookOnPasswordChangingProperty);
    //    //}

    //    //public static void SetHookOnPasswordChanging(DependencyObject obj, bool value)
    //    //{
    //    //    obj.SetValue(HookOnPasswordChangingProperty, value);
    //    //}

    //    //public static readonly DependencyProperty HookOnPasswordChangingProperty =
    //    //    DependencyProperty.RegisterAttached("HookOnPasswordChanging", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnHookChanged));

    //    //private static void OnHookChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    //{
    //    //    if (d is PasswordBox p)
    //    //    {
    //    //        p.PasswordChanged -= OnPasswordValueChanged;

    //    //        if ((bool)e.NewValue)
    //    //        {
    //    //            SetHasText(p);
    //    //            p.PasswordChanged += OnPasswordValueChanged;
    //    //        }
    //    //    }
    //    //}

    //    //private static void OnPasswordValueChanged(object sender, RoutedEventArgs e)
    //    //{
    //    //    SetHasText((PasswordBox)sender);
    //    //}

    //    //private static void SetHasText(PasswordBox p)
    //    //{
    //    //    p.SetValue(HasTextProperty, p.Password.Length > 0);
    //    //}
    //    #endregion
    //}

    #endregion
}
