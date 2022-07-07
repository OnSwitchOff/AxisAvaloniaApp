using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.MessageBox;
using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService.Exceptions;
using Microinvest.DeviceService.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Sale
{
    public class PaymentOrder : AbstractStage
    {
        private readonly IPaymentService paymentService;
        private readonly ITranslationService translationService;

        private ObservableCollection<OperationItemModel> orderList;
        private EPaymentTypes paymentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentOrder"/> class.
        /// </summary>
        /// <param name="orderList">Orders list.</param>
        /// <param name="paymentType">Type of payment.</param>
        public PaymentOrder(ObservableCollection<OperationItemModel> orderList, EPaymentTypes paymentType)
        {
            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            this.orderList = orderList;
            this.paymentType = paymentType;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Total amount of the orders list.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>23.06.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            decimal amountToPay;
            if (request == null || !decimal.TryParse(request.ToString(), out amountToPay))
            {
                throw new InvalidCastException();
            }

            while (true)
            {
                FiscalExecutionResult fiscalResult = await paymentService.FiscalDevice.PayOrderAsync(orderList, paymentType, amountToPay);

                if (fiscalResult.ResultException == null)
                {
                    return await base.Invoke(fiscalResult);

                }
                else
                {
                    string errorDescription = string.Empty;
                    string errorTitle = string.Empty;

                    switch (fiscalResult.ResultException)
                    {
                        case DeviceExtendedException devEx:
                            errorDescription = devEx.Message;
                            errorTitle = devEx.Section;
                            break;
                        default:
                            errorDescription = fiscalResult.ResultException.Message;
                            errorTitle = translationService.Localize("strError");
                            break;
                    }

                    switch (await loggerService.ShowDialog1(errorDescription, errorTitle, EButtonIcons.Error, EButtons.AbortRetryIgnore))
                    {
                        case EButtonResults.Abort:
                            return await Task.FromResult<object>(-1);
                        case EButtonResults.Retry:
                            break;
                        case EButtonResults.Ignore:
                            loggerService.RegisterError(DataBase.Enums.EApplicationLogEvents.PrintReceipt, "Printing of receipt is ignored by user!");
                            return await base.Invoke(fiscalResult);
                    }
                }
            }
        }
    }
}
