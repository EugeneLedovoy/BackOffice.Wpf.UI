using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Core
{
    public class TextBoxEnterKeyUpdateBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject != null)
            {
                base.OnAttached();
                this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == Key.Return && e.Key == Key.Enter)
            {
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}
