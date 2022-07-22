using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using DataBase.Repositories.Exchanges;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class DataWasNotImportedEarlier : AbstractStage
    {
        private readonly ITranslationService translationService;
        private readonly IExchangesRepository exchangesRepository;
        private Action<string> messageAction;
        private Dictionary<ulong, int> baseOperations;
        private string appName;
        private string appKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWasNotImportedEarlier"/> class.
        /// </summary>
        /// <param name="messageAction">Method to visualize non supported type of operation</param>
        /// <param name="baseOperations">List to set imported base operations.</param>
        public DataWasNotImportedEarlier(Action<string> messageAction, Dictionary<ulong, int> baseOperations, string appName, string appKey)
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            exchangesRepository = Splat.Locator.Current.GetRequiredService<IExchangesRepository>();
            this.messageAction = messageAction;
            this.baseOperations = baseOperations;
            this.appName = appName;
            this.appKey = appKey;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>21.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            var checkingData = request as (EOperTypes OperType, ulong Acct)?;
            if (checkingData == null || !checkingData.HasValue)
            {
                throw new ArgumentException();
            }

            var result = await exchangesRepository.GetImportedData(appName, appKey, (long)checkingData.Value.Acct, checkingData.Value.OperType);

            if (result.OperationHeaderId > 0)
            {
                if (messageAction != null)
                {
                    messageAction.Invoke(
                        string.Format(
                            translationService.Localize("msgImportDataIsExisted"),
                            translationService.Localize("str" + checkingData.Value.OperType.ToString()),
                            checkingData.Value.Acct));
                }

                if (checkingData.Value.OperType == EOperTypes.Sale)
                {
                    baseOperations.Add(checkingData.Value.Acct, result.OperationHeaderId);
                }

                return await Task.FromResult<object>(-1);
            }
            else
            {
                return await base.Invoke(request);
            }
        }
    }
}
