using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using AxisAvaloniaApp.Services.Settings;
using DataBase.Repositories.Documents;
using DataBase.Repositories.Exchanges;
using DataBase.Repositories.OperationHeader;
using Microinvest.ExchangeDataService.Enums;
using Microinvest.ExchangeDataService.Models.Auditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.ExchangeService
{
    public class ExchangeService : IExchangeService
    {
        private readonly ISettingsService settingsService;
        private readonly IOperationHeaderRepository operationHeaderRepository;
        private readonly IDocumentsRepository documentsRepository;
        private readonly IExchangesRepository exchangesRepository;
        private readonly ISearchData searchService;


        /// <summary>
        /// Gets data from the database and prepares it for export
        /// </summary>
        /// <param name="app">An application to which data will be exported.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <returns>Returns String.Empty if data for export is absent; otherwise returns string with data for export.</returns>
        /// <date>13.07.2022.</date>
        public async Task<string> GetDataForExportAsync(EExchanges app, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo)
        {
            return await Task.Run(async () =>
            {
                switch (app)
                {
                    case EExchanges.ExportToNAP:
                        AuditorModel auditor = new AuditorModel()
                        {
                            TaxNumber = settingsService.AppSettings[ESettingKeys.TaxNumber],
                            UniqueShopNumber = settingsService.AppSettings[ESettingKeys.OnlineShopNumber],
                            ShopType = (Microinvest.CommonLibrary.Enums.EOnlineShopTypes)settingsService.AppSettings[ESettingKeys.OnlineShopType],
                            DomainName = settingsService.AppSettings[ESettingKeys.OnlineShopDomainName],
                            CreationDate = DateTime.Now,
                            Month = (ulong)dateFrom.Month,
                            Year = (ulong)dateFrom.Year,
                        };
                        Operation operation;
                        DataBase.Entities.Documents.Document? document;
                        Goods nAPItem;
                        decimal amount;
                        decimal vATAmount;
                        decimal totalAmount;

                        List<DataBase.Entities.OperationHeader.OperationHeader> sales = await operationHeaderRepository.GetSalesAndRefunds(dateFrom.Year, dateFrom.Month, acctFrom, acctTo);
                        foreach (var sale in sales)
                        {
                            operation = new Operation();
                            operation.Acct = (ulong)sale.Acct;
                            operation.AcctDate = sale.Date;
                            document = await documentsRepository.GetDocumentsByOperationHeaderAsync(sale, Microinvest.CommonLibrary.Enums.EDocumentTypes.Invoice);
                            if (document != null)
                            {
                                operation.DocumentNumber = ulong.Parse(document.DocumentNumber);
                                operation.DocumentDate = document.DocumentDate;
                            }

                            amount = 0;
                            vATAmount = 0;
                            totalAmount = 0;
                            foreach (var item in sale.OperationDetails)
                            {
                                nAPItem = new Goods()
                                {
                                    Name = item.Goods.Name,
                                    Quantity = item.Qtty,
                                    Price = item.SalePrice - item.SaleVAT,
                                    VATRate = (uint)item.Goods.Vatgroup.VATValue,
                                    TotalVATValue = item.Qtty * item.SaleVAT,
                                    Sum = item.Qtty * item.SalePrice,
                                };

                                amount += nAPItem.Price * nAPItem.Quantity;
                                vATAmount += nAPItem.TotalVATValue;
                                totalAmount += nAPItem.Sum;
                                operation.Products.Add(nAPItem);
                            }

                            operation.Sum = amount;
                            operation.Discount = 0;
                            operation.VATSum = vATAmount;
                            operation.TotalSum = totalAmount;
                            operation.PaymentMethod = sale.Payment.PaymentIndex;
                            auditor.Operations.Add(operation);
                        }

                        break;
                }

                return string.Empty;
            });
        }
    }
}
