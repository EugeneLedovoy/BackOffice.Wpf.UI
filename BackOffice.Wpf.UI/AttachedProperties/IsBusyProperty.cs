using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfApp
{
    public class IsBusyProperty : AttachedPropertyBase<IsBusyProperty, bool>
    {

    }

    #region IsBusyProperty in oldschool manner
    //public static class ButtonHelper
    //{
    //    public static bool GetIsBusy(DependencyObject obj)
    //    {
    //        return (bool)obj.GetValue(IsBusyProperty);
    //    }

    //    public static void SetIsBusy(DependencyObject obj, bool value)
    //    {
    //        obj.SetValue(IsBusyProperty, value);
    //    }

    //    public static readonly DependencyProperty IsBusyProperty =
    //        DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(ButtonHelper), new PropertyMetadata(false));
    //}
    #endregion

}
