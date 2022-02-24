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
    public class HeightAnimationBehavior : Behavior<ContentControl>
    {
        public double InitialHeight { get; private set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            InitialHeight = AssociatedObject.Height;
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
            DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(HeightAnimationBehavior), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.15))));


        public IEnumerable ToggleCollection
        {
            get { return (IEnumerable)GetValue(ToggleCollectionProperty); }
            set { SetValue(ToggleCollectionProperty, value); }
        }

        public static readonly DependencyProperty ToggleCollectionProperty =
            DependencyProperty.Register("ToggleCollection", typeof(IEnumerable), typeof(HeightAnimationBehavior), new PropertyMetadata(default));


        public double ExtraHeight
        {
            get { return (double)GetValue(ExtraHeightProperty); }
            set { SetValue(ExtraHeightProperty, value); }
        }

        public static readonly DependencyProperty ExtraHeightProperty =
            DependencyProperty.Register("ExtraHeight", typeof(double), typeof(HeightAnimationBehavior), new PropertyMetadata(0.0d));


        public Decorator ContentToResize
        {
            get { return (Decorator)GetValue(ContentToResizeProperty); }
            set { SetValue(ContentToResizeProperty, value); }
        }

        public static readonly DependencyProperty ContentToResizeProperty =
            DependencyProperty.Register("ContentToResize", typeof(Decorator), typeof(HeightAnimationBehavior), new PropertyMetadata(default));


        public FrameworkElement InnerContent
        {
            get { return (FrameworkElement)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(FrameworkElement), typeof(HeightAnimationBehavior), new PropertyMetadata(default));


        public bool IsApply
        {
            get { return (bool)GetValue(IsApplyProperty); }
            set { SetValue(IsApplyProperty, value); }
        }

        public static readonly DependencyProperty IsApplyProperty =
            DependencyProperty.Register("IsApply", typeof(bool), typeof(HeightAnimationBehavior), new PropertyMetadata(false, OnIsApplyChanged));

        #endregion

        private static void OnIsApplyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not HeightAnimationBehavior b || b.AssociatedObject is not ContentControl cc)
                return;

            if ((bool)e.NewValue)
            {
                Task.Delay(5).ContinueWith(task =>
                {
                    var desiredHeight = b.GetContentDesiredHeight(b.ContentToResize, b.InnerContent, b.ExtraHeight);
                    var newHeight = b.InitialHeight + desiredHeight;

                    if ((b.ToggleCollection != default && b.ToggleCollection.OfType<ToggleButton>().Any(t => t.IsChecked ?? false) && cc.ActualHeight >= newHeight)
                        || (b.ToggleCollection == default && cc.ActualHeight >= newHeight))
                        return;

                    b.ExpandWindow(cc, newHeight);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                if (b.ToggleCollection != default && b.ToggleCollection.OfType<ToggleButton>().Any(t => t.IsChecked ?? false))
                    return;

                b.ShrinkWindow(cc);
            }

        }

        private double GetContentDesiredHeight(Decorator decorator, FrameworkElement content, double extraHeight = 0)
        {
            content.Measure(new Size(decorator.MaxHeight, decorator.MaxHeight));
            var desiredHeight = content.DesiredSize.Height + content.Margin.Top + content.Margin.Bottom + extraHeight;
            if (decorator is Border brd)
                desiredHeight += brd.Padding.Top + brd.Padding.Bottom;

            foreach (var b in decorator.Child.FindVisualChildren<Border>())
                desiredHeight += b.Padding.Top + b.Padding.Bottom + b.Margin.Top + b.Margin.Bottom;

            return desiredHeight;
        }

        private void ExpandWindow(FrameworkElement fe, double newHeight)
        {
            var da = new DoubleAnimation(fe.ActualHeight, newHeight, OpenCloseDuration);
            fe.BeginAnimation(FrameworkElement.HeightProperty, da);
        }

        private void ShrinkWindow(FrameworkElement fe)
        {
            var da = new DoubleAnimation(InitialHeight, OpenCloseDuration);
            fe.BeginAnimation(FrameworkElement.HeightProperty, da);
        }

    }
}
