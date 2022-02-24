using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MyWpfApp
{
    #region DoubleAnimation Props
    public class DoubleValueForAnimationProperty : AttachedPropertyBase<DoubleValueForAnimationProperty, double>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            var self = (FrameworkElement)d;
            var da = new DoubleAnimation(newValue, new Duration(TimeSpan.FromSeconds(0.1)));
            self.BeginAnimation(ToDoubleAnimationProperty.ValueProperty, da);
        }
    }

    public class ToDoubleAnimationProperty : AttachedPropertyBase<ToDoubleAnimationProperty, double>
    {

    }
    #endregion

    #region DoubleAnimation Props (oldschool way)
    //public static class AnimatableDoubleHelper
    //{
    //    // Это attached property OriginalProperty. К нему мы будем привязывать свойство из VM,
    //    // и получать нотификацию об его изменении
    //    public static double GetOriginalProperty(DependencyObject obj) => (double)obj.GetValue(OriginalPropertyProperty);
    //    public static void SetOriginalProperty(DependencyObject obj, double value) => obj.SetValue(OriginalPropertyProperty, value);

    //    public static readonly DependencyProperty OriginalPropertyProperty =
    //        DependencyProperty.RegisterAttached("OriginalProperty", typeof(double), typeof(AnimatableDoubleHelper), new PropertyMetadata(OnOriginalUpdated));

    //    // это "производное" attached property, которое будет
    //    // анимированно "догонять" OriginalProperty
    //    public static double GetAnimatedProperty(DependencyObject obj) => (double)obj.GetValue(AnimatedPropertyProperty);
    //    public static void SetAnimatedProperty(DependencyObject obj, double value) => obj.SetValue(AnimatedPropertyProperty, value);

    //    public static readonly DependencyProperty AnimatedPropertyProperty =
    //        DependencyProperty.RegisterAttached("AnimatedProperty", typeof(double), typeof(AnimatableDoubleHelper));

    //    static void OnOriginalUpdated(DependencyObject o, DependencyPropertyChangedEventArgs e)
    //    {
    //        double newValue = (double)e.NewValue;
    //        var self = (FrameworkElement)o;
    //        var da = new DoubleAnimation(newValue, new Duration(TimeSpan.FromSeconds(0.1)));
    //        self.BeginAnimation(AnimatedPropertyProperty, da);
    //    }
    //}

    #endregion


    // ToDo:
    //public class AnimateFadeInProperty : AnimationAttachedPropertyBase<AnimateFadeInProperty>
    //{
    //    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    //    {
    //        if (value)
    //            // Animate in
    //            await element.FadeInAsync(firstLoad, firstLoad ? 0 : 0.3f);
    //        else
    //            // Animate out
    //            await element.FadeOutAsync(firstLoad ? 0 : 0.3f);
    //    }
    //}

}
