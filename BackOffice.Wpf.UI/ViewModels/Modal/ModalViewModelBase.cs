using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class ModalViewModelBase : Screen
    {
        bool _isOpenPopup;
        public bool IsOpenPopup { get => _isOpenPopup; set => Set(ref _isOpenPopup, value); }

        bool _isCloseWhenUnfocused;
        public bool IsCloseWhenUnfocused { get => _isCloseWhenUnfocused; set => Set(ref _isCloseWhenUnfocused, value); }

    }
}
