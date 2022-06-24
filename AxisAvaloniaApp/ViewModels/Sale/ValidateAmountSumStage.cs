using Microinvest.CommonLibrary.Enums;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class ValidateAmountSumStage : SaleOperationStage
    {
        private double amountSum;
        private EPaymentTypes paymentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateAmountSumStage"/> class.
        /// </summary>
        /// <param name="amountSum">Amount to pay order.</param>
        /// <param name="paymentType">Type of payment.</param>
        public ValidateAmountSumStage(double amountSum, EPaymentTypes paymentType)
        {
            this.amountSum = amountSum;
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
                        await loggerService.ShowDialog("msgNotEnoughMoney", "strWarning", UserControls.MessageBox.EButtonIcons.Warning);
                        return Task.FromResult<object>(-1);
                    }

                    break;
                default:
                    amountSum = Math.Round(totalAmount, 3);
                    break;
            }

            return base.Invoke(amountSum);
        }
    }
}
