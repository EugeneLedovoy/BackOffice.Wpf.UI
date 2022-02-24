using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wpf.Core;

namespace MyWpfApp
{
    public partial class MainView : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        private readonly WindowResizer _windowResizer;
        private WindowDockPosition _dockPosition = WindowDockPosition.Undocked;

        #region XAML Props

        public bool Borderless => base.WindowState == WindowState.Maximized || _dockPosition != WindowDockPosition.Undocked;
        public int FlatBorderThickness => Borderless && base.WindowState != WindowState.Maximized ? 1 : 0;
        public int ResizeBorder => base.WindowState == WindowState.Maximized ? 0 : 4;
        public Thickness ResizeBorderThickness => new(OuterMarginSize.Left + ResizeBorder,
                                                      OuterMarginSize.Top + ResizeBorder,
                                                      OuterMarginSize.Right + ResizeBorder,
                                                      OuterMarginSize.Bottom + ResizeBorder);

        private Thickness _outerMarginSize = new(5);
        public Thickness OuterMarginSize
        {
            get => base.WindowState == WindowState.Maximized ? _windowResizer.CurrentMonitorMargin : (Borderless ? new Thickness(0) : _outerMarginSize);
            set { _outerMarginSize = value; OnPropertyChanged(); OnPropertyChanged(nameof(ResizeBorderThickness)); OnPropertyChanged(nameof(ResizeBorder)); OnPropertyChanged(nameof(Borderless)); }
        }

        private int _windowRadius = 10;
        public int WindowRadius
        {
            get => Borderless ? 0 : _windowRadius;
            set { _windowRadius = value; OnPropertyChanged(); OnPropertyChanged(nameof(WindowCornerRadius)); OnPropertyChanged(nameof(Borderless)); }
        }

        public CornerRadius WindowCornerRadius => new(WindowRadius);

        private bool _isMoving;
        public bool IsMoving { get => _isMoving; set { _isMoving = value; OnPropertyChanged(); } }

        bool _isShadow;
        public bool IsShadow { get => _isShadow; set { _isShadow = value; OnPropertyChanged(); } }

        #endregion

        public MainView()
        {
            InitializeComponent();

            base.StateChanged += (s, e) => WindowResized();

            _windowResizer = new WindowResizer(this);

            _windowResizer.WindowDockChanged += dock =>
            {
                _dockPosition = dock;
                WindowResized();
            };

            _windowResizer.WindowStartedMove += () => IsMoving = true;

            _windowResizer.WindowFinishedMove += () =>
            {
                IsMoving = false;

                if (_dockPosition == WindowDockPosition.Undocked && base.Top == _windowResizer.CurrentScreenSize.Top)
                    base.Top = -OuterMarginSize.Top;
            };
        }

        private void WindowResized()
        {
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(FlatBorderThickness));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }

        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            //SystemCommands.MinimizeWindow(this);
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeClick(object sender, RoutedEventArgs e)
        {
            base.WindowState ^= WindowState.Maximized;
            //if(this.WindowState == System.Windows.WindowState.Maximized)
            //    SystemCommands.RestoreWindow(this);
            //else
            //    SystemCommands.MaximizeWindow(this);
        }

        private void OnCloseIconClick(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void OnMainIconClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.ShowSystemMenu(this, GetMousePosition());
        }

        private Point GetMousePosition()
        {
            return _windowResizer.GetCursorPosition();
        }

        private void OnMainViewActivated(object sender, EventArgs e)
        {
            IsShadow = false;
        }

        private void OnMainViewDeactivated(object sender, EventArgs e)
        {
            IsShadow = true;
        }

        //private Point GetMousePosition(Window window)
        //{
        //    var p = Mouse.GetPosition(window);
        //    return new Point(p.X + window.Left, p.Y + window.Top);
        //}

    }
}
