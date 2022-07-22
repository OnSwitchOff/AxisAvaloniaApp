using DataBase.Entities.Exchanges;
using DataBase.Enums;
using Microinvest.CommonLibrary.Enums;
using Microinvest.ExchangeDataService.Enums;
using Microinvest.ExchangeDataService.Models.Auditor;
using Microinvest.ExchangeDataService.Models.DeltaPro;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.Exchanges
{
    public class ExchangesRepository : IExchangesRepository
    {
        private readonly DatabaseContext databaseContext;
        private static string defaultFiscalPrinterNumber;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangesRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public ExchangesRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;

            defaultFiscalPrinterNumber = Microinvest.DeviceService.FiscalPrinterService.GetDefaultReceiptNumber(string.Empty, 0);
            defaultFiscalPrinterNumber = defaultFiscalPrinterNumber.Substring(0, defaultFiscalPrinterNumber.IndexOf('-'));
        }

        /// <summary>
        /// Checks if record is exist.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns true, id of record and acct if record exist; otherwise returns false, 0, 0.</returns>
        /// <date>16.06.2022.</date>
        public async Task<(bool isExist, int id, int acct)> CheckRecordAsync(EExchangeDirections exchangeType, string appName, string appKey)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Exchange record = databaseContext.Exchanges.
                    Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                    FirstOrDefault();

                    if (record != null)
                    {
                        return (true, record.Id, (int)record.Acct);
                    }
                    else
                    {
                        return (false, 0, 0);
                    }
                }
            });
        }

        /// <summary>
        /// Gets record in according to parameters.
        /// </summary>
        /// <param name="exchangeType">Type of exchange.</param>
        /// <param name="appName">Name of app to exchange.</param>
        /// <param name="appKey">Identifier of app to exchange.</param>
        /// <returns>Returns Exchange object if record exist; otherwise returns null.</returns>
        /// <date>16.06.2022.</date>
        public async Task<Exchange> GetRecordAsync(EExchangeDirections exchangeType, string appName, string appKey)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(r => r.ExchangeType == exchangeType && r.AppName.Equals(appName) && r.AppKey.Equals(appKey)).
                    FirstOrDefault();
                }
            });
        }

        /// <summary>
        /// Checks whether data was exported to NAP. 
        /// </summary>
        /// <param name="date">Date to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasExportedToNAP(DateTime date)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToNAP.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex => ex.OperationHeader.Date.Month == date.Month && ex.OperationHeader.Date.Year == date.Year).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was exported to unidentified app.
        /// </summary>
        /// <param name="operType">Type of the operation to search data to the database.</param>
        /// <param name="from">Start date to search data to the database.</param>
        /// <param name="to">End date to search data to the database.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasExportedToUnidentifiedApp(EOperTypes operType, DateTime from, DateTime to, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToSomeApp.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex => 
                        ex.OperationHeader.OperType == operType && 
                        ex.OperationHeader.Date >= from && 
                        ex.OperationHeader.Date <= to.AddDays(1) && 
                        ex.OperationHeader.Acct >= acctFrom && 
                        ex.OperationHeader.Acct <= acctTo).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was exported to Warehouse Sklad Pro.
        /// </summary>
        /// <param name="from">Start date to search data to the database.</param>
        /// <param name="to">End date to search data to the database.</param>
        /// <param name="acctFrom">Start acct to search data to the database.</param>
        /// <param name="acctTo">End acct to search data to the database.</param>
        /// <returns>Returns true if data for the period was exported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasExportedToWarehouseSkladPro(DateTime from, DateTime to, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => ex.ExchangeType == EExchangeDirections.Export && ex.AppName.Equals(EExchanges.ExportToSomeApp.ToString())).
                    Include(ex => ex.OperationHeader).
                    Where(ex =>
                        (ex.OperationHeader.OperType == EOperTypes.Sale || ex.OperationHeader.OperType == EOperTypes.Refund || ex.OperationHeader.OperType == EOperTypes.Delivery) &&
                        ex.OperationHeader.Date >= from &&
                        ex.OperationHeader.Date <= to.AddDays(1) &&
                        ex.OperationHeader.Acct >= acctFrom &&
                        ex.OperationHeader.Acct <= acctTo).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was imported.
        /// </summary>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns true if data was imported; otherwise returns false.</returns>
        /// <date>12.07.2022.</date>
        public async Task<bool> DataWasImported(string appName, string appKey, long acct, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex => 
                    ex.ExchangeType == EExchangeDirections.Import && 
                    ex.AppName.Equals(appName) &&
                    ex.AppKey.Equals(appKey) &&
                    ex.Acct == acct &&
                    ex.OperType == operType).
                    FirstOrDefault() != null;
                }
            });
        }

        /// <summary>
        /// Checks whether data was imported.
        /// </summary>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns true if data was imported; otherwise returns false.</returns>
        /// <date>21.07.2022.</date>
        public async Task<(long ImpotedOperAcct, int OperationHeaderId)> GetImportedData(string appName, string appKey, long acct, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Exchange exchange = databaseContext.Exchanges.
                    Where(ex =>
                    ex.ExchangeType == EExchangeDirections.Import &&
                    ex.AppName.Equals(appName) &&
                    ex.AppKey.Equals(appKey) &&
                    ex.Acct == acct &&
                    ex.OperType == operType).
                    Include(ex => ex.OperationHeader).
                    FirstOrDefault();

                    if (exchange == null)
                    {
                        return (0, 0);
                    }
                    else
                    {
                        return (exchange.Acct, exchange.OperationHeader.Id);
                    }
                }
            });
        }

        /// <summary>
        /// Adds new record to the Exchange table.
        /// </summary>
        /// <param name="operationHeaderId">Id of record of an OperationHeader table.</param>
        /// <param name="direction">Direction of an exchange.</param>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns false if record wasn't added to database; otherwise returns true.</returns>
        /// <date>13.07.2022.</date>
        public async Task<bool> AddNewRecordAsync(int operationHeaderId, EExchangeDirections direction, string appName, string appKey, long acct, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Entities.OperationHeader.OperationHeader header = databaseContext.OperationHeaders.
                    Where(oh => oh.Id == operationHeaderId).
                    FirstOrDefault(); 
                    if (header == null)
                    {
                        return false;
                    }

                    databaseContext.Exchanges.Add(Exchange.Create(header, direction, appName, appKey, acct, operType));
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Adds new record to the Exchange table.
        /// </summary>
        /// <param name="headerAcct">Acct of record of an OperationHeader table.</param>
        /// <param name="headerOperType">Operation type of record of an OperationHeader table.</param>
        /// <param name="direction">Direction of an exchange.</param>
        /// <param name="appName">Name of the application to search data to the database.</param>
        /// <param name="appKey">Specific key of the application to search data to the database.</param>
        /// <param name="acct">Acct to search data to the database.</param>
        /// <param name="operType">Type of operation to search data to the database.</param>
        /// <returns>Returns false if record wasn't added to database; otherwise returns true.</returns>
        /// <date>15.07.2022.</date>
        public async Task<bool> AddNewRecordAsync(long headerAcct, EOperTypes headerOperType, EExchangeDirections direction, string appName, string appKey, long acct, EOperTypes operType)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    Entities.OperationHeader.OperationHeader header = databaseContext.OperationHeaders.
                    Where(oh => oh.Acct == headerAcct && oh.OperType == headerOperType).
                    FirstOrDefault();
                    if (header == null)
                    {
                        return false;
                    }

                    databaseContext.Exchanges.Add(Exchange.Create(header, direction, appName, appKey, acct, operType));
                    return databaseContext.SaveChanges() > 0;
                }
            });
        }

        /// <summary>
        /// Gets id of the exported operations. 
        /// </summary>
        /// <param name="operTypes">Types of operations to search exported data into the database.</param>
        /// <param name="dateFrom">Start date to search exported data into the database.</param>
        /// <param name="dateTo">End date to search exported data into the database.</param>
        /// <param name="acctFrom">Start acct to search exported data into the database.</param>
        /// <param name="acctTo">End acct to search exported data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns list with id of the exported operations.</returns>
        /// <date>14.07.2022.</date>
        public async Task<List<int>> GetExportedRecordsIdAsync(EOperTypes[] operTypes, DateTime dateFrom, DateTime dateTo, long acctFrom, long acctTo, string appName, string appKey)
        {
            return await Task.Run(() =>
            {
                lock(locker)
                {
                    return databaseContext.Exchanges.
                    Where(ex =>
                    operTypes.Contains(ex.OperationHeader.OperType) &&
                    ex.OperationHeader.Date >= dateFrom &&
                    ex.OperationHeader.Date <= dateTo.AddDays(1) &&
                    ex.OperationHeader.Acct >= acctFrom &&
                    ex.OperationHeader.Acct <= acctTo &&
                    ex.ExchangeType == EExchangeDirections.Export &&
                    ex.AppName.Equals(appName) &&
                    ex.AppKey.Equals(appKey)).
                    Select(ex => ex.OperationHeader.Id).
                    ToList();
                }
            });
        }

        /// <summary>
        /// Gets records to generate export file for Delta Pro.
        /// </summary>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns all records to prepare data for export to Delta Pro if "appName" is string.Empty; otherwise, it returns only previously unexported entries.</returns>
        /// <date>15.07.2022.</date>
        public async Task<List<OperationRowModel>> GetRecordsForDeltaProAsync(DateTime dateFrom, DateTime dateTo, long acctFrom, long acctTo, string appName = "", string appKey = "")
        {
            return await Task.Run(async () =>
            {
                List<int> exportedDataIndexes = null;

                if (string.IsNullOrEmpty(appName))
                {
                    exportedDataIndexes = new List<int>();
                }
                else
                {
                    exportedDataIndexes = await GetExportedRecordsIdAsync(
                        new EOperTypes[] { EOperTypes.Sale, EOperTypes.Delivery }, 
                        dateFrom, 
                        dateTo, 
                        acctFrom, 
                        acctTo, 
                        appName, 
                        appKey);
                }
                lock (locker)
                {
                    List<Entities.Documents.Document> defaultDoc = new List<Entities.Documents.Document>()
                    {
                        Entities.Documents.Document.Create(null, "0", DateTime.Now, EDocumentTypes.Unknown, DateTime.Now, "0", DateTime.Now, "", "", "", ""),
                    };

                    List<OperationRowModel>records = databaseContext.OperationHeaders.
                    Where(oh =>
                    (oh.OperType == EOperTypes.Sale || oh.OperType == EOperTypes.Delivery) &&
                    !exportedDataIndexes.Contains(oh.Id) &&
                    oh.Date >= dateFrom &&
                    oh.Date <= dateTo.AddDays(1) &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo &&
                    oh.Payment.PaymentIndex == EPaymentTypes.Cash).
                    Include(oh => oh.Partner).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    Select(oh => new
                    {
                        oh.Acct,
                        oh.OperType,
                        oh.Date,
                        oh.Partner,
                        oh.OperationDetails,
                        Documents = databaseContext.Documents.
                        Where(doc => doc.OperationHeader.Id == oh.Id && doc.DocumentType != EDocumentTypes.ProformInvoice).
                        ToList(),
                    }).
                    SelectMany(oh => oh.OperationDetails, (h, d) => new
                    {
                        h.Acct,
                        h.OperType,
                        h.Date,
                        h.Partner,
                        d.Qtty,
                        d.SalePrice,
                        d.PurchasePrice,
                        d.SaleVAT,
                        d.PurchaseVAT,
                        d.Goods.Vatgroup.VATValue,
                        h.Documents,
                    }).
                    AsEnumerable().
                    SelectMany(sm => (sm.Documents.Count == 0 ? defaultDoc : sm.Documents), (m, doc) => new
                    {
                        m.Acct,
                        m.OperType,
                        m.Date,
                        m.Partner,
                        m.Qtty,
                        m.SalePrice,
                        m.PurchasePrice,
                        m.SaleVAT,
                        m.PurchaseVAT,
                        m.VATValue,
                        doc.DocumentNumber,
                        doc.DocumentDate,
                        doc.DocumentType,
                        doc.DealDescription,
                    }).
                    AsEnumerable().
                    GroupBy(g =>
                    new { g.VATValue, g.Acct, g.OperType, g.Date, g.Partner, g.DocumentType, g.DocumentNumber },
                    (key, col) => new OperationRowModel
                    {
                        OperType = key.OperType,
                        Acct = (ulong)key.Acct,
                        Date = key.Date,
                        TransactionVatValue = key.VATValue,
                        PartnerName = key.Partner.Company,
                        PartnerPrincipal = key.Partner.Principal,
                        PartnerCity = key.Partner.City,
                        PartnerAddress = key.Partner.Address,
                        PartnerVATNumber = key.Partner.VATNumber,
                        PartnerTaxNumber = key.Partner.TaxNumber,
                        PartnerBankData = string.IsNullOrEmpty(key.Partner.IBAN) ? key.Partner.BankName : key.Partner.IBAN,
                        TransactionSum = col.Sum(i => i.Qtty * (i.OperType == EOperTypes.Delivery ? i.PurchasePrice : i.SalePrice)),
                        TransactionVATSum = col.Sum(i => i.Qtty * (i.OperType == EOperTypes.Delivery ? i.PurchaseVAT : i.SaleVAT)),
                        DocumentNumber = ulong.Parse(key.DocumentNumber),
                        DocumentDate = col.Select(i => i.DocumentDate).FirstOrDefault(),
                        DocumentType = key.DocumentType,
                        DealDescription = col.Select(i => i.DealDescription).FirstOrDefault(),
                    }).
                    ToList();

                    List<int> issuedInvoicesIndexes = databaseContext.Documents.
                    Where(doc =>
                    (doc.OperationHeader.OperType == EOperTypes.Sale || doc.OperationHeader.OperType == EOperTypes.Delivery) &&
                    doc.OperationHeader.Date >= dateFrom &&
                    doc.OperationHeader.Date <= dateTo.AddDays(1) &&
                    doc.OperationHeader.Acct >= acctFrom &&
                    doc.OperationHeader.Acct <= acctTo).
                    Select(doc => doc.OperationHeader.Id).
                    ToList();

                    List<OperationRowModel> cashBook = databaseContext.OperationHeaders.
                    Where(oh =>
                    (oh.OperType == EOperTypes.Sale || oh.OperType == EOperTypes.Delivery) &&
                    !exportedDataIndexes.Contains(oh.Id) &&
                    !issuedInvoicesIndexes.Contains(oh.Id) &&
                    oh.Date >= dateFrom &&
                    oh.Date <= dateTo.AddDays(1) &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo &&
                    oh.Payment.PaymentIndex == EPaymentTypes.Cash).
                    Include(oh => oh.Partner).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    SelectMany(oh => oh.OperationDetails, (h, d) => new
                    {
                        Acct = ParseToUlong(string.Concat(dateTo.Month.ToString().PadLeft(2, '0'), dateTo.Year)),
                        OperType = EOperTypes.WriteOut,
                        h.Date,
                        h.Partner,
                        d.Qtty,
                        d.SalePrice,
                        d.PurchasePrice,
                        d.SaleVAT,
                        d.PurchaseVAT,
                        d.Goods.Vatgroup.VATValue,
                    }).
                    AsEnumerable().
                    GroupBy(g =>
                    new { g.VATValue, g.Acct, g.OperType, g.Partner },
                    (key, col) => new OperationRowModel
                    {
                        OperType = key.OperType,
                        Acct = key.Acct,
                        Date = col.Select(i => i.Date).FirstOrDefault(),
                        TransactionVatValue = key.VATValue,
                        PartnerName = key.Partner.Company,
                        PartnerPrincipal = key.Partner.Principal,
                        PartnerCity = key.Partner.City,
                        PartnerAddress = key.Partner.Address,
                        PartnerVATNumber = key.Partner.VATNumber,
                        PartnerTaxNumber = key.Partner.TaxNumber,
                        PartnerBankData = string.IsNullOrEmpty(key.Partner.IBAN) ? key.Partner.BankName : key.Partner.IBAN,
                        TransactionSum = col.Sum(i => i.Qtty * (i.OperType == EOperTypes.Delivery ? i.PurchasePrice : i.SalePrice)),
                        TransactionVATSum = col.Sum(i => i.Qtty * (i.OperType == EOperTypes.Delivery ? i.PurchaseVAT : i.SaleVAT)),
                    }).
                    ToList();

                    records.AddRange(cashBook);

                    return records;
                }
            });
        }

        /// <summary>
        /// Gets records to generate export file for NAP.
        /// </summary>
        /// <param name="date">Date to search data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <returns>Returns all sales and refunds to prepare data for export to NAP.</returns>
        /// <date>15.07.2022.</date>
        public async Task<(List<Operation> Sales, List<Refund> Refunds)> GetRecordsForNAPAsync(DateTime date, long acctFrom, long acctTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    var tmpSales = databaseContext.OperationHeaders.
                    Where(oh =>
                    oh.OperType == EOperTypes.Sale &&
                    oh.Date.Year == date.Year &&
                    oh.Date.Month == date.Month &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo).
                    Include(oh => oh.Payment).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    Select(oh => new 
                    {
                        oh.Acct,
                        oh.Date,
                        Products = oh.OperationDetails.Select(od => new Goods
                        {
                            Name = od.Goods.Name,
                            Quantity = od.Qtty,
                            Price = od.SalePrice - od.SaleVAT,
                            VATRate = (uint)od.Goods.Vatgroup.VATValue,
                            TotalVATValue = od.Qtty * od.SaleVAT,
                            Sum = od.Qtty * od.SalePrice,
                        }).ToList(),
                        Sum = (decimal)oh.OperationDetails.Sum(od => (double)(od.Qtty * (od.SalePrice - od.SaleVAT))),
                        Discount = 0,
                        VATSum = (decimal)oh.OperationDetails.Sum(od => (double)(od.Qtty * od.SaleVAT)),
                        TotalSum = (decimal)oh.OperationDetails.Sum(od => (double)(od.Qtty * od.SalePrice)),
                        PaymentType = oh.Payment.PaymentIndex,
                        DocumentNumber = ParseToUlong(databaseContext.Documents.
                        Where(doc => doc.OperationHeader.Id == oh.Id && doc.DocumentType == EDocumentTypes.Invoice).
                        Select(doc => doc.DocumentNumber).
                        FirstOrDefault()),
                        DocumentDate = databaseContext.Documents.
                        Where(doc => doc.OperationHeader.Id == oh.Id && doc.DocumentType == EDocumentTypes.Invoice).
                        Select(doc => doc.DocumentDate).
                        FirstOrDefault(),
                    }).
                    ToList();

                    List<Operation> sales = new List<Operation>();
                    foreach (var sale in tmpSales)
                    {
                        sales.Add(new Operation()
                        {
                            Acct = (ulong)sale.Acct,
                            AcctDate = sale.Date,
                            Products = sale.Products,
                            Sum = sale.Sum,
                            Discount = sale.Discount,
                            VATSum = sale.VATSum,
                            TotalSum = sale.TotalSum,
                            PaymentMethod = sale.PaymentType,
                            DocumentNumber = sale.DocumentNumber,
                            DocumentDate = sale.DocumentDate,
                        });
                    }

                    var refunds = databaseContext.OperationHeaders.
                    Where(oh =>
                    oh.OperType == EOperTypes.Refund &&
                    oh.Date.Year == date.Year &&
                    oh.Date.Month == date.Month &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo).
                    Include(oh => oh.Payment).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    Select(oh => new Refund
                    {
                        Acct = (ulong)oh.Acct,
                        DateRefund = oh.Date,
                        PaymentType = oh.Payment.PaymentIndex,
                        TotalSumRefund = (decimal)oh.OperationDetails.Sum(oh => (double)(oh.Qtty * oh.SalePrice)),
                    }).
                    ToList();

                    return (sales, new List<Refund>());
                }
            });
        }

        /// <summary>
        /// Gets records to generate export file for some application (exported file will be with 5 columns (acct, data of item, price, qty and data of partner )).
        /// </summary>
        /// <param name="operType">Type of operations to search exported data into the database.</param>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <returns>Returns all records to prepare data for some application.</returns>
        /// <date>18.07.2022.</date>
        public async Task<List<Entities.OperationHeader.OperationHeader>> GetRecordsForUnidentifiedAppAsync(EOperTypes operType, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo)
        {
            return await Task.Run(() =>
            {
                lock (locker)
                {
                    return databaseContext.OperationHeaders.Where(oh =>
                    oh.OperType == operType &&
                    oh.Date >= dateFrom &&
                    oh.Date <= dateTo.AddDays(1) &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo).
                    Include(oh => oh.Partner).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).
                    OrderBy(oh => oh.Acct).
                    ToList();
                }
            });
        }

        /// <summary>
        /// Gets records to generate export file for Warehouse Sklad Pro.
        /// </summary>
        /// <param name="acctFrom">Start acct to search data into the database.</param>
        /// <param name="acctTo">End acct to search data into the database.</param>
        /// <param name="dateFrom">Start date to search data into the database.</param>
        /// <param name="dateTo">End date to search data into the database.</param>
        /// <param name="appName">Name of app to exchange to search exported data into the database.</param>
        /// <param name="appKey">Key of app to exchange to search exported data into the database.</param>
        /// <returns>Returns all records to prepare data for Warehouse Sklad Pro if "appName" is string.Empty; otherwise, it returns only previously unexported entries.</returns>
        /// <date>18.07.2022.</date>
        public async Task<(List<Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel> Operations, List<Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel> Partners, List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> Items)> GetRecordsForWarehouseSkladPro(long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo, string appName = "", string appKey = "")
        {
            return await Task.Run(async () =>
            {
                List<int> exportedDataIndexes = null;

                if (string.IsNullOrEmpty(appName))
                {
                    exportedDataIndexes = new List<int>();
                }
                else
                {
                    exportedDataIndexes = await GetExportedRecordsIdAsync(
                        new EOperTypes[] { EOperTypes.Sale, EOperTypes.Refund },
                        dateFrom,
                        dateTo,
                        acctFrom,
                        acctTo,
                        appName,
                        appKey);
                }
                lock (locker)
                {
                    OperationHeader.OperationHeaderRepository headerRepository = new OperationHeader.OperationHeaderRepository(databaseContext);
                    List<Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel> partners = new List<Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel>();
                    List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> items = new List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel>();

                    List<Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel> operations = databaseContext.OperationHeaders.
                    Where(oh =>
                    oh.OperType != EOperTypes.Revaluation &&
                    !exportedDataIndexes.Contains(oh.Id) &&
                    oh.Date >= dateFrom &&
                    oh.Date <= dateTo.AddDays(1) &&
                    oh.Acct >= acctFrom &&
                    oh.Acct <= acctTo).
                    Include(oh => oh.Partner).ThenInclude(p => p.Group).
                    Include(oh => oh.Payment).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Group).
                    Include(oh => oh.OperationDetails).ThenInclude(od => od.Goods).ThenInclude(g => g.Vatgroup).
                    Include(oh => oh.SrcDoc).
                    Select(oh => ParseToWarehouseProOperationModel(
                        oh, 
                        databaseContext.Documents.
                        Where(doc => doc.OperationHeader.Id == oh.Id).
                        ToList(),
                        headerRepository,
                        partners, 
                        items)).
                    ToList();

                    return (operations, partners, items);
                }
            });
        }

        /// <summary>
        /// Gets number from the object.
        /// </summary>
        /// <param name="obj">Object to parse to long.</param>
        /// <returns>Returns "0" if object is null or not number; otherwise returns parsed number.</returns>
        /// <date>15.06.2022.</date>
        private static ulong ParseToUlong(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if (ulong.TryParse(obj.ToString(), out ulong result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Parses List of Document to List of DocumentModel (WarehousePro).
        /// </summary>
        /// <param name="documents">List with data of document.</param>
        /// <param name="taxNumber">Tax number of partner.</param>
        /// <returns>Returns null if documents equals null or empty; otherwise returns List with DocumentModel object.</returns>
        /// <date>18.07.2022.</date>
        private static List<Microinvest.ExchangeDataService.Models.WarehousePro.DocumentModel> ParseToWarehouseProDocumentModel(List<Entities.Documents.Document> documents, string taxNumber)
        {
            if (documents == null || documents.Count == 0)
            {
                return null;
            }

            List<Microinvest.ExchangeDataService.Models.WarehousePro.DocumentModel> warehouseProDocuments = new List<Microinvest.ExchangeDataService.Models.WarehousePro.DocumentModel>();
            foreach (Entities.Documents.Document document in documents)
            {
                warehouseProDocuments.Add(new Microinvest.ExchangeDataService.Models.WarehousePro.DocumentModel()
                {
                    DocumentNumber = document.DocumentNumber,
                    DocumentDate = document.DocumentDate,
                    DocumentType = document.DocumentType,
                    RecipientName = document.RecipientName,
                    RecipientTaxNumber = taxNumber,
                    DocumentCreatorName = document.CreatorName,
                    DealDescription = document.DealDescription,
                    DealPlace = document.DealLocation,
                });
            }

            return warehouseProDocuments;
        }

        /// <summary>
        /// Parses OperationHeader to OperationModel (WarehousePro).
        /// </summary>
        /// <param name="operation">Data of operation.</param>
        /// <param name="document">Data of document.</param>
        /// <param name="headerRepository">Repository to get price of item.</param>
        /// <param name="partners">List of partnerts to fill.</param>
        /// <param name="items">List of items to fill.</param>
        /// <returns>Returns new OperationModel object.</returns>
        /// <date>19.07.2022.</date>
        private static Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel ParseToWarehouseProOperationModel(
            Entities.OperationHeader.OperationHeader operation,
            List<Entities.Documents.Document> documents,
            OperationHeader.OperationHeaderRepository headerRepository,
            List<Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel> partners,
            List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> items)
        {
            if (partners.Where(p => p.Id == operation.Partner.Id).FirstOrDefault() == null)
            {
                partners.Add(new Microinvest.ExchangeDataService.Models.WarehousePro.PartnerModel()
                {
                    Id = operation.Partner.Id,
                    Name = operation.Partner.Company,
                    Principal = operation.Partner.Principal,
                    Address = new Microinvest.ExchangeDataService.Models.WarehousePro.AddressModel()
                    {
                        City = operation.Partner.City,
                        Address = operation.Partner.Address,
                    },
                    Phone = operation.Partner.Phone,
                    EMail = operation.Partner.Email,
                    TaxNumber = operation.Partner.TaxNumber,
                    VATNumber = operation.Partner.VATNumber,
                    BankAccount = new Microinvest.ExchangeDataService.Models.WarehousePro.BankDataModel()
                    {
                        BankName = operation.Partner.BankName,
                        BankBIC = operation.Partner.BankBic,
                        CompanyIBAN = operation.Partner.IBAN,
                    },
                    DiscountCardNumber = operation.Partner.DiscountCard,
                    PartnerDiscount = operation.Partner.Group.Discount,
                });
            }

            return new Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel()
            {
                Acct = (ulong)operation.Acct,
                DateOpened = operation.Date,
                OperType = operation.OperType,
                PartnerId = operation.Partner.Id,
                RefundData = operation.OperType == EOperTypes.Refund ?
                            new Microinvest.ExchangeDataService.Models.WarehousePro.RefundModel()
                            {
                                Acct = (ulong)operation.SrcDoc.Acct,
                                OperType = operation.SrcDoc.OperType,
                            } :
                            null,
                Products = operation.OperationDetails.Select(od => 
                {
                    if (items.Where(i => i.Id == od.Goods.Id).FirstOrDefault() == null)
                    {
                        items.Add(new Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel()
                        {
                            Id = od.Goods.Id,
                            Name = od.Goods.Name,
                            Code = od.Goods.Code,
                            Barcode = od.Goods.Barcode,
                            Measure = od.Goods.Measure,
                            PurchasePrice = 0,
                            SalePrices = new Microinvest.ExchangeDataService.Models.WarehousePro.SalePriceModel()
                            {
                                RetailPrice = (decimal)headerRepository.GetItemPrice(od.Goods.Id),
                            },
                            VATGroupId = od.Goods.Vatgroup.Id,
                        });
                    }

                    return new Microinvest.ExchangeDataService.Models.WarehousePro.OperationProductModel()
                    {
                        ProductId = od.Goods.Id,
                        Price = operation.OperType == EOperTypes.Delivery ? od.PurchasePrice : od.SalePrice,
                        Qtty = (double)od.Qtty,
                    };
                }).ToList(),
                PaymentDetails = new Microinvest.ExchangeDataService.Models.WarehousePro.PaymentModel()
                {
                    Payments = new List<Microinvest.ExchangeDataService.Models.WarehousePro.PaymentDescriptionModel>()
                            {
                                new Microinvest.ExchangeDataService.Models.WarehousePro.PaymentDescriptionModel()
                                {
                                    PaymentId = (uint)operation.Payment.Id,
                                    PaymentSum = operation.OperType == EOperTypes.Delivery ?
                                    operation.OperationDetails.Sum(od => od.Qtty * od.PurchasePrice) :
                                    operation.OperationDetails.Sum(od => od.Qtty * od.SalePrice),
                                }
                            },
                    ReceiptData = new Microinvest.ExchangeDataService.Models.WarehousePro.Receipt()
                    {
                        ReceiptNumber = (uint)operation.EcrreceiptNumber,
                        ReceiptDate = operation.Date,
                        ReceiptType = operation.EcrreceiptType,
                        FiscalPrinterSerialNumber = operation.USN.StartsWith(defaultFiscalPrinterNumber) ?
                                                    string.Empty :
                                                    operation.USN.Substring(0, operation.USN.IndexOf('-')),
                        UniqueSaleNumber = operation.USN.StartsWith(defaultFiscalPrinterNumber) ?
                                            string.Empty :
                                            operation.USN,
                        ReceiptSum = operation.OperType == EOperTypes.Delivery ?
                                    operation.OperationDetails.Sum(od => od.Qtty * od.PurchasePrice) :
                                    operation.OperationDetails.Sum(od => od.Qtty * od.SalePrice),
                    }
                },
                Documents = ParseToWarehouseProDocumentModel(documents, operation.Partner.TaxNumber),
            };
        }
    }
}
