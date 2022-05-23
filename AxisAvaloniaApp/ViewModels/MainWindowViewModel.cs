using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Navigation;
using AxisAvaloniaApp.UserControls.NavigationView;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly INavigationService navigationService;
        private NavigationViewItem selectedItem;
        private ObservableCollection<INavigationViewItem> activaSales;
        private INavigationViewItem selectedSale;
        private IControl content;
        private string explanation;
        private string licenseData = "Some license data!";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            navigationService = Splat.Locator.Current.GetRequiredService<INavigationService>();
            GoToYouTubeCommand = ReactiveCommand.Create(GoToYouTube);
        }

        private async void GoToYouTube()
        {
            string URL = "https://www.youtube.com/channel/UCgHHhDj1hskaBTWwQU4rRWQ/playlists";
            if (Uri.IsWellFormedUriString(URL, UriKind.RelativeOrAbsolute))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = URL,
                    UseShellExecute = true,
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public NavigationViewItem SelectedItem
        {
            get => selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);

                Content = navigationService.NavigateTo(
                    selectedItem.GetValue(NavigationExtensions.NavigateToProperty),
                    Content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public ObservableCollection<INavigationViewItem> ActivaSales
        {
            get => activaSales;
            set => this.RaiseAndSetIfChanged(ref activaSales, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public INavigationViewItem SelectedSale
        {
            get => selectedSale;
            set => this.RaiseAndSetIfChanged(ref selectedSale, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public IControl Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public string Explanation
        {
            get => explanation;
            set => this.RaiseAndSetIfChanged(ref explanation, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public IReactiveCommand GoToYouTubeCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public IObservable<string> AppDateTime { get; } = Observable.Timer(DateTimeOffset.Now, new TimeSpan(0, 0, 0, 1)).Select(_ => System.DateTime.Now.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <date>23.05.2022.</date>
        public string LicenseData
        {
            get => licenseData;
            set => this.RaiseAndSetIfChanged(ref licenseData, value);
        }
    }
}
