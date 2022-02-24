using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyWpfApp
{
    public class LoginCredentialsViewModel : Screen
    {
        string _login;
        public string Login { get => _login; set { Set(ref _login, value); NotifyOfPropertyChange(() => CanOnOkClicked); } }

        string _password;
        public string Password { get => _password; set { Set(ref _password, value); NotifyOfPropertyChange(() => CanOnOkClicked); } }

        bool _isLoginFocused;
        public bool IsLoginFocused { get => _isLoginFocused; set => Set(ref _isLoginFocused, value); }

        bool _isPasswordFocused;
        public bool IsPasswordFocused { get => _isPasswordFocused; set => Set(ref _isPasswordFocused, value); }

        public bool CanOnOkClicked => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrEmpty(Password) && !IsOkBusy;

        string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set => Set(ref _errorMessage, value); }

        bool _isOkBusy;
        public bool IsOkBusy { get => _isOkBusy; set { Set(ref _isOkBusy, value); NotifyOfPropertyChange(() => CanOnOkClicked); } }

        private readonly IEventAggregator _eventAggregator;

        public LoginCredentialsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public async Task OnOkClicked()
        {
            if (IsOkBusy)
                return;

            IsOkBusy = true;
            await Task.Delay(2000);
            IsOkBusy = false;

            var isOk = Password == "111";
            if (!isOk)
            {
                ErrorMessage = "Incorrect password, please, try again";
                Password = null;
                if (!IsPasswordFocused)
                    IsPasswordFocused = true;

                return;
            }

            await _eventAggregator.PublishOnUIThreadAsync(new OnAuthenticationMessage(isOk, isOk ? "OK": ErrorMessage));
            Password = ErrorMessage = null;
        }

        public void OnCancelClicked()
        {
            _eventAggregator.PublishOnUIThreadAsync(new OnCancelMessage());
        }

        public async void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CanOnOkClicked)
                await OnOkClicked();

            else if (e.Key == Key.Escape)
                OnCancelClicked();
        }
    }
}
