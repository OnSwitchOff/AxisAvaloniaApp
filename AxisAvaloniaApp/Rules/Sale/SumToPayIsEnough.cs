using Microinvest.CommonLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Sale
{
    public class SumToPayIsEnough : AbstractStage
    {
        private double amountSum;
        private EPaymentTypes paymentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SumToPayIsEnough"/> class.
        /// </summary>
        /// <param name="amountSum">Amount to pay order.</param>
        /// <param name="paymentType">Type of payment.</param>
        public SumToPayIsEnough(double amountSum, EPaymentTypes paymentType)
        {
            this.amountSum = amountSum;
            this.paymentType = paymentType;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Total amount of the orders list.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>23.06.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            double totalAmount;
            if (request == null || !double.TryParse(request.ToString(), out totalAmount))
            {
                throw new InvalidCastException();
            }

            switch (paymentType)
            {
                case EPaymentTypes.Cash:
                    if (Math.Round(totalAmount, 3) > Math.Round(amountSum, 3))
                    {
                        await loggerService.ShowDialog("msgNotEnoughMoney", "strWarning", UserControls.MessageBoxes.EButtonIcons.Warning);
                        return await Task.FromResult<object>(-1);
                    }

                    break;
                default:
                    amountSum = Math.Round(totalAmount, 3);
                    break;
            }

            return await base.Invoke(amountSum);
        }
    }
}
