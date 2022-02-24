using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class ContentMainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<OnShowContentTypeMessage>
    {
        ModalViewModelBase _modalViewModel;
        public ModalViewModelBase ModalViewModel { get => _modalViewModel; set => Set(ref _modalViewModel, value); }
        public MenuViewModel MenuViewModel { get; }
        public SettingsViewModel SettingsViewModel { get; }

        private readonly IEventAggregator _eventAggregator;
        private readonly ReportViewModel _reportViewModel;

        public ContentMainViewModel(IEventAggregator eventAggregator, MenuViewModel menuViewModel, SettingsViewModel settingsViewModel, ReportViewModel cashdeskReportViewModel)
        {
            _eventAggregator = eventAggregator;
            MenuViewModel = menuViewModel;
            SettingsViewModel = settingsViewModel;
            _reportViewModel = cashdeskReportViewModel;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _eventAggregator.SubscribeOnUIThread(this);
            await Task.WhenAll(base.OnActivateAsync(cancellationToken), ActivateItemAsync(_reportViewModel, cancellationToken));
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public async Task HandleAsync(OnShowContentTypeMessage message, CancellationToken cancellationToken)
        {
            switch (message.ContentType)
            {
                case ContentType.MainContent:
                    await ActivateItemAsync(_reportViewModel, cancellationToken);
                    ModalViewModel.IsOpenPopup = false;
                    break;

                case ContentType.Settings:
                    ModalViewModel = SettingsViewModel;
                    ModalViewModel.IsOpenPopup = true;
                    break;
            }
        }

        public void CloseModal()
        {
            ModalViewModel.IsOpenPopup = false;
        }
    }
}
