using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Wpf.Core
{
    #region DoubleAnimation Props
    public class DoubleValueForAnimationProperty : AttachedPropertyBase<DoubleValueForAnimationProperty, double>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = (double)e.NewValue;
            var self = (UIElement)d;
            var da = new DoubleAnimation(newValue, new Duration(TimeSpan.FromSeconds(0.1)));
            self.BeginAnimation(ToDoubleAnimationProperty.ValueProperty, da);
        }
    }

    public class ToDoubleAnimationProperty : AttachedPropertyBase<ToDoubleAnimationProperty, double> { }
    #endregion

    public class WidthAnimationProperty : AttachedPropertyBase<WidthAnimationProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Decorator dec)
                return;

            if ((bool)e.NewValue)
            {
                // задежка для рендеринга содержимого ContentControl (если он есть)
                Task.Delay(5).ContinueWith(t =>
                {
                    if (dec.Child is not FrameworkElement fe)
                        return;

                    OpenHorizontalAnimated(dec, fe, new Duration(TimeSpan.FromSeconds(0.2)));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                dec.BeginAnimation(FrameworkElement.WidthProperty, new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2))));
            }
        }

        public static double GetDesiredWidth(Decorator outerBorder, FrameworkElement innerContent)
        {
            innerContent.Measure(new Size(outerBorder.MaxWidth, outerBorder.MaxHeight));
            var width = innerContent.DesiredSize.Width + innerContent.Margin.Left + innerContent.Margin.Right;
            if (outerBorder is Border brd)
                width += brd.Padding.Left + brd.Padding.Right + brd.Margin.Left + brd.Margin.Right;

            foreach (var b in outerBorder.Child.FindVisualChildren<Border>())
                width += b.Padding.Left + b.Padding.Right + b.Margin.Left + b.Margin.Right;

            return width;
        }

        private static void OpenHorizontalAnimated(Decorator outerBorder, FrameworkElement innerContent, Duration dur)
        {
            var da = new DoubleAnimation(0, GetDesiredWidth(outerBorder, innerContent), dur);
            outerBorder.BeginAnimation(FrameworkElement.WidthProperty, da);
        }

        public static DoubleAnimation GetDoubleAnimation(Decorator outerBorder, FrameworkElement innerContent, bool isOpen)
        {
            return isOpen ? AnimationHelper.CreateDoubleAnimation(nameof(FrameworkElement.Width), GetDesiredWidth(outerBorder, innerContent), 0.2d)
                          : AnimationHelper.CreateDoubleAnimation(nameof(FrameworkElement.Width), 0d, 0.2d);
        }
    }

    public class WidthAndOpacityAnimationProperty : AttachedPropertyBase<WidthAndOpacityAnimationProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Decorator dec)
                return;

            Task.Delay(5).ContinueWith(t =>
            {
                if (dec.Child is not FrameworkElement fe)
                    return;

                var newValue = (bool)e.NewValue;
                var widthda = WidthAnimationProperty.GetDoubleAnimation(dec, fe, newValue);
                var opda = OpacityAnimationProperty.GetDoubleAnimation(newValue);
                dec.RunNewStoryboard(new List<Timeline> { widthda, opda });
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public class HeightAnimationProperty : AttachedPropertyBase<HeightAnimationProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Decorator dec)
                return;

            if ((bool)e.NewValue)
            {
                // задежка для рендеринга содержимого ContentControl (если он есть)
                Task.Delay(5).ContinueWith(t =>
                {
                    if (dec.Child is not FrameworkElement fe)
                        return;

                    OpenVerticalAnimated(dec, fe, new Duration(TimeSpan.FromSeconds(0.2)));
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                dec.BeginAnimation(FrameworkElement.HeightProperty, new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2))));
            }
        }

        private static void OpenVerticalAnimated(Decorator outerBorder, FrameworkElement innerContent, Duration dur)
        {
            var da = new DoubleAnimation(0, GetDesiredHeight(outerBorder, innerContent), dur);
            outerBorder.BeginAnimation(FrameworkElement.HeightProperty, da);
        }

        public static double GetDesiredHeight(Decorator outerBorder, FrameworkElement innerContent)
        {
            innerContent.Measure(new Size(outerBorder.MaxWidth, outerBorder.MaxHeight));
            var height = innerContent.DesiredSize.Height + innerContent.Margin.Top + innerContent.Margin.Bottom;
            if (outerBorder is Border brd)
                height += brd.Padding.Top + brd.Padding.Bottom + brd.Margin.Top + brd.Margin.Bottom;

            foreach (var b in outerBorder.Child.FindVisualChildren<Border>())
                height += b.Padding.Top + b.Padding.Bottom + b.Margin.Top + b.Margin.Bottom;

            return height;
        }

        public static DoubleAnimation GetDoubleAnimation(Decorator outerBorder, FrameworkElement innerContent, bool isOpen)
        {
            return isOpen ? AnimationHelper.CreateDoubleAnimation(nameof(FrameworkElement.Height), GetDesiredHeight(outerBorder, innerContent), 0.2d)
                          : AnimationHelper.CreateDoubleAnimation(nameof(FrameworkElement.Height), 0d, 0.2d);
        }
    }

    public class HeightAndOpacityAnimationProperty : AttachedPropertyBase<HeightAndOpacityAnimationProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Decorator dec)
                return;

            Task.Delay(5).ContinueWith(t =>
            {
                if (dec.Child is not FrameworkElement fe)
                    return;

                var newValue = (bool)e.NewValue;
                var heightda = HeightAnimationProperty.GetDoubleAnimation(dec, fe, newValue);
                var opda = OpacityAnimationProperty.GetDoubleAnimation(newValue);
                dec.RunNewStoryboard(new List<Timeline> { heightda, opda });
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public class OpacityAnimationProperty : AttachedPropertyBase<OpacityAnimationProperty, bool>
    {
        protected override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement uie)
                return;

            if ((bool)e.NewValue)
            {
                uie.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.25))));
            }
            else
            {
                uie.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.15))));
            }
        }

        public static DoubleAnimation GetDoubleAnimation(bool isOpen)
        {
            return isOpen ? AnimationHelper.CreateDoubleAnimation(nameof(UIElement.Opacity), 1d, 0.25d, beginTime: 0.1d)
                          : AnimationHelper.CreateDoubleAnimation(nameof(UIElement.Opacity), 0d, 0.15d);
        }
    }

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
