using Microinvest.ExchangeDataService.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class FullMonthForNAP : AbstractStage
    {
        private EExchanges app;
        private DateTime from;
        private DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullMonthForNAP"/> class.
        /// </summary>
        /// <param name="app">Application to exhange data.</param>
        /// <param name="from">Start date to get data from the database.</param>
        /// <param name="to">Start date to get data from the database.</param>
        public FullMonthForNAP(EExchanges app, DateTime from, DateTime to)
        {
            this.app = app;
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>12.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (app == EExchanges.ExportToNAP && (from.Month - to.Month) > 0)
            {
                await loggerService.ShowDialog("msgSelectOneFullMonth", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
