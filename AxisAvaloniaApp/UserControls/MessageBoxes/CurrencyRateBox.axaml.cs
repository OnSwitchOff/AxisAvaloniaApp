using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.UserControls.MessageBoxes
{
    public partial class CurrencyRateBox : Window
    {
        private readonly ISettingsService settingsService;
        private double rate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyRateBox"/> class.
        /// </summary>
        public CurrencyRateBox()
        {
            InitializeComponent();

            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public static readonly StyledProperty<EButtonResults> ResultProperty =
            AvaloniaProperty.Register<CurrencyRateBox, EButtonResults>(nameof(Result), EButtonResults.None);

        /// <summary>
        /// Gets or sets choice of user.
        /// </summary>
        /// <date>20.07.2022.</date>
        public EButtonResults Result
        {
            get => GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }

        public static readonly StyledProperty<string> Exchangeable—urrenciesProperty =
           AvaloniaProperty.Register<CurrencyRateBox, string>(nameof(Exchangeable—urrencies));

        /// <summary>
        ///Gets or sets value to convert from which currency to which currency we need the exchange rate.
        /// </summary>
        /// <date>20.07.2022.</date>
        public string Exchangeable—urrencies
        {
            get => GetValue(Exchangeable—urrenciesProperty);
            set => SetValue(Exchangeable—urrenciesProperty, value);
        }

        public static readonly StyledProperty<string> RateProperty =
            AvaloniaProperty.Register<CurrencyRateBox, string>(nameof(Rate));

        /// <summary>
        /// Gets or sets exchange rate of currency.
        /// </summary>
        /// <date>20.07.2022.</date>
        public string Rate
        {
            get => GetValue(RateProperty);
            set => SetValue(RateProperty, value);
        }

        public static readonly StyledProperty<bool> RateIsValidProperty =
            AvaloniaProperty.Register<CurrencyRateBox, bool>(nameof(RateIsValid), false);

        /// <summary>
        /// Gets or sets value indicating whether rate of currency is valid.
        /// </summary>
        /// <date>20.07.2022.</date>
        public bool RateIsValid
        {
            get => GetValue(RateIsValidProperty);
            set => SetValue(RateIsValidProperty, value);
        }

        /// <summary>
        /// Sets Result and closes window when button was pressed.
        /// </summary>
        /// <param name="buttonResult">Result of button pressed.</param>
        /// <date>20.07.2022.</date>
        public void ButtonClick(EButtonResults buttonResult)
        {
            Result = buttonResult;
            this.Close();
        }

        /// <summary>
        /// Parsed exchange rate when Rate is changing.
        /// </summary>
        /// <typeparam name="T">Type of changed property.</typeparam>
        /// <param name="change">Property that was changed.</param>
        /// <date>20.07.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(Rate):
                    RateIsValid = double.TryParse(
                        Rate.
                        Replace(",", settingsService.Culture.NumberFormat.NumberDecimalSeparator).
                        Replace(".", settingsService.Culture.NumberFormat.NumberDecimalSeparator), 
                        out rate) && 
                        rate > 0;
                    break;
            }
        }

        /// <summary>
        /// Shows window and returns result of button pressed and exchange rate.
        /// </summary>
        /// <returns>returns result of button pressed and exchange rate.</returns>
        /// <date>20.07.2022.</date>
        public async Task<(EButtonResults result, double rate)> ShowDialog()
        {
            await this.ShowDialog(App.MainWindow);

            return (this.Result, this.rate);
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        /// <date>20.07.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
