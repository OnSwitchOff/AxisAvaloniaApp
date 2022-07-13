using AxisAvaloniaApp.Helpers;
using DataBase.Repositories.Exchanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class DataWasImportedEarlier : AbstractStage
    {
        private readonly IExchangesRepository exchangesRepository;
        private string appName;
        private string appKey;
        private long acct;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWasImportedEarlier"/> class.
        /// </summary>
        /// <param name="app">Application to exhange data.</param>
        /// <param name="from">Start date to get data from the database.</param>
        /// <param name="to">Start date to get data from the database.</param>
        /// <param name="acctFrom">Start acct to get data from the database.</param>
        /// <param name="acctTo">End acct to get data from the database.</param>
        public DataWasImportedEarlier(string appName, string appKey, long acct)
        {
            exchangesRepository = Splat.Locator.Current.GetRequiredService<IExchangesRepository>();
            this.appName = appName;
            this.appKey = appKey;
            this.acct = acct;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>12.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            bool wasExported = false;
            //switch (app)
            //{
            //    case EExchanges.ExportToNAP:
            //        wasExported = await exchangesRepository.DataWasExportedToNAP(from);
            //        break;
            //    case EExchanges.ExportToSomeApp:
            //        wasExported = await exchangesRepository.DataWasExportedToUnidentifiedApp(Microinvest.CommonLibrary.Enums.EOperTypes.Sale, from, to, acctFrom, acctTo);
            //        break;
            //    case EExchanges.ExportToWarehouseSkladPro:
            //        wasExported = await exchangesRepository.DataWasExportedToWarehouseSkladPro(from, to, acctFrom, acctTo);
            //        break;
            //}

            if (wasExported)
            {
                if (await loggerService.ShowDialog("msgDataWasExported", "strAttention", UserControls.MessageBox.EButtonIcons.Info, UserControls.MessageBox.EButtons.YesNo) == UserControls.MessageBox.EButtonResults.Yes)
                {
                    return await base.Invoke(request);
                }
                else
                {
                    return await Task.FromResult<object>(-1);
                }
            }

            return await base.Invoke(request);
        }
    }
}
