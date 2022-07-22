using AxisAvaloniaApp.Actions.Exchange;
using AxisAvaloniaApp.Actions.Sale;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules.Exchange;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment.Device;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using DataBase.Repositories.Exchanges;
using DataBase.Repositories.Items;
using DataBase.Repositories.ItemsGroups;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Partners;
using DataBase.Repositories.PartnersGroups;
using DataBase.Repositories.PaymentTypes;
using DataBase.Repositories.VATGroups;
using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService.Helpers;
using Microinvest.ExchangeDataService;
using Microinvest.ExchangeDataService.Enums;
using Microinvest.ExchangeDataService.Exceptions;
using Microinvest.ExchangeDataService.Models.Auditor;
using Microinvest.ExchangeDataService.Models.DeltaPro;
using Microinvest.ExchangeDataService.Models.OtherApp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Exchange
{
    public class ExchangeService : IExchangeService
    {
        private readonly ISettingsService settingsService;
        private readonly IVATsRepository vATsRepository;
        private readonly IPaymentTypesRepository paymentTypesRepository;
        private readonly IOperationHeaderRepository operationHeaderRepository;
        private readonly IItemRepository itemRepository;
        private readonly IItemsGroupsRepository itemsGroupsRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private readonly IExchangesRepository exchangesRepository;
        private readonly ISearchData searchService;
        private readonly ITranslationService translationService;
        private readonly ILoggerService loggerService;
        private NoDevice device;

        public ExchangeService(
            ISettingsService settingsService, 
            IExchangesRepository exchangesRepository, 
            ISearchData searchService,
            IOperationHeaderRepository operationHeaderRepository,
            IVATsRepository vATsRepository,
            IPaymentTypesRepository paymentTypesRepository,
            ITranslationService translationService,
            ILoggerService loggerService,
            IItemRepository itemRepository,
            IPartnerRepository partnerRepository,
            IItemsGroupsRepository itemsGroupsRepository,
            IPartnersGroupsRepository partnersGroupsRepository)
        {
            this.settingsService = settingsService;
            this.exchangesRepository = exchangesRepository;
            this.searchService = searchService;
            this.operationHeaderRepository = operationHeaderRepository;
            this.vATsRepository = vATsRepository;
            this.paymentTypesRepository = paymentTypesRepository;
            this.translationService = translationService;
            this.loggerService = loggerService;
            this.itemRepository = itemRepository;
            this.partnerRepository = partnerRepository;
            this.itemsGroupsRepository = itemsGroupsRepository;
            this.partnersGroupsRepository = partnersGroupsRepository;

            device = new NoDevice(settingsService);
        }

        /// <summary>
        /// Adds or removes event that occurs when the status of export is changed.
        /// </summary>
        /// <date>19.07.2022.</date>
        public event Action<string> ExchangeDataStatus;

        /// <summary>
        /// Adds or removes event that occurs when we need to get currency rate.
        /// </summary>
        /// <date>19.07.2022.</date>
        public event Func<string, Task<double>> GetCurrencyRateAsync;

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
        public async Task<string> GetDataForExportAsync(EExchanges app, long acctFrom, long acctTo, DateTime dateFrom, DateTime dateTo, string appName, string appKey)
        {
            return await Task.Run(async () =>
            {
                InvokeExchangeDataStatus(translationService.Localize("msgGettingDataFromDatabase"));
                try
                {
                    switch (app)
                    {
                        case EExchanges.ExportToNAP:
                            AuditorModel auditor = new AuditorModel()
                            {
                                TaxNumber = settingsService.AppSettings[ESettingKeys.TaxNumber],
                                UniqueShopNumber = settingsService.AppSettings[ESettingKeys.OnlineShopNumber],
                                ShopType = (EOnlineShopTypes)settingsService.AppSettings[ESettingKeys.OnlineShopType],
                                DomainName = settingsService.AppSettings[ESettingKeys.OnlineShopDomainName],
                                CreationDate = DateTime.Now,
                                Month = (ulong)dateFrom.Month,
                                Year = (ulong)dateFrom.Year,
                            };
                            ulong acct;

                            var operations = await exchangesRepository.GetRecordsForNAPAsync(dateFrom, acctFrom, acctTo);
                            InvokeExchangeDataStatus(translationService.Localize("msgCreatingDataModel"));
                            foreach (Operation sale in operations.Sales)
                            {
                                auditor.Operations.Add(sale);
                                acct = sale.Acct;
                                await exchangesRepository.AddNewRecordAsync((long)acct, EOperTypes.Sale, DataBase.Enums.EExchangeDirections.Export, app.ToString(), string.Empty, 0, 0);
                            }

                            decimal totalAmount = 0;
                            foreach (Refund refund in operations.Refunds)
                            {
                                auditor.Refunds.Add(refund);
                                totalAmount += refund.TotalSumRefund;
                                acct = refund.Acct;
                                await exchangesRepository.AddNewRecordAsync((long)acct, EOperTypes.Refund, DataBase.Enums.EExchangeDirections.Export, app.ToString(), string.Empty, 0, 0);
                            }
                            auditor.CountReturn = operations.Refunds.Count;
                            auditor.TotalSumRefund = totalAmount;

                            InvokeExchangeDataStatus(translationService.Localize("msgConvertingDataModelToTextFile"));
                            return await ExchangeData.ExportToAuditorAsync(auditor);
                        case EExchanges.ExportToDeltaPro:
                            List<Microinvest.ExchangeDataService.Models.DeltaPro.OperationRowModel> deltaProData = await exchangesRepository.GetRecordsForDeltaProAsync(
                                dateFrom,
                                dateTo,
                                acctFrom,
                                acctTo,
                                EExchanges.ExportToDeltaPro.ToString(),
                                string.Empty);

                            OperationModel deltaProModel = new OperationModel();
                            OperationItemModel deltaProItemModel;
                            InvokeExchangeDataStatus(translationService.Localize("msgCreatingDataModel"));
                            for (int i = 0; i < deltaProData.Count; i++)
                            {
                                do
                                {
                                    deltaProItemModel = new OperationItemModel();
                                    deltaProData.Add(deltaProData[i]);
                                    deltaProItemModel.PaymentInCash += deltaProData[i].TransactionSum;
                                    i++;
                                }
                                while (i < deltaProData.Count &&
                                deltaProData[i - 1].Acct == deltaProData[i].Acct &&
                                deltaProData[i - 1].OperType == deltaProData[i].OperType);

                                deltaProModel.Add(deltaProItemModel);
                                await exchangesRepository.AddNewRecordAsync(
                                    (long)deltaProData[i - 1].Acct,
                                    deltaProData[i - 1].OperType,
                                    DataBase.Enums.EExchangeDirections.Export,
                                    app.ToString(),
                                    string.Empty,
                                    0,
                                    0);
                            }

                            InvokeExchangeDataStatus(translationService.Localize("msgConvertingDataModelToTextFile"));
                            return await ExchangeData.ExportToDeltaProAsync(deltaProModel);
                        case EExchanges.ExportToSomeApp:
                            OperationDataModel operationData = new OperationDataModel();
                            List<Microinvest.ExchangeDataService.Models.OtherApp.OperationRowModel> rows;
                            List<DataBase.Entities.OperationHeader.OperationHeader> sales = await exchangesRepository.GetRecordsForUnidentifiedAppAsync(EOperTypes.Sale, acctFrom, acctTo, dateFrom, dateTo);

                            InvokeExchangeDataStatus(translationService.Localize("msgCreatingDataModel"));
                            foreach (DataBase.Entities.OperationHeader.OperationHeader sale in sales)
                            {
                                rows = new List<Microinvest.ExchangeDataService.Models.OtherApp.OperationRowModel>();
                                foreach (DataBase.Entities.OperationDetails.OperationDetail detail in sale.OperationDetails)
                                {
                                    rows.Add(new Microinvest.ExchangeDataService.Models.OtherApp.OperationRowModel()
                                    {
                                        Acct = (int)sale.Acct,
                                        ProductName = detail.Goods.Name,
                                        ProductBarcode = detail.Goods.Barcode,
                                        Price = (double)detail.SalePrice,
                                        Qtty = (double)detail.Qtty,
                                        PartnerName = sale.Partner.Company,
                                        PartnerTaxNumber = sale.Partner.TaxNumber,
                                        PartnerVATNumber = sale.Partner.VATNumber,
                                        PartnerEMail = sale.Partner.Email,
                                    });
                                }
                                operationData.ExchangeData.Add((int)sale.Acct, rows);
                                await exchangesRepository.AddNewRecordAsync(sale.Id, DataBase.Enums.EExchangeDirections.Export, app.ToString(), "", 0, 0);
                            }

                            InvokeExchangeDataStatus(translationService.Localize("msgConvertingDataModelToTextFile"));
                            return await ExchangeData.ExportInOtherAppFormatAsync(operationData);
                        case EExchanges.ExportToWarehouseSkladPro:
                            Microinvest.ExchangeDataService.Models.WarehousePro.ExchangeDataModel exchangeData = new Microinvest.ExchangeDataService.Models.WarehousePro.ExchangeDataModel();

                            // устанавливаем имя приложения и ключ
                            exchangeData.ApplicationDescription.ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                            exchangeData.ApplicationDescription.ApplicationKey = settingsService.AppSettings[ESettingKeys.SoftwareID];

                            // создаём свойство "Объекты"
                            exchangeData.Subjects = new Microinvest.ExchangeDataService.Models.WarehousePro.Subjects
                            {
                                // записываем данные нашей фирмы
                                Company = new Microinvest.ExchangeDataService.Models.WarehousePro.CompanyModel()
                                {
                                    Name = settingsService.AppSettings[ESettingKeys.Company],
                                    Code = string.Empty,
                                    Principal = settingsService.AppSettings[ESettingKeys.Principal],
                                    Address = new Microinvest.ExchangeDataService.Models.WarehousePro.AddressModel()
                                    {
                                        Country = settingsService.Country.EnglishName,
                                        City = settingsService.AppSettings[ESettingKeys.City],
                                        Address = settingsService.AppSettings[ESettingKeys.Address],
                                    },
                                    Phone = settingsService.AppSettings[ESettingKeys.Phone],
                                    TaxNumber = settingsService.AppSettings[ESettingKeys.TaxNumber],
                                    VATNumber = settingsService.AppSettings[ESettingKeys.VATNumber],
                                    BankAccount = new Microinvest.ExchangeDataService.Models.WarehousePro.BankDataModel()
                                    {
                                        BankName = settingsService.AppSettings[ESettingKeys.BankName],
                                        BankBIC = settingsService.AppSettings[ESettingKeys.BankBIC],
                                        CompanyIBAN = settingsService.AppSettings[ESettingKeys.IBAN],
                                    },
                                },
                            };

                            // записываем данные по группам НДС
                            exchangeData.Subjects.VATs = new List<Microinvest.ExchangeDataService.Models.WarehousePro.VATModel>();
                            await foreach (DataBase.Entities.VATGroups.VATGroup vATGroup in vATsRepository.GetVATGroupsAsync())
                            {
                                exchangeData.Subjects.VATs.Add(
                                            new Microinvest.ExchangeDataService.Models.WarehousePro.VATModel()
                                            {
                                                Id = vATGroup.Id,
                                                Name = vATGroup.Name,
                                                Rate = (double)vATGroup.VATValue,
                                            });
                            }

                            // записываем данные по типам оплат
                            exchangeData.Subjects.PaymentMethods = new List<Microinvest.ExchangeDataService.Models.WarehousePro.PaymentMethodModel>();
                            await foreach (DataBase.Entities.PaymentTypes.PaymentType paymentType in paymentTypesRepository.GetPaymentTypes())
                            {
                                exchangeData.Subjects.PaymentMethods.Add(
                                    new Microinvest.ExchangeDataService.Models.WarehousePro.PaymentMethodModel()
                                    {
                                        Id = paymentType.Id,
                                        Name = paymentType.Name,
                                        PaymentType = paymentType.PaymentIndex,
                                    });
                            }

                            // записываем данные по используемой валюте
                            exchangeData.Subjects.Currencies = new Microinvest.ExchangeDataService.Models.WarehousePro.Currencies();
                            exchangeData.Subjects.Currencies.MainCurrency.Abbreviature = settingsService.Country.CurrencyCode;
                            exchangeData.Subjects.Currencies.MainCurrency.Description = settingsService.Country.CurrencyName;

                            // записываем информацию по операциям
                            var warehouseOperations = await exchangesRepository.GetRecordsForWarehouseSkladPro(acctFrom, acctTo, dateFrom, dateTo);
                            exchangeData.Subjects.Partners = warehouseOperations.Partners;
                            exchangeData.Subjects.Products = warehouseOperations.Items;
                            exchangeData.Operations = new List<Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel>();
                            InvokeExchangeDataStatus(translationService.Localize("msgCreatingDataModel"));
                            foreach (Microinvest.ExchangeDataService.Models.WarehousePro.OperationModel operation in warehouseOperations.Operations)
                            {
                                exchangeData.Operations.Add(operation);

                                await exchangesRepository.AddNewRecordAsync((long)operation.Acct, operation.OperType, DataBase.Enums.EExchangeDirections.Export, app.ToString(), "", 0, 0);
                            }

                            InvokeExchangeDataStatus(translationService.Localize("msgConvertingDataModelToTextFile"));
                            return await ExchangeData.ExportInWarehouseProFormatAsync(exchangeData);
                    }
                }
                catch (Exception ex)
                {
                    InvokeErrorDescription(ex, nameof(GetDataForExportAsync));
                }

                return string.Empty;
            });
        }

        /// <summary>
        /// Parses and writes down import data to database.
        /// </summary>
        /// <param name="app">An application from which data will be imported.</param>
        /// <param name="filePath">Path to file with import data.</param>
        /// <returns>Returns true if data was parsed and written down to database successfully; otherwise returns false.</returns>
        /// <date>19.07.2022.</date>
        public async Task<bool> SaveImportedData(EExchanges app, string filePath)
        {
            return await Task.Run(async () =>
            {
                InvokeExchangeDataStatus(translationService.Localize("strDataReading"));
                try
                {
                    SaveOrder save;
                    FiscalExecutionResult fiscalResult;
                    switch (app)
                    {
                        case EExchanges.ImportFromSomeApp:
                            OperationDataModel importData = await ExchangeData.ImportFromOtherAppAsync(filePath);
                            // если данные успешно считаны
                            if (importData.ParseDataStatus.IsSuccess != null && (bool)importData.ParseDataStatus.IsSuccess)
                            {
                                // сообщаем пользователю, что мы начали запись данных в базу
                                InvokeExchangeDataStatus(translationService.Localize("strDataWriting"));

                                // формируем данные для сохранения их в базе данных
                                save = new SaveOrder();
                                fiscalResult = new FiscalExecutionResult();
                                ObservableCollection<Models.OperationItemModel> ordersList;
                                foreach (var operation in importData.ExchangeData)
                                {
                                    ordersList = new ObservableCollection<Models.OperationItemModel>();
                                    foreach (var item in operation.Value)
                                    {
                                        ordersList.Add(new Models.OperationItemModel()
                                        {
                                            Item = await Models.ItemModel.FindOrCreateItemAsync(
                                                new Models.ItemModel()
                                                { 
                                                    Name = item.ProductName,
                                                    Barcode = item.ProductBarcode,
                                                },
                                                false,
                                                InvokeExchangeDataStatus),
                                            Qty = item.Qtty,
                                            Price = item.Price,
                                        });
                                    }

                                    save.USN = device.ReceiptNumber;
                                    save.OrdersList = ordersList;
                                    save.Partner = await Models.PartnerModel.FindOrCreatePartnerAsync(
                                        new Models.PartnerModel()
                                        { 
                                            Name = operation.Value[0].PartnerName,
                                            TaxNumber = operation.Value[0].PartnerTaxNumber,
                                            VATNumber = operation.Value[0].PartnerVATNumber,
                                            Email = operation.Value[0].PartnerEMail,
                                        }, 
                                        false,
                                        InvokeExchangeDataStatus);
                                    save.PaymentType = EPaymentTypes.Cash;
                                    await save.Invoke(fiscalResult);
                                }

                                // сообщаем пользователю, что мы завершили запись данных в базу
                                InvokeExchangeDataStatus(translationService.Localize("msgImportIsFinished"));
                            }
                            else
                            {
                                // сообщаем пользователю об ошибке
                                if (importData.IncorrectDataDescription == null)
                                {
                                    throw importData.ParseDataStatus.ExceptionData;
                                }
                                else
                                {
                                    throw importData.IncorrectDataDescription.ExceptionData;
                                }
                            }
                            break;
                        case EExchanges.ImportFromWarehouseSkladPro:
                            Microinvest.ExchangeDataService.Models.WarehousePro.ExchangeDataModel importedData = await ExchangeData.ImportFromWarehouseProAsync(filePath);

                            // если десериализация данных прошла успешно и все обязательные поля заполнены
                            if (importedData.DeserializeStatus.IsSuccess != null && 
                            (bool)importedData.DeserializeStatus.IsSuccess &&
                            importedData.ValidateStatus.IsSuccess != null && 
                            (bool)importedData.ValidateStatus.IsSuccess)
                            {
                                double currencyRate = 1;
                                // проверяем, какая валюта используется в импортируемом файле
                                if (!settingsService.Country.CurrencyCode.ToLower().Equals(importedData.Subjects.Currencies.MainCurrency.Abbreviature.ToLower()))
                                {
                                    // если валюта отличается от основной (используемой приложением) - просим пользователя ввести курс для конвертации
                                    if (GetCurrencyRateAsync != null)
                                    {
                                        currencyRate = await GetCurrencyRateAsync.Invoke(
                                            string.Format(
                                                "{0} -> {1}",
                                                importedData.Subjects.Currencies.MainCurrency.Abbreviature.ToUpper(),
                                                settingsService.Country.CurrencyCode.ToUpper())
                                            );

                                        // если курс для конвертации не корретный
                                        if (currencyRate <= 0)
                                        {
                                            // сообщаем пользователю, что у нас нет данных для конвертации валюты
                                            InvokeExchangeDataStatus(translationService.Localize("msgCurrencyRateIsAbsent"));
                                            // прерываем импорт данных
                                            return false;
                                        }
                                    }                                    
                                }

                                // сообщаем пользователю, что мы начали запись данных в базу
                                InvokeExchangeDataStatus(translationService.Localize("strDataWriting"));
                                // словарь для хранения продаж (для сопоставления с источником для операции "Возврат")
                                Dictionary<ulong, int> saleOperations = new Dictionary<ulong, int>();
                                // инициализируем этапы валидации/подготовки импортируемых данных
                                OperationTypeIsSupported operationTypeIsSupported = new OperationTypeIsSupported(InvokeExchangeDataStatus);
                                RefundHasBaseOperation refundHasBaseOperation = new RefundHasBaseOperation(
                                    InvokeExchangeDataStatus, 
                                    saleOperations);
                                DataWasNotImportedEarlier dataWasNotImportedEarlier = new DataWasNotImportedEarlier(
                                    InvokeExchangeDataStatus,
                                    saleOperations,
                                    importedData.ApplicationDescription.ApplicationName,
                                    importedData.ApplicationDescription.ApplicationKey);
                                PreparePartnerData preparePartnerData = new PreparePartnerData(InvokeExchangeDataStatus);
                                PrepareOrdersList prepareOrdersList = new PrepareOrdersList(
                                    InvokeExchangeDataStatus,
                                    importedData.Subjects.Products,
                                    importedData.Subjects.VATs, 
                                    importedData.Subjects.Company.Address.Country);
                                SaveImportedWarehouseProRecord saveImportedWarehouseProRecord = new SaveImportedWarehouseProRecord(
                                    exchangesRepository,
                                    prepareOrdersList.Orders,
                                    preparePartnerData.NewPartner,
                                    saleOperations,
                                    importedData.ApplicationDescription.ApplicationName,
                                    importedData.ApplicationDescription.ApplicationKey,
                                    InvokeExchangeDataStatus);
                                foreach (var operation in importedData.Operations.OrderBy(op => op.OperType, new EOperTypesComparer()))
                                {
                                    preparePartnerData.PartnerData = (Models.PartnerModel)importedData.Subjects.Partners.FirstOrDefault(p => p.Id == operation.PartnerId);

                                    prepareOrdersList.ItemsData = operation.Products;
                                    prepareOrdersList.ExchangeRate = 
                                        operation.CurrencyId == -1 ? 
                                        currencyRate : 
                                        currencyRate * importedData.Subjects.Currencies.AdditionalCurrencies.FirstOrDefault(ac => ac.Id == operation.CurrencyId).ExchangeRate;

                                    saveImportedWarehouseProRecord.USN = operation.PaymentDetails.ReceiptData.UniqueSaleNumber;
                                    saveImportedWarehouseProRecord.PaymentType = (await paymentTypesRepository.GetPaymentTypeByIndexAsync(
                                       importedData.Subjects.PaymentMethods.
                                       FirstOrDefault(p => p.Id == operation.PaymentDetails.Payments[0].PaymentId).PaymentType)).PaymentIndex;
                                    saveImportedWarehouseProRecord.PaymentDate = operation.PaymentDetails.ReceiptData.ReceiptDate;
                                    saveImportedWarehouseProRecord.ReceiptNumber = (int)operation.PaymentDetails.ReceiptData.ReceiptNumber;
                                    saveImportedWarehouseProRecord.ReceiptType = operation.PaymentDetails.ReceiptData.ReceiptType;
                                    if (operation.OperType == EOperTypes.Refund)
                                    {
                                        saveImportedWarehouseProRecord.RefundSourceAcct = operation.RefundData.Acct;
                                    }

                                    operationTypeIsSupported.
                                        SetNext(refundHasBaseOperation).
                                        SetNext(dataWasNotImportedEarlier).
                                        SetNext(preparePartnerData).
                                        SetNext(prepareOrdersList).
                                        SetNext(saveImportedWarehouseProRecord);

                                    await operationTypeIsSupported.Invoke(((EOperTypes)operation.OperType, operation.Acct));
                                }

                                // сообщаем пользователю, что мы завершили запись данных в базу
                                InvokeExchangeDataStatus(translationService.Localize("msgImportIsFinished"));
                            }
                            else
                            {
                                // иначе сообщаем пользователю об ошибке
                                if (importedData.ValidateStatus.IsSuccess == null)
                                {
                                    throw importedData.DeserializeStatus.ExceptionData;
                                }
                                else
                                {
                                    throw importedData.ValidateStatus.ExceptionData;
                                }
                            }
                            break;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    InvokeErrorDescription(ex, nameof(SaveImportedData));
                    return false;
                }
            });
        }       

        /// <summary>
        /// Invokes description of exchange status.
        /// </summary>
        /// <param name="status">Description of exchange status.</param>
        /// <date>19.07.2022.</date>
        private void InvokeExchangeDataStatus(string status)
        {
            if (ExchangeDataStatus != null)
            {
                ExchangeDataStatus.Invoke(status);
            }
        }

        /// <summary>
        /// Invokes description of exception that happened during exchange data.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="methodName">Name of method in which exception was thrown.</param>
        /// <date>19.07.2022.</date>
        private void InvokeErrorDescription(Exception exception, string methodName)
        {
            string errorDescription = string.Empty;
            switch (exception)
            {
                case DomainNameIsEmptyException domName:
                    errorDescription = translationService.Localize("msgDomainNameHasNotBeenSet");
                    break;
                case InvalidAdditionalCodesSectionException addCol:
                    errorDescription = string.Format(
                        translationService.Localize("msgSomeElementInAdditionalCodesSectionAreInvalid"),
                        addCol.InvalidElementIndex + 1,
                        addCol.ElementSectionIndex + 1);
                    break;
                case InvalidApplicationDescriptionSectionException appDescr:
                    errorDescription = translationService.Localize("msgApplicationDescriptionSectionAbsentOrInvalid");
                    break;
                case InvalidElementSectionException invalidElementSection:
                    errorDescription = string.Format(
                        translationService.Localize("msgElementSectionIsInvalid"),
                        invalidElementSection.InvalidElementIndex + 1,
                        invalidElementSection.SectionName);
                    break;
                case InvalidSectionException invalidSection:
                    errorDescription = string.Format(
                        translationService.Localize("msgSectionIsInvalid"),
                        invalidSection.SectionName);
                    break;
                case InvalidExchangeRowException exchangeRow:
                    errorDescription = string.Format(
                        translationService.Localize("msgValueInRowIsInvalid"),
                        exchangeRow.InvalidRecord,
                        exchangeRow.InvalidValueIndex + 1);
                    break;
                case InvalidOnlineShopTypeException onlineShopType:
                    errorDescription = translationService.Localize("msgOnlineShopTypeIsNotChosen");
                    break;
                case NomenclatureDescriptionIsAbsentException nomDescrAbsent:
                    errorDescription = string.Format(
                        translationService.Localize("msgNomenclatureDescriptionIsAbsent"),
                        nomDescrAbsent.NomenclatureIndex,
                        nomDescrAbsent.DescriptionSectionName);
                    break;
                case NonSupportedOperTypeException nonSupportedOperType:
                    errorDescription = string.Format(
                        translationService.Localize("msgNonSupportedOperType"),
                        nonSupportedOperType.OperType.ToString());
                    break;
                case NotSupportedBeforeThisYearException nonSupportedYear:
                    errorDescription = translationService.Localize("msgIsSupportedOnlyFrom2020");
                    break;
                case PartnerTaxNumber_VATNumber_EmailOrNameIsEmptyException partnerDataIsEmpty:
                    errorDescription = translationService.Localize("msgTax_VATNumberOrEmainMustBeFilled");
                    break;
                case ProductNameOrProductBarcodeIsEmptyException productDataIsEmpty:
                    errorDescription = translationService.Localize("msgProductNameOrBarcodeMustBeFilled");
                    break;
                case QtyOrPriceIsNegativeOrZeroException invalidQtyOrPrice:
                    errorDescription = translationService.Localize("msgQtyOrPriceCanNotBeNegativeOrZero");
                    break;
                case SaleToRefundIsAbsentException saleToRefundIsAbsent:
                    errorDescription = translationService.Localize("msgSaleForRefundIsAbsent");
                    break;
                case TaxNumberIsEmptyException taxNumberIsEmpty:
                    errorDescription = translationService.Localize("msgTaxNumberHasNotBeenSpecified");
                    break;
                case UniqueOnlineShopNumberIsEmptyException shopNumberIsEmpty:
                    errorDescription = translationService.Localize("msgOnlineShopNumberHasNotBeenSet");
                    break;
                case FormatException format:
                    errorDescription = translationService.Localize("msgInvalidDataFormat");
                    break;
                default:
                    errorDescription = translationService.Localize("msgInvalidDataFormat");
                    loggerService.RegisterError(this, exception, methodName);
                    break;
            }

            InvokeExchangeDataStatus(errorDescription);
        }
    }
}
