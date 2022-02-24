using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfApp
{
    public class AnimationAttachedPropertyBase<TExtendingClass> : AttachedPropertyBase<TExtendingClass, bool> where TExtendingClass : class, new()
    {
        //ToDo:
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // ToDo: Logic


            DoAnimation(d, (bool)e.NewValue);
        }

        // ToDo: ponder out whether the flag isFirstLoad is necessary
        protected virtual void DoAnimation(DependencyObject d, bool value/*, bool isFirstLoad*/) { }
    }
}
