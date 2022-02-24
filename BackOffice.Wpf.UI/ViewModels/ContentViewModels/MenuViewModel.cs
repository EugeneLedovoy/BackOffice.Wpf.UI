using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class MenuViewModel
    {
        readonly IEventAggregator _eventAggregator;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void OnLogoutClicked()
        {
            _eventAggregator.PublishOnUIThreadAsync(new OnLogoutMessage()); 
        }

        public void OnSettingsClicked()
        {
            _eventAggregator.PublishOnUIThreadAsync(new OnShowContentTypeMessage(ContentType.Settings));
        }
    }
}
