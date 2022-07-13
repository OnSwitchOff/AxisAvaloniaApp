using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Entities.OperationDetails;
using DataBase.Entities.OperationHeader;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.PaymentTypes;
using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Sale
{
    public class SaveOrder : AbstractStage
    {
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IPaymentTypesRepository paymentTypesRepository;

        private string uSN;
        private ObservableCollection<OperationItemModel> ordersList;
        private PartnerModel partner;
        private EPaymentTypes paymentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentOrder"/> class.
        /// </summary>
        /// <param name="uSN">Unique sale number.</param>
        /// <param name="order">Orders list.</param>
        /// <param name="partner">Partner that is selected for the operation.</param>
        /// <param name="paymentType">Type of payment of the operation.</param>
        public SaveOrder(string uSN, ObservableCollection<OperationItemModel> order, PartnerModel partner, EPaymentTypes paymentType)
        {
            headerRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            paymentTypesRepository = Splat.Locator.Current.GetRequiredService<IPaymentTypesRepository>();

            this.uSN = uSN;
            ordersList = order;
            this.partner = partner;
            this.paymentType = paymentType;
        }

        /// <summary>
        /// Gets number of the current operation.
        /// </summary>
        /// <date>24.06.2022.</date>
        public long Acct 
        { 
            get; 
            private set;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">FiscalExecutionResult.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>23.06.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            FiscalExecutionResult? result = request as FiscalExecutionResult;
            if (result == null)
            {
                throw new System.InvalidCastException();
            }

            List<OperationDetail> detailsList = new List<OperationDetail>();
            Acct = await headerRepository.GetNextAcctAsync(EOperTypes.Sale);
            OperationHeader header = OperationHeader.Create(
                EOperTypes.Sale,
                Acct,
                result.DocumentDateTime,
                uSN,
                (DataBase.Entities.Partners.Partner)partner,
                await paymentTypesRepository.GetPaymentTypeByIndexAsync(paymentType),
                "",
                result.ReceiptType.Convert(),
                result.ReceiptNumber);
            foreach (OperationItemModel itemModel in ordersList)
            {
                if (itemModel.Item.Id > 0)
                {
                    detailsList.Add(OperationDetail.Create(
                        header,
                        (DataBase.Entities.Items.Item)itemModel.Item,
                        (decimal)itemModel.Qty * itemModel.Multiplier,
                        -1,
                        (decimal)itemModel.Price,
                        (decimal)itemModel.VATValue,
                        itemModel.Note));
                }
            }
            header.OperationDetails.AddRange(detailsList);

            while (true)
            {
                if (await headerRepository.AddNewRecordAsync(header) > 0)
                {
                    return await base.Invoke(header);
                }
                else
                {
                    switch (await loggerService.ShowDialog("msgErrorDuringSavingOperation", "strError", UserControls.MessageBox.EButtonIcons.Error, UserControls.MessageBox.EButtons.AbortRetryIgnore))
                    {
                        case UserControls.MessageBox.EButtonResults.Abort:
                            return await Task.FromResult<object>(-1);
                        case UserControls.MessageBox.EButtonResults.Retry:
                            break;
                        case UserControls.MessageBox.EButtonResults.Ignore:
                            loggerService.RegisterError(DataBase.Enums.EApplicationLogEvents.SaveOperation, "Writing data to database is ignored by user!");
                            return await base.Invoke(header);
                    }
                }
            }
        }
    }
}
