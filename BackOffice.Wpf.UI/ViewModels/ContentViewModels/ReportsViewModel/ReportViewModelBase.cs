using Caliburn.Micro;
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
    public class ReportViewModelBase : Conductor<Screen>.Collection.AllActive
    {
        public UpperPaneViewModel UpperPane { get; }
        public LowerPaneViewModel LowerPane { get; }
        public LeftPaneViewModel LeftPane { get; }
        public RightPaneViewModel RightPane { get; }

        bool _isShowLeftToggle;
        public bool IsShowLeftToggle { get => _isShowLeftToggle; set { Set(ref _isShowLeftToggle, value); if (!value) IsShowLeftPanel = value; } }

        bool _isShowRightToggle;
        public bool IsShowRightToggle { get => _isShowRightToggle; set { Set(ref _isShowRightToggle, value); if (!value) IsShowRightPanel = value; } }

        bool _isShowLeftPanel;
        public bool IsShowLeftPanel { get => _isShowLeftPanel; set => Set(ref _isShowLeftPanel, value); }

        bool _isShowRightPanel;
        public bool IsShowRightPanel { get => _isShowRightPanel; set => Set(ref _isShowRightPanel, value); }

        bool _isUpdating;
        public bool IsUpdating { get => _isUpdating; set => Set(ref _isUpdating, value); }

        public ReportViewModelBase(UpperPaneViewModel upperPane, LowerPaneViewModel lowerPane, LeftPaneViewModel leftPane, RightPaneViewModel rightPane)
        {
            UpperPane = upperPane;
            LowerPane = lowerPane;
            LeftPane = leftPane;
            RightPane = rightPane;

            Items.AddRange(new Screen[] { UpperPane, LowerPane });

            AddComponents();
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
            await Task.Delay(2000);
            IsUpdating = false;
        }

        private void AddComponents()
        {
            var updater = new Button()
            {
                Content = "Update",
                Width = 100,
                Margin = new Thickness(5)
            };

            updater.Click += Update;

            _ = BindingOperations.SetBinding(updater,  IsBusyProperty.ValueProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath("IsUpdating")
            });

            UpperPane.ControlCollection.Add(updater);
        }
    }
}
