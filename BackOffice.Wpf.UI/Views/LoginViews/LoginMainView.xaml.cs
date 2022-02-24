using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Core;

namespace MyWpfApp
{
    public partial class LoginMainView : UserControl
    {
        DependencyPropertyDescriptor _contentHook;
        bool _isFirst = true;
        object _previousContent;
        bool _isContentSubstitution;

        FrameworkElement PrimalParent => (FrameworkElement)this.GetPrimalParent();

        public LoginMainView()
        {
            InitializeComponent();

            //Loaded += (s, e) => this.RunFromLeftStoryboard(PrimalParent.ActualWidth);

            DataContextChanged += LoginMainView_DataContextChanged;

            _contentHook = DependencyPropertyDescriptor.FromProperty(ContentProperty, typeof(ContentControl));
            _contentHook.AddValueChanged(ActiveItem, OnChanged);
        }

        private void LoginMainView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Please, don't kick me,
            // I've been trying but have no clue yet how to animate out the LoginMainView another way after successfull 2FA code validation
            // because of fucking Caliburn.Micro
            ((IViewModelChanging)DataContext).ViewModelChanging += On2FAScreenDone;
        }

        private void On2FAScreenDone(object sender, EventArgs e)
        {
            this.RunToRightStoryboard(PrimalParent.ActualWidth);
        }

        private async void OnChanged(object sender, EventArgs e)
        {
            if (_isContentSubstitution)
                return;

            var offset = PrimalParent.ActualWidth;

            if (_isFirst)
            {
                this.RunFromLeftStoryboard(offset);
                _isFirst = false;
            }
            else
            {
                var currentContent = ActiveItem.Content;
                bool isBack = _previousContent.GetType() == typeof(Login2FAView) && currentContent.GetType() == typeof(LoginCredentialsView);

                _isContentSubstitution = true;

                ActiveItem.Content = _previousContent;

                if (isBack)
                {
                    this.RunToLeftStoryboard(offset);
                }
                else
                {
                    this.RunToRightStoryboard(offset);
                }
                
                await Task.Delay(200);

                ActiveItem.Content = currentContent;

                if (isBack)
                {
                    this.RunFromRightStoryboard(offset);
                }
                else
                {
                    this.RunFromLeftStoryboard(offset);
                }
                
                _isContentSubstitution = false;
            }
            _previousContent = ActiveItem.Content;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ((LoginMainViewModel)DataContext).ViewModelChanging -= On2FAScreenDone;
            DataContextChanged -= LoginMainView_DataContextChanged;
            _contentHook?.RemoveValueChanged(ActiveItem, OnChanged);
            _contentHook = null;
            _previousContent = null;
        }
    }
}
