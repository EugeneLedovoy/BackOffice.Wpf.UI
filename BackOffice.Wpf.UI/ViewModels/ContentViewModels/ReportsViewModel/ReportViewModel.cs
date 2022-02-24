using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Core;

namespace MyWpfApp
{
    public class ReportViewModel : ReportViewModelBase
    {
        string _statusText = "Status-huyatus";
        public string StatusText { get => _statusText; set => Set(ref _statusText, value); }

        public ReportViewModel(UpperPaneViewModel upperPane, LowerPaneViewModel lowerPane, LeftPaneViewModel leftPane, RightPaneViewModel rightPane)
            : base(upperPane, lowerPane, leftPane, rightPane)
        {
            FillUpperPane();
            FillLowerPane();
        }

        private void FillUpperPane()
        {
            //UpperPane.ControlCollection.Add(new Button()
            //{
            //    Content = "Update",
            //    Width = 100,
            //    Margin = new System.Windows.Thickness(5)
            //});

            UpperPane.ControlCollection.Add(new TextBlock()
            {
                Text = "User Id",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            });

            UpperPane.ControlCollection.Add(new TextBox()
            {
                Width = 150,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            });

            var chbx = new CheckBox
            {
                Content = "Show fingroups",
                Margin = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            var stack = new StackPanel { Orientation = Orientation.Horizontal, Opacity = 0 };
            var border = new Border { Width = 0, Child = stack };

            BindingOperations.SetBinding(border, WidthAnimationProperty.ValueProperty, new Binding
            {
                Source = chbx,
                Path = new PropertyPath(nameof(chbx.IsChecked)),
            });

            BindingOperations.SetBinding(stack, OpacityAnimationProperty.ValueProperty, new Binding
            {
                Source = chbx,
                Path = new PropertyPath(nameof(chbx.IsChecked)),
            });

            var txtblk = new TextBlock()
            {
                Text = "Fingroup",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var cmbx = new ComboBox()
            {
                ItemsSource = new ObservableCollection<ComboBoxItem>(new ComboBoxItem[]
                {
                    new ComboBoxItem { Content = "-not selected-", IsSelected = true, },
                    new ComboBoxItem { Content = "Agents", },
                    new ComboBoxItem { Content = "Partners", },
                }),

                SelectedIndex = 0,
                Margin = new Thickness(5),
                Width = 150,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            stack.Children.Insert(0, txtblk);
            stack.Children.Insert(1, cmbx);

            UpperPane.ControlCollection.Add(chbx);
            UpperPane.ControlCollection.Add(border);

            var b = new Button
            {
                Content = "New rep",
                Width = 100,
                Margin = new Thickness(5)
            };

            b.Loaded += OnBLoaded;
            UpperPane.ControlCollection.Add(b);

        }

        private void OnBLoaded(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            b.Click += OnBClick;
            b.Unloaded += OnBUnloaded;
        }

        private void OnBUnloaded(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            b.Click -= OnBClick;
            b.Unloaded -= OnBUnloaded;
        }

        private void OnBClick(object sender, RoutedEventArgs e)
        {
            var r = new MainView()
            {
                Title = "WebBetDetails",
            };
            var c = new TextBlock
            {
                Text = "Ya otchyoteg",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 32,
            };
            r.ActiveItem.Content = c;
            r.Show();
        }

        private void FillLowerPane()
        {
            var chb1 = new CheckBox
            {
                Content = "Show the left panel",
                Margin = new Thickness(0, 0, 5, 0),
            };

            BindingOperations.SetBinding(chb1, CheckBox.IsCheckedProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(base.IsShowLeftToggle)),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            var chb2 = new CheckBox
            {
                Content = "Show the right panel",
                Margin = new Thickness(5, 0, 5, 0),
            };

            BindingOperations.SetBinding(chb2, CheckBox.IsCheckedProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(base.IsShowRightToggle)),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            LowerPane.ControlCollection.Add(chb1);
            LowerPane.ControlCollection.Add(chb2);
        }


    }
}
