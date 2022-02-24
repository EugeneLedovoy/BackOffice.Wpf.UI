using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfApp
{
    public class SettingsViewModel : ModalViewModelBase
    {
        bool _isDarkTheme;
        public bool IsDarkTheme { get => _isDarkTheme; set { Set(ref _isDarkTheme, value); App.ChangeAppTheme(value); } }

        public void OnCloseClicked()
        {
            IsOpenPopup = false;
        }
    }
}
