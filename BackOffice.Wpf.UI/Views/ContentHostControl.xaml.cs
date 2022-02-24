using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWpfApp
{
    public partial class ContentHostControl
    {
        public object CurrentViewModel
        {
            get { return (object)GetValue(CurrentViewModelProperty); }
            set { SetValue(CurrentViewModelProperty, value); }
        }

        public static readonly DependencyProperty CurrentViewModelProperty =
            DependencyProperty.Register("CurrentViewModel", typeof(object), typeof(ContentHostControl), new PropertyMetadata(null, null, OnCurrentViewModelChanging));

        private static object OnCurrentViewModelChanging(DependencyObject d, object baseValue)
        {

            if (baseValue == null)
                return baseValue;

            var currentView = (UserControl)GetView(baseValue);

            var host = d as ContentHostControl;
            var newActiveItem = host.NewActiveItem;
            var oldActiveItem = host.OldActiveItem;

            var oldViewContent = (UserControl)newActiveItem.Content;

            newActiveItem.Content = null;

            oldActiveItem.Content = oldViewContent;

            Task.Delay(200)
                .ContinueWith(t => oldActiveItem.Content = null, TaskScheduler.FromCurrentSynchronizationContext());

            newActiveItem.Content = currentView;

            return baseValue;
        }

        private static object GetView(object vm)
        {
            if (vm.GetType() == typeof(LoginMainViewModel))
            {
                var lmv = (LoginMainView)Caliburn.Micro.ViewLocator.GetOrCreateViewType(typeof(LoginMainView));
                lmv.DataContext = vm;
                return lmv;
            }

            else if (vm.GetType() == typeof(ContentMainViewModel))
            {
                var cmv = (ContentMainView)Caliburn.Micro.ViewLocator.GetOrCreateViewType(typeof(ContentMainView));
                cmv.DataContext = vm;
                return cmv;
            }

            return null;
        }

        public ContentHostControl()
        {
            InitializeComponent();
        }
    }
}
