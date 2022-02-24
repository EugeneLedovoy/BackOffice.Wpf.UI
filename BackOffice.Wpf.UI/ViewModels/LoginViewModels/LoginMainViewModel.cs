using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class LoginMainViewModel : Conductor<Screen>.Collection.OneActive, IViewModelChanging, IHandle<OnAuthenticationMessage>, IHandle<OnBackMessage>, IHandle<On2FAMessage>
    {
        public event EventHandler ViewModelChanging;

        private readonly IEventAggregator _eventAggregator;
        private readonly LoginCredentialsViewModel _loginCredentialsViewModel;
        public Login2FAViewModel Login2FAViewModel { get; private set; }

        public LoginMainViewModel(IEventAggregator eventAggregator, LoginCredentialsViewModel loginCredentialsViewModel/*, Login2FAViewModel login2FAViewModel*/)
        {
            _eventAggregator = eventAggregator;
            _loginCredentialsViewModel = loginCredentialsViewModel;
            //_login2FAViewModel = login2FAViewModel;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            _eventAggregator.SubscribeOnPublishedThread(this);
            _loginCredentialsViewModel.IsLoginFocused = true;
            ActivateItemAsync(_loginCredentialsViewModel);
        }

        //protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        //{
        //    _eventAggregator.SubscribeOnPublishedThread(this);
        //    await Task.WhenAll(base.OnActivateAsync(cancellationToken), ActivateItemAsync(_loginCredentialsViewModel, cancellationToken));
        //}

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            await base.OnDeactivateAsync(close, cancellationToken);
        }

        public async Task HandleAsync(OnAuthenticationMessage message, CancellationToken cancellationToken)
        {
            Login2FAViewModel = IoC.Get<Login2FAViewModel>();
            await ActivateItemAsync(Login2FAViewModel, cancellationToken);
        }

        public async Task HandleAsync(OnBackMessage message, CancellationToken cancellationToken)
        {
            _loginCredentialsViewModel.IsLoginFocused = true;
            await ActivateItemAsync(_loginCredentialsViewModel, cancellationToken);
        }

        public async Task HandleAsync(On2FAMessage message, CancellationToken cancellationToken)
        {
            #region for animating out the content of the Login2FAView
            ViewModelChanging?.Invoke(this, EventArgs.Empty);
            await Task.Delay(500);
            #endregion

            _loginCredentialsViewModel.Login = null;
            await ActivateItemAsync(_loginCredentialsViewModel, cancellationToken);
        }
    }

    public interface IViewModelChanging
    {
        event EventHandler ViewModelChanging;
    }
}
