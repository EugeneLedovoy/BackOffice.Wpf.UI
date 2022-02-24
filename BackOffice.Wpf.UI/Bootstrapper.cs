using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            _container.PerRequest<MainViewModel>();

            _container.PerRequest<LoginMainViewModel>();
            _container.PerRequest<LoginCredentialsViewModel>();
            _container.PerRequest<Login2FAViewModel>();

            _container.PerRequest<SplashScreenViewModel>();

            _container.PerRequest<ContentMainViewModel>();
            _container.PerRequest<SettingsViewModel>();
            _container.PerRequest<ReportViewModel>();

            _container.PerRequest<UpperPaneViewModel>();
            _container.PerRequest<LowerPaneViewModel>();
            _container.PerRequest<LeftPaneViewModel>();
            _container.PerRequest<RightPaneViewModel>();

            _container.PerRequest<MenuViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _ = DisplayRootViewFor<MainViewModel>();
        }

    }
}
