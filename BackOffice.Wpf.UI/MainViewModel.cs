using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<On2FAMessage>, IHandle<OnCancelMessage>, IHandle<OnLogoutMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private LoginMainViewModel _loginMainViewModel;
        private readonly SplashScreenViewModel _splashScreenViewModel;
        private readonly ContentMainViewModel _contentMainViewModel;

        public MainViewModel(IEventAggregator eventAggregator, LoginMainViewModel loginMainViewModel, SplashScreenViewModel splashScreenViewModel, ContentMainViewModel contentMainViewModel)
        {
            _eventAggregator = eventAggregator;
            _loginMainViewModel = loginMainViewModel;
            _splashScreenViewModel = splashScreenViewModel;
            _contentMainViewModel = contentMainViewModel;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnPublishedThread(this);
            await Task.WhenAll(base.OnActivateAsync(cancellationToken), ActivateItemAsync(_loginMainViewModel, cancellationToken));
        }

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            await base.OnDeactivateAsync(close, cancellationToken);
        }

        public async Task HandleAsync(On2FAMessage message, CancellationToken cancellationToken)
        {

            #region for animating out the content of the LoginMainView
            await Task.Delay(500);
            #endregion

            await DeactivateItemAsync(_loginMainViewModel, true, cancellationToken);
            await ActivateItemAsync(_splashScreenViewModel, cancellationToken);
            await Task.Delay(500);
            await _splashScreenViewModel.GetDataAsync();
            await DeactivateItemAsync(_splashScreenViewModel, true, cancellationToken);
            await ActivateItemAsync(_contentMainViewModel, cancellationToken);
        }

        public async Task HandleAsync(OnCancelMessage message, CancellationToken cancellationToken)
        {
            await TryCloseAsync();
        }

        public async Task HandleAsync(OnLogoutMessage message, CancellationToken cancellationToken)
        {
            _loginMainViewModel = IoC.Get<LoginMainViewModel>();
            await ActivateItemAsync(_loginMainViewModel, cancellationToken);
        }

    }
}
