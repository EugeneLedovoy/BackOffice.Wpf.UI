using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Controls
{
    [TemplateVisualState(Name = "OpenState")]
    [TemplateVisualState(Name = "CloseState")]
    public class ModalPopup : ContentControl
    {
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(ModalPopup), new PropertyMetadata(false, OnOpenStateChanged));

        private static void OnOpenStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ModalPopup p)
            {
                GoToState(p, p.IsOpen ? "OpenState" : "CloseState");
            }
        }

        private static void GoToState(FrameworkElement e, string stateName)
        {
            _ = VisualStateManager.GoToState(e, stateName, false);
        }

        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register("ContentBackground", typeof(Brush), typeof(ModalPopup), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        static ModalPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalPopup), new FrameworkPropertyMetadata(typeof(ModalPopup)));
            BackgroundProperty.OverrideMetadata(typeof(ModalPopup), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black) { Opacity = 0.15 }));
        }
    }
}
