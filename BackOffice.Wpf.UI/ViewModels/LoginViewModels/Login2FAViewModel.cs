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
    public class Login2FAViewModel : Screen
    {
        string _code;
        public string Code { get => _code; set { Set(ref _code, value); NotifyOfPropertyChange(() => CanOnOkClicked); } }

        bool _isCodeFocused;
        public bool IsCodeFocused { get => _isCodeFocused; set => Set(ref _isCodeFocused, value); }

        public bool CanOnOkClicked => !string.IsNullOrWhiteSpace(Code);

        string _errorMessage;
        public string ErrorMessage { get => _errorMessage; set => Set(ref _errorMessage, value); }

        private readonly IEventAggregator _eventAggregator;

        public Login2FAViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void OnOkClicked()
        {
            var isOk = Code == "123";
            if (!isOk)
            {
                ErrorMessage = "Incorrect code, please, try again";
                Code = null;
                if (!IsCodeFocused)
                    IsCodeFocused = true;

                return;
            }

            _eventAggregator.PublishOnUIThreadAsync(new On2FAMessage(isOk, isOk ? "OK" : ErrorMessage));
            Code = ErrorMessage = null;
        }

        public void OnCancelClicked()
        {
            _eventAggregator.PublishOnUIThreadAsync(new OnBackMessage());
            Code = null;
        }

        public void OnKeyPressed(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CanOnOkClicked)
                OnOkClicked();

            else if (e.Key == Key.Escape)
                OnCancelClicked();
        }

    }
}
