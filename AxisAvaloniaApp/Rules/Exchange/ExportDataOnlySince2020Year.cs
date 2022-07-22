using Microinvest.ExchangeDataService.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class ExportDataOnlySince2020Year : AbstractStage
    {
        private EExchanges app;
        private DateTime date;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDataOnlySince2020Year"/> class.
        /// </summary>
        /// <param name="app">Application to exhange data.</param>
        /// <param name="date">Date to prepare data for export.</param>
        public ExportDataOnlySince2020Year(EExchanges app, DateTime date)
        {
            this.app = app;
            this.date = date;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>18.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (app == EExchanges.ExportToNAP && date.Year < 2020)
            {
                await loggerService.ShowDialog("msgIsSupportedOnlyFrom2020", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
