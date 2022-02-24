using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Wpf.Core
{
    public class WidthAnimationBehavior : Behavior<ContentControl>
    {
        public double InitialWidth { get; private set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            InitialWidth = AssociatedObject.Width;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        #region DpProps

        public Duration OpenCloseDuration
        {
            get { return (Duration)GetValue(OpenCloseDurationProperty); }
            set { SetValue(OpenCloseDurationProperty, value); }
        }

        public static readonly DependencyProperty OpenCloseDurationProperty =
            DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(WidthAnimationBehavior), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.15))));


        public IEnumerable ToggleCollection
        {
            get { return (IEnumerable)GetValue(ToggleCollectionProperty); }
            set { SetValue(ToggleCollectionProperty, value); }
        }

        public static readonly DependencyProperty ToggleCollectionProperty =
            DependencyProperty.Register("ToggleCollection", typeof(IEnumerable), typeof(WidthAnimationBehavior), new PropertyMetadata(default));


        public double ExtraWidth
        {
            get { return (double)GetValue(ExtraWidthProperty); }
            set { SetValue(ExtraWidthProperty, value); }
        }

        public static readonly DependencyProperty ExtraWidthProperty =
            DependencyProperty.Register("ExtraWidth", typeof(double), typeof(WidthAnimationBehavior), new PropertyMetadata(0.0d));


        public Decorator ContentToResize
        {
            get { return (Decorator)GetValue(ContentToResizeProperty); }
            set { SetValue(ContentToResizeProperty, value); }
        }

        public static readonly DependencyProperty ContentToResizeProperty =
            DependencyProperty.Register("ContentToResize", typeof(Decorator), typeof(WidthAnimationBehavior), new PropertyMetadata(default));


        public FrameworkElement InnerContent
        {
            get { return (FrameworkElement)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(FrameworkElement), typeof(WidthAnimationBehavior), new PropertyMetadata(default));


        public bool IsApply
        {
            get { return (bool)GetValue(IsApplyProperty); }
            set { SetValue(IsApplyProperty, value); }
        }

        public static readonly DependencyProperty IsApplyProperty =
            DependencyProperty.Register("IsApply", typeof(bool), typeof(WidthAnimationBehavior), new PropertyMetadata(false, OnIsApplyChanged));

        #endregion

        private static void OnIsApplyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WidthAnimationBehavior b || b.AssociatedObject is not ContentControl cc)
                return;

            if ((bool)e.NewValue)
            {
                Task.Delay(5).ContinueWith(task =>
                {
                    var desiredWidth = b.GetContentDesiredWidth(b.ContentToResize, b.InnerContent, b.ExtraWidth);
                    var newWidth = b.InitialWidth + desiredWidth;

                    if ((b.ToggleCollection != default && b.ToggleCollection.OfType<ToggleButton>().Any(t => t.IsChecked ?? false) && cc.ActualWidth >= newWidth)
                        || (b.ToggleCollection == default && cc.ActualWidth >= newWidth))
                        return;

                    b.ExpandWindow(cc, newWidth);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                if (b.ToggleCollection != default && b.ToggleCollection.OfType<ToggleButton>().Any(t => t.IsChecked ?? false))
                    return;

                b.ShrinkWindow(cc);
            }

        }

        private double GetContentDesiredWidth(Decorator decorator, FrameworkElement content, double extraWidth = 0)
        {
            content.Measure(new Size(decorator.MaxWidth, decorator.MaxHeight));
            var desiredWidth = content.DesiredSize.Width + content.Margin.Left + content.Margin.Right + extraWidth;
            if (decorator is Border brd)
                desiredWidth += brd.Padding.Left + brd.Padding.Right;

            foreach (var b in decorator.Child.FindVisualChildren<Border>())
                desiredWidth += b.Padding.Left + b.Padding.Right + b.Margin.Left + b.Margin.Right;

            return desiredWidth;
        }

        private void ExpandWindow(FrameworkElement fe, double newWidth)
        {
            var da = new DoubleAnimation(fe.ActualWidth, newWidth, OpenCloseDuration);
            fe.BeginAnimation(FrameworkElement.WidthProperty, da);
        }

        private void ShrinkWindow(FrameworkElement fe)
        {
            var da = new DoubleAnimation(InitialWidth, OpenCloseDuration);
            fe.BeginAnimation(FrameworkElement.WidthProperty, da);
        }

    }
}
