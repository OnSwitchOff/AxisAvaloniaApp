using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Settings;
using ReactiveUI;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public abstract class SettingsViewModelBase : ViewModelBase, IVisible
    {
        protected readonly ISettingsService settingsService;
        protected readonly ILoggerService loggerService;
        private bool isVisible;
        private double titleMinWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModelBase"/> class.
        /// </summary>
        public SettingsViewModelBase()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();

            TitleMinWidth = 0;
        }

        /// <summary>
        /// Gets or sets value indicating whether the UserControl is visible.
        /// </summary>
        /// <date>09.06.2022.</date>
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        /// <summary>
        /// Gets or sets min width of title.
        /// </summary>
        /// <date>09.06.2022.</date>
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }
    }
}
