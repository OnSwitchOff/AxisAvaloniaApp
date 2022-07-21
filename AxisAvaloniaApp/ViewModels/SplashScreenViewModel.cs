using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.StartUp;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class SplashScreenViewModel : ReactiveObject
    {
        private readonly IStartUpService startUpService;
        private int progress;
        private string message;

        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }

        public string Message 
        { 
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

        public SplashScreenViewModel()
        {
            startUpService = Locator.Current.GetRequiredService<IStartUpService>();
            StarupLoading();
        }

        private void StartLoadingTask()
        {
            Task.Run(() => StarupLoading());
        }

        private void StarupLoading()
        {
            startUpService.ProgressChanged += StartUpService_ProgressChanged;
            startUpService.ActivateAsync(!Configurations.AppConfiguration.IsDatabaseExist);
        }

        private void StartUpService_ProgressChanged(int prog, string msg)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Progress = prog;
                Message = msg;
            });
 
        }
    }
}
