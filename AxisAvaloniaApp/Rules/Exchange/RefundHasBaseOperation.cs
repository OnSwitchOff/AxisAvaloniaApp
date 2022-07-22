using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class RefundHasBaseOperation : AbstractStage
    {
        private readonly ITranslationService translationService;
        private Action<string> messageAction;
        private Dictionary<ulong, int> baseOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefundHasBaseOperation"/> class.
        /// </summary>
        /// <param name="messageAction">Method to visualize non supported type of operation</param>
        /// <param name="baseOperations">List of the imported base operations.</param>
        public RefundHasBaseOperation(Action<string> messageAction, Dictionary<ulong, int> baseOperations)
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.messageAction = messageAction;
            this.baseOperations = baseOperations;
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

            switch (checkingData.Value.OperType)
            {
                case EOperTypes.Refund:
                    // если для возврата нет подходящей операции "Продажа"
                    if (!baseOperations.ContainsKey(checkingData.Value.Acct))
                    {
                        // сообщаем пользователю о невозможности сохранить текущую операцию и переходим к следующей операции
                        if (messageAction != null)
                        {
                            messageAction.Invoke(
                                string.Format(
                                    translationService.Localize("msgImportError_RefundNoCorrespondingToSale"),
                                    checkingData.Value.Acct));
                        }

                        return await Task.FromResult<object>(-1);
                    }
                    else
                    {
                        return await base.Invoke(request);
                    }
                default:
                    return await base.Invoke(request);
            }
        }
    }
}
