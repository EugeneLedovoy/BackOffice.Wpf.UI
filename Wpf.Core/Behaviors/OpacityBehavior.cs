using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Wpf.Core
{
    public class OpacityBehavior : Behavior<UIElement>
    {
        private readonly Duration _duration = new(TimeSpan.FromSeconds(0.2));

        public double PreviousValue { get; private set; }

        public bool IsApply
        {
            get { return (bool)GetValue(IsApplyProperty); }
            set { SetValue(IsApplyProperty, value); }
        }

        public static readonly DependencyProperty IsApplyProperty =
            DependencyProperty.Register("IsApply", typeof(bool), typeof(OpacityBehavior), new PropertyMetadata(false, OnIsApplyChanged));

        private static void OnIsApplyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            if (d is not OpacityBehavior b || b.AssociatedObject is not UIElement el)
                return;

            if (!(bool)e.OldValue && (bool)e.NewValue && Math.Round(el.Opacity, 2) != b.NewValue)
                el.BeginAnimation(UIElement.OpacityProperty, b.GetDoubleAnimation(b.NewValue));

            else if ((bool)e.OldValue && !(bool)e.NewValue && Math.Round(el.Opacity, 2) != b.PreviousValue)
                el.BeginAnimation(UIElement.OpacityProperty, b.GetDoubleAnimation(b.PreviousValue));
        }

        public double NewValue
        {
            get { return (double)GetValue(NewValueProperty); }
            set { SetValue(NewValueProperty, value); }
        }

        public static readonly DependencyProperty NewValueProperty =
            DependencyProperty.Register("NewValue", typeof(double), typeof(UIElement), new PropertyMetadata(1.00d, OnNewValueChanged));

        private static void OnNewValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not OpacityBehavior b || b.AssociatedObject is not UIElement el || Math.Round(el.Opacity, 2) == b.NewValue)
                return;

            b.BeginAnimation(UIElement.OpacityProperty, b.GetDoubleAnimation(b.NewValue));
        }

        private DoubleAnimation GetDoubleAnimation(double newValue)
        {
            return new DoubleAnimation(newValue, _duration);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            PreviousValue = this.AssociatedObject.Opacity;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
