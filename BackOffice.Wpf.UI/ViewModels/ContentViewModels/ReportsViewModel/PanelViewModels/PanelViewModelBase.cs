using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class PanelViewModelBase : Screen
    {
        BindableCollection<object> _controlCollection = new();
        public BindableCollection<object> ControlCollection { get => _controlCollection; set => Set(ref _controlCollection, value); }
    }
}
