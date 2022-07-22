using AxisAvaloniaApp.Helpers;
using DataBase.Repositories.Exchanges;
using Microinvest.ExchangeDataService.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class DataWasNotExportedEarlier : AbstractStage
    {
        private readonly IExchangesRepository exchangesRepository;
        private EExchanges app;
        private DateTime from;
        private DateTime to;
        private long acctFrom;
        private long acctTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWasNotExportedEarlier"/> class.
        /// </summary>
        /// <param name="app">Application to exhange data.</param>
        /// <param name="from">Start date to get data from the database.</param>
        /// <param name="to">Start date to get data from the database.</param>
        /// <param name="acctFrom">Start acct to get data from the database.</param>
        /// <param name="acctTo">End acct to get data from the database.</param>
        public DataWasNotExportedEarlier(EExchanges app, DateTime from, DateTime to, long acctFrom, long acctTo)
        {
            exchangesRepository = Splat.Locator.Current.GetRequiredService<IExchangesRepository>();
            this.app = app;
            this.from = from;
            this.to = to;
            this.acctFrom = acctFrom;
            this.acctTo = acctTo;
        }

        public string AppName { get; private set; }
        public string AppKey { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>12.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            bool wasExported;
            AppName = string.Empty;
            AppKey = string.Empty;
            switch (app)
            {
                case EExchanges.ExportToNAP:
                    wasExported = await exchangesRepository.DataWasExportedToNAP(from);
                    break;
                case EExchanges.ExportToSomeApp:
                    wasExported = await exchangesRepository.DataWasExportedToUnidentifiedApp(Microinvest.CommonLibrary.Enums.EOperTypes.Sale, from, to, acctFrom, acctTo);
                    break;
                case EExchanges.ExportToWarehouseSkladPro:
                    wasExported = await exchangesRepository.DataWasExportedToWarehouseSkladPro(from, to, acctFrom, acctTo);
                    break;
                default:
                    wasExported = false;
                    break;
            }

            if ((bool)wasExported)
            {
                UserControls.MessageBoxes.EButtonResults result = await loggerService.ShowDialog("msgDataWasExported", "strAttention", UserControls.MessageBoxes.EButtonIcons.Info, UserControls.MessageBoxes.EButtons.YesNoCancel);
                switch (result)
                {
                    case UserControls.MessageBoxes.EButtonResults.Yes:
                        return await base.Invoke(request);
                    case UserControls.MessageBoxes.EButtonResults.No:
                        AppName = app.ToString();
                        return await base.Invoke(request);
                    default:
                        return await Task.FromResult<object>(-1);
                }
            }
            else
            {
                return await base.Invoke(request);
            }
        }
    }
}
