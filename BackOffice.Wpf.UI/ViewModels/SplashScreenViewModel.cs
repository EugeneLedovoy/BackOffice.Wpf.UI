using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfApp
{
    public class SplashScreenViewModel : Screen, IViewModelChanging
    {
        public event EventHandler ViewModelChanging;

        public string Version => "beta version";

        private string _statusMessage;
        public string StatusMessage { get => _statusMessage; set => Set(ref _statusMessage, value); }

        private int _progressPercent;
        public int ProgressPercent { get => _progressPercent; set => Set(ref _progressPercent, value); }

        bool _isBusy;
        public bool IsBusy { get => _isBusy; set => Set(ref _isBusy, value); }

        public async Task GetDataAsync()
        {
            IsBusy = true;
            await ShowProgress();
            await Task.Delay(200);
            IsBusy = false;
            ProgressPercent = 0;


            ViewModelChanging?.Invoke(this, EventArgs.Empty);
            await Task.Delay(500);
        }

        private async Task ShowProgress()
        {
            StatusMessage = "Something is going on for a while....";

            while (ProgressPercent < 100)
            {
                await Task.Delay(500);
                ProgressPercent += 10;
            }
        }
    }
}
