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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyWpfApp
{
    public partial class ReportView : UserControl
    {
        //private readonly Duration _openCloseDuration = new(TimeSpan.FromSeconds(0.2));

        public ReportView()
        {
            InitializeComponent();
        }

        //private void OnChecked(object sender, RoutedEventArgs e)
        //{
        //    if (sender == IsShowLeftPanel)
        //        OpenAnimated(_leftBorder, LeftPane);
        //    else if (sender == IsShowRightPanel)
        //        OpenAnimated(_rightBorder, RightPane);
        //}

        //private void OnUnchecked(object sender, RoutedEventArgs e)
        //{
        //    if (sender == IsShowLeftPanel)
        //        CloseAnimated(_leftBorder);
        //    else if (sender == IsShowRightPanel)
        //        CloseAnimated(_rightBorder);
        //}

        //private void OpenAnimated(Border border, ContentControl content)
        //{
        //    content.Measure(new Size(border.MaxWidth, border.MaxHeight));
        //    var widthAnimation = new DoubleAnimation(0, content.DesiredSize.Width, _openCloseDuration);
        //    border.BeginAnimation(WidthProperty, widthAnimation);
        //}

        //private void CloseAnimated(Border border)
        //{
        //    var widthAnimation = new DoubleAnimation(0, _openCloseDuration);
        //    border.BeginAnimation(WidthProperty, widthAnimation);
        //}
    }
}
