using AxisAvaloniaApp.Services.Settings;
using Microinvest.ExchangeDataService.Enums;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class DomainNameIsNotEmpty : AbstractStage
    {
        private readonly ISettingsService settingsServise;
        private EExchanges app;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainNameIsNotEmpty"/> class.
        /// </summary>
        /// <param name="settingsServise">Service to get settings of the app.</param>
        /// <param name="app">Application to exhange data.</param>
        public DomainNameIsNotEmpty(ISettingsService settingsServise, EExchanges app)
        {
            this.settingsServise = settingsServise;
            this.app = app;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>18.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (app == EExchanges.ExportToNAP && string.IsNullOrEmpty(settingsServise.AppSettings[Enums.ESettingKeys.OnlineShopDomainName]))
            {
                await loggerService.ShowDialog("msgDomainNameHasNotBeenSet", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
