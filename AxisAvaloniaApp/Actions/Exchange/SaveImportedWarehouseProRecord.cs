using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Translation;
using DataBase.Entities.OperationDetails;
using DataBase.Entities.OperationHeader;
using DataBase.Repositories.Exchanges;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.PaymentTypes;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Exchange
{
    public class SaveImportedWarehouseProRecord : AbstractStage
    {
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IPaymentTypesRepository paymentTypesRepository;
        private readonly IExchangesRepository exchangesRepository;
        private readonly ITranslationService translationService;

        private List<OperationItemModel> ordersList;
        private PartnerModel partner;
        private Dictionary<ulong, int> baseOperations;
        private string appName;
        private string appKey;
        private Action<string> errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveImportedWarehouseProRecord"/> class.
        /// </summary>
        public SaveImportedWarehouseProRecord(
            IExchangesRepository exchangesRepository, 
            List<OperationItemModel> ordersList, 
            PartnerModel partner, 
            Dictionary<ulong, int> baseOperations,
            string appName,
            string appKey,
            Action<string> errorMessage)
        {
            headerRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            paymentTypesRepository = Splat.Locator.Current.GetRequiredService<IPaymentTypesRepository>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            this.exchangesRepository = exchangesRepository;
            this.ordersList = ordersList;
            this.partner = partner;
            this.baseOperations = baseOperations;
            this.appName = appName;
            this.appKey = appKey;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets number of the saved operation.
        /// </summary>
        /// <date>21.07.2022.</date>
        public long Acct
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets unique sale number.
        /// </summary>
        /// <date>21.07.2022.</date>
        public string USN { private get; set; }

        /// <summary>
        /// Sets type of payment.
        /// </summary>
        /// <date>21.07.2022.</date>
        public EPaymentTypes PaymentType { private get; set; }

        /// <summary>
        /// Sets date of payment.
        /// </summary>
        /// <date>21.07.2022.</date>
        public DateTime PaymentDate { private get; set; }

        /// <summary>
        /// Sets number of receipt.
        /// </summary>
        /// <date>21.07.2022.</date>
        public int ReceiptNumber { private get; set; }

        /// <summary>
        /// Sets type of receipt.
        /// </summary>
        /// <date>21.07.2022.</date>
        public EECCheckTypes ReceiptType { private get; set; }

        /// <summary>
        /// Sets number of the imported operation that was base for the refund.
        /// </summary>
        /// <date>21.07.2022.</date>
        public ulong? RefundSourceAcct { private get; set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">FiscalExecutionResult.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>23.06.2022.</date>
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

            if (partner == null || ordersList == null)
            {
                throw new NullReferenceException();
            }

            if (checkingData.Value.OperType == EOperTypes.Refund && RefundSourceAcct == null)
            {
                throw new NotSupportedException("RefundSourceAcct property isn't set!");
            }

            List<OperationDetail> detailsList = new List<OperationDetail>();
            Acct = await headerRepository.GetNextAcctAsync(checkingData.Value.OperType);
            OperationHeader? sourceHeader = null;
            if (checkingData.Value.OperType == EOperTypes.Refund && RefundSourceAcct != null)
            {
                sourceHeader = await headerRepository.GetOperationHeaderByIdAsync(baseOperations[(ulong)RefundSourceAcct]);
            }

            OperationHeader header = OperationHeader.Create(
                checkingData.Value.OperType,
                Acct,
                PaymentDate,
                USN,
                (DataBase.Entities.Partners.Partner)partner,
                await paymentTypesRepository.GetPaymentTypeByIndexAsync(PaymentType),
                "",
                sourceHeader,
                ReceiptType,
                ReceiptNumber,
                DateTime.Now);
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
            RefundSourceAcct = null;

            if (await headerRepository.AddNewRecordAsync(header) > 0)
            {
                // если тип текущей операции "Продажа"
                if (checkingData.Value.OperType == EOperTypes.Sale && !baseOperations.ContainsKey(checkingData.Value.Acct))
                {
                    // сохраняем новый номер документа в словаре для использования с операциями "Возврата"
                    baseOperations.Add(checkingData.Value.Acct, header.Id);
                }

                // записываем данные в таблицу обмена
                await exchangesRepository.AddNewRecordAsync(
                    header.Id,
                    DataBase.Enums.EExchangeDirections.Import,
                    appName,
                    appKey,
                    (long)checkingData.Value.Acct,
                    checkingData.Value.OperType);

                return await base.Invoke(request);
            }
            else
            {
                // иначе сообщаем пользователю о том, что текущая операция не записана в базу
                if (errorMessage != null)
                {
                    errorMessage.Invoke(
                        string.Format(
                            translationService.Localize("msgImportErrorOccuredSavingOperationData"),
                            translationService.Localize("str" + checkingData.Value.OperType.ToString()),
                            checkingData.Value.Acct));
                }

                return await Task.FromResult<object>(-1);
            }
        }
    }
}
