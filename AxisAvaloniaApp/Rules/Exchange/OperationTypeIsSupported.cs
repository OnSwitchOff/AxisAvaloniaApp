using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Exchange
{
    public class OperationTypeIsSupported : AbstractStage
    {
        private readonly ITranslationService translationService;
        private Action<string> messageAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationTypeIsSupported"/> class.
        /// </summary>
        /// <param name="messageAction">Method to visualize non supported type of operation</param>
        public OperationTypeIsSupported(Action<string> messageAction)
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.messageAction = messageAction;
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
                case EOperTypes.Sale:
                case EOperTypes.Refund:
                    return await base.Invoke(request);
                default:
                    if (messageAction != null)
                    {
                        messageAction.Invoke(
                            string.Format(
                                translationService.Localize("msgImportErrorNonSupportedOperType"),
                                checkingData.Value.Acct,
                                checkingData.Value.OperType.ToString())
                            );
                    }
                    return await Task.FromResult<object>(-1);
            }
        }
    }
}
