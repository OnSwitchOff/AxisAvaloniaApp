using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Document;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using DataBase.Entities.OperationHeader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Sale
{
    public class CreateReceipt : AbstractStage
    {
        private readonly ISettingsService settingsService;
        private readonly ITranslationService translationService;
        private ObservableCollection<OperationItemModel> ordersList;
        private readonly IDocumentService documentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateReceipt"/> class.
        /// </summary>
        /// <param name="order">Orders list.</param>
        /// <param name="documentService">Service to create receipt.</param>
        public CreateReceipt(ObservableCollection<OperationItemModel> order, IDocumentService documentService)
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            this.ordersList = order;
            this.documentService = documentService;
            Pages = new List<Image>();
        }

        /// <summary>
        /// Gets list with images to show receipt.
        /// </summary>
        /// <date>06.07.2022.</date>
        public List<Image> Pages { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">FiscalExecutionResult.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>23.06.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            OperationHeader? operationData = request as OperationHeader;
            if (operationData == null)
            {
                throw new InvalidCastException();
            }

            // заполняем данные о покупателе
            documentService.CustomerData = operationData.Partner;

            // заполняем описание документа
            documentService.DocumentDescription.DocumentName = translationService.Localize("strStockReceipt");
            documentService.DocumentDescription.DocumentDescription = translationService.Localize("strForSale");
            documentService.DocumentDescription.DocumentNumber = operationData.Acct.ToString("D10");
            documentService.DocumentDescription.DocumentDate = DateTime.Now;
            documentService.DocumentDescription.PaymentType = operationData.Payment.Name;
            documentService.DocumentDescription.DealReason = translationService.Localize("strSale");
            documentService.DocumentDescription.DocumentSum = settingsService.AppLanguage.MoneyByWords(
                (double)operationData.OperationDetails.Sum(d => d.Qtty * d.SalePrice), 
                settingsService.Country.CurrencyCode.Convert());
            documentService.DocumentDescription.ReceivedBy = operationData.Partner.Principal;
            documentService.DocumentDescription.CreatedBy = settingsService.AppSettings[ESettingKeys.Principal];

            // размечаем таблицу с товарами
            documentService.ItemsData = new System.Data.DataTable();
            documentService.ItemsData.Columns.Add(translationService.Localize("strRowNumber"), typeof(int));
            documentService.ItemsData.Columns.Add(translationService.Localize("strCode"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strGoods"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strMeasure"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strQtty"), typeof(double));
            documentService.ItemsData.Columns.Add(translationService.Localize("strPrice"), typeof(double));
            documentService.ItemsData.Columns.Add(translationService.Localize("strDiscount"), typeof(double));
            documentService.ItemsData.Columns.Add(translationService.Localize("strAmount_Short"), typeof(double));

            // заполняем таблицу с товарами данными
            documentService.ClearVATList();
            foreach (var item in ordersList)
            {
                if (item.Item.Id > 0)
                {
                    documentService.ItemsData.Rows.Add(
                        item.RecordId,
                        item.Item.Code,
                        item.Item.Name,
                        item.SelectedMeasure.Measure,
                        item.Qty,
                        item.Price,
                        item.Discount,
                        item.Amount);

                    Microinvest.PDFCreator.Models.VATModel vAT = new Microinvest.PDFCreator.Models.VATModel()
                    {
                        VATRate = (float)item.Item.VATGroup.Value,
                        VATSum = Math.Round(item.VATValue * item.Qty, 2),
                        VATBase = Math.Round((item.Price - item.VATValue) * item.Qty, 2),
                    };

                    documentService.AddNewVATRecord(vAT);
                }
            }

            // генерируем документ
            if (documentService.GenerateReceipt(EDocumentVersionsPrinting.Original, operationData.Payment.PaymentIndex))
            {
                Pages = documentService.ConvertDocumentToImageList();
            }
            else
            {
                await loggerService.ShowDialog("msgErrorDuringReceiptGeneration", "strWarning", UserControls.MessageBox.EButtonIcons.Warning);
            }

            return await base.Invoke(request);
        }
    }
}
