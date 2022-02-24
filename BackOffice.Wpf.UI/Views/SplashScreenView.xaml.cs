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
using Wpf.Core;

namespace MyWpfApp
{
    public partial class SplashScreenView : UserControl
    {
        FrameworkElement PrimalParent => (FrameworkElement)this.GetPrimalParent();

        public SplashScreenView()
        {
            InitializeComponent();
            Loaded += SplashScreenView_Loaded;
            DataContextChanged += SplashScreenView_DataContextChanged;
        }

        private void SplashScreenView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // The same bullshit as one at the LoginMainView.xaml.cs
            ((IViewModelChanging)DataContext).ViewModelChanging += OnLoadDataDone;
        }

        private void SplashScreenView_Loaded(object sender, RoutedEventArgs e)
        {
            this.RunFromLeftStoryboard(PrimalParent.ActualWidth);
        }

        private void OnLoadDataDone(object sender, EventArgs e)
        {
            this.RunToRightStoryboard(PrimalParent.ActualWidth);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= SplashScreenView_Loaded;
            ((IViewModelChanging)DataContext).ViewModelChanging -= OnLoadDataDone;
            DataContextChanged -= SplashScreenView_DataContextChanged;
        }
    }
}
