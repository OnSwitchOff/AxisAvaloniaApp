using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using DataBase.Entities.OperationHeader;
using DataBase.Repositories.Documents;
using DataBase.Repositories.Items;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Partners;
using DataBase.Repositories.PaymentTypes;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    public class BulgarianReportsService : IReportsService
    {
        private ObservableCollection<ReportItemModel> supportedReports;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;

        private readonly IOperationHeaderRepository operationHeaderRepository;
        private readonly IDocumentsRepository documentsRepository;
        private readonly IItemRepository itemsRepository;
        private readonly IPartnerRepository partnersRepository;
        private readonly IPaymentTypesRepository paymentTypesRepository;
        private readonly ISettingsService settingsService;

        public BulgarianReportsService()
        {
            operationHeaderRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            documentsRepository = Splat.Locator.Current.GetRequiredService<IDocumentsRepository>();
            itemsRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            partnersRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            paymentTypesRepository = Splat.Locator.Current.GetRequiredService<IPaymentTypesRepository>();
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();

            supportedReports = new ObservableCollection<ReportItemModel>();
            InitSupportedReports();
        }

        private void InitSupportedReports()
        {
            // операции
            supportedReports.Add(new ReportItemModel()
            {
                LocalizeReportNameKey = "strOperations",
                SubReports = new System.Collections.Generic.List<ReportItemModel>()
                {
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.Sales,
                        LocalizeReportNameKey = "strSales",
                    },
                }
            });
            
            // менеджмент
            supportedReports.Add(new ReportItemModel()
            {
                LocalizeReportNameKey = "strManagement",
                SubReports = new System.Collections.Generic.List<ReportItemModel>()
                {
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.SalesByItems,
                        LocalizeReportNameKey = "strSalesGroupedByGoods",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.SalesByPartners,
                        LocalizeReportNameKey = "strSalesGroupedByPartners",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.SalesByItemsGroups,
                        LocalizeReportNameKey = "strSalesGroupedByGoodsGroup",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.SalesByPartnersGroups,
                        LocalizeReportNameKey = "strSalesGroupedByPartnersGroup",
                    },
                }
            });

            // документы

            supportedReports.Add(new ReportItemModel()
            {
                LocalizeReportNameKey = "strDocuments",
                SubReports = new List<ReportItemModel>()
                {
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.OutputInvoices,
                        LocalizeReportNameKey = "strOutputInvoices",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.OutputProformInvoices,
                        LocalizeReportNameKey = "strOutputProformInvoices",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.OutputDebitNotes,
                        LocalizeReportNameKey = "strOutputDebitNotes",
                    },
                    new ReportItemModel()
                    {
                        ReportKey= (int)EBulgarianReports.OutputCreditNotes,
                        LocalizeReportNameKey = "strOutputCreditNotes"
                    }
                }
            }) ;

            // Номенклатуры

            supportedReports.Add(new ReportItemModel()
            {
                LocalizeReportNameKey = "strNomenclatures",
                SubReports = new List<ReportItemModel>()
                {
                    new ReportItemModel(){
                        ReportKey = (int)EBulgarianReports.NomenclatureOfItems,
                        LocalizeReportNameKey = "strItems",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.NomenclatureOfPaymentTypes,
                        LocalizeReportNameKey = "strPaymentTypes",
                    },
                    new ReportItemModel()
                    {
                        ReportKey = (int)EBulgarianReports.NomenclatureOfPartners,
                        LocalizeReportNameKey = "strPartners",
                    }
                }
            });



            // СУПТО
        }

        /// <summary>
        /// Gets list with supported reports.
        /// </summary>
        /// <date>16.06.2022.</date>
        public ObservableCollection<ReportItemModel> SupportedReports => supportedReports;

        /// <summary>
        /// Gets list with data to generate DataGridColumn.
        /// </summary>
        /// <date>16.06.2022.</date>
        public ObservableCollection<ReportDataModel> ColumnsData => columnsData;

        /// <summary>
        /// Gets list with source to show.
        /// </summary>
        /// <date>16.06.2022.</date>
        public IEnumerable Source => source;

        /// <summary>
        /// Generates data to show report.
        /// </summary>
        /// <param name="reportKey">Key to get report data.</param>
        /// <param name="acctFrom">Start act number to filter data.</param>
        /// <param name="acctTo">End act number to filter data.</param>
        /// <param name="dateFrom">Start date to filter data</param>
        /// <param name="dateTo">End date to filter data.</param>
        /// <date>16.06.2022.</date>
        public async void GenerateReportData(int reportKey, ulong acctFrom, ulong acctTo, DateTime dateFrom, DateTime dateTo)
        {
            switch ((EBulgarianReports)reportKey)
            {
                case EBulgarianReports.Sales:

                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strAct", "Acct", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strDate", "Date", Avalonia.Layout.HorizontalAlignment.Center, 105),
                        new ReportDataModel("strGoods", "ItemName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strMeasure", "Measure", Avalonia.Layout.HorizontalAlignment.Left, 75),
                        new ReportDataModel("strQtty", "Qty", Avalonia.Layout.HorizontalAlignment.Right, 80),
                        new ReportDataModel("strPurchaseSum", "PurchaseSum", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSaleSum", "SaleSum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<SalesReportModel>();
                    int i = 0;
                    decimal totalQtty = 0;
                    decimal totalSaleSum = 0;
                    decimal totalPurchaseSum = 0;
                    (await operationHeaderRepository.GetOperationHeadersByDatesAsync(dateFrom, dateTo, EOperTypes.Sale)).ForEach((oh) =>
                        {
                            oh.OperationDetails.ForEach((od)=>
                                {
                                    i++;
                                    totalQtty += od.Qtty;
                                    totalSaleSum += od.Qtty * od.SalePrice;
                                    totalPurchaseSum += od.Qtty * od.PurchasePrice;
                                    ((ObservableCollection<SalesReportModel>)source).Add(new SalesReportModel(i)
                                    {
                                        Acct = oh.Acct.ToString(),
                                        Date = oh.Date.ToShortDateString(),
                                        ItemName = od.Goods.Name,
                                        Qty = od.Qtty.ToString(settingsService.QtyFormat),
                                        Measure = od.Goods.Measure,
                                        PurchaseSum = (od.Qtty * od.PurchasePrice).ToString(settingsService.PriceFormat),
                                        SaleSum = (od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                                    }); 

                                }
                            );
                            
                        }
                    );                  

                    SalesReportModel resultRow = new SalesReportModel()
                    {
                        Qty = totalQtty.ToString(settingsService.QtyFormat),
                        SaleSum = totalSaleSum.ToString(settingsService.PriceFormat),
                        PurchaseSum = totalPurchaseSum.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<SalesReportModel>)source).Add(resultRow);   

                    break;
                case EBulgarianReports.SalesByItems:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strGoods", "ItemName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strMeasure", "Measure", Avalonia.Layout.HorizontalAlignment.Left, 75),
                        new ReportDataModel("strQtty", "Qty", Avalonia.Layout.HorizontalAlignment.Right, 80),
                        new ReportDataModel("strPurchaseSum", "PurchaseSum", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSaleSum", "SaleSum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<SalesByItemsReportModel>();
                    int i1 = 0;
                    decimal totalQtty1 = 0;
                    decimal totalSaleSum1 = 0;
                    decimal totalPurchaseSum1 = 0;

                    var operByDates1 = await operationHeaderRepository.GetOperationHeadersByDatesAsync(dateFrom, dateTo, EOperTypes.Sale);

                    operByDates1.SelectMany(oh => oh.OperationDetails).GroupBy(od => od.Goods.Id).
                     Select(gr =>
                     {
                         i1++;
                         return new SalesByItemsReportModel(i1)
                         {
                             ItemName = gr.First().Goods.Name,
                             Qty = gr.Sum(od => od.Qtty).ToString(settingsService.QtyFormat),
                             Measure = gr.First().Goods.Measure,
                             PurchaseSum = gr.Sum(od => od.Qtty * od.PurchasePrice).ToString(settingsService.PriceFormat),
                             SaleSum = gr.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                         };
                     }).ToList().ForEach(model => {
                         totalQtty1 += decimal.Parse(model.Qty);
                         totalSaleSum1 += decimal.Parse(model.SaleSum);
                         totalPurchaseSum1 += decimal.Parse(model.PurchaseSum);
                         ((ObservableCollection<SalesByItemsReportModel>)source).Add(model);
                     });

                    SalesByItemsReportModel resultRow1 = new SalesByItemsReportModel()
                    {
                        Qty = totalQtty1.ToString(settingsService.QtyFormat),
                        SaleSum = totalSaleSum1.ToString(settingsService.PriceFormat),
                        PurchaseSum = totalPurchaseSum1.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<SalesByItemsReportModel>)source).Add(resultRow1);
                    break;
                case EBulgarianReports.SalesByPartners:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strPartner", "PartnerName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Left, 75),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<SalesByPartnersReportModel>();
                    int i2 = 0;
                    decimal totalSum2 = 0; 

                    var operByDates2 = await operationHeaderRepository.GetOperationHeadersByDatesAsync(dateFrom, dateTo, EOperTypes.Sale);

                    operByDates2.GroupBy(oh => oh.Partner.Id).
                    Select(gr =>
                    {
                        i2++;
                        return new SalesByPartnersReportModel(i2)
                        {
                            PartnerName = gr.First().Partner.Company,
                            TaxNumber = gr.First().Partner.TaxNumber,
                            Sum = gr.SelectMany(oh => oh.OperationDetails).Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                        };
                    }).ToList().ForEach(model => {
                        totalSum2 += decimal.Parse(model.Sum);
                        ((ObservableCollection<SalesByPartnersReportModel>)source).Add(model);
                    });

                    SalesByPartnersReportModel resultRow2 = new SalesByPartnersReportModel()
                    {
                        Sum = totalSum2.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<SalesByPartnersReportModel>)source).Add(resultRow2);
                    break;
                case EBulgarianReports.SalesByItemsGroups:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strItemGroup", "ItemsGroupName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strQtty", "Qty", Avalonia.Layout.HorizontalAlignment.Right, 80),
                        new ReportDataModel("strPurchaseSum", "PurchaseSum", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSaleSum", "SaleSum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<SalesByItemsGroupsReportModel>();
                    int i3 = 0;
                    decimal totalSaleSum3 = 0;
                    decimal totalPurchaseSum3 = 0;
                    decimal totalQty3 = 0;

                    var operByDates3 = await operationHeaderRepository.GetOperationHeadersByDatesAsync(dateFrom, dateTo, EOperTypes.Sale);

                    operByDates3.SelectMany(oh => oh.OperationDetails).GroupBy(od => od.Goods.Group.Id).
                    Select(gr =>
                    {
                        i3++;
                        return new SalesByItemsGroupsReportModel(i3)
                        {
                            ItemsGroupName = gr.First().Goods.Group.Name,
                            Qty = gr.Sum(od => od.Qtty).ToString(settingsService.QtyFormat),
                            PurchaseSum = gr.Sum(od => od.Qtty * od.PurchasePrice).ToString(settingsService.PriceFormat),
                            SaleSum = gr.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat),
                        };
                    }).ToList().ForEach(model => {
                        totalQty3 += decimal.Parse(model.Qty);
                        totalPurchaseSum3 += decimal.Parse(model.PurchaseSum);
                        totalSaleSum3 += decimal.Parse(model.SaleSum);
                        ((ObservableCollection<SalesByItemsGroupsReportModel>)source).Add(model);
                    });

                    SalesByItemsGroupsReportModel resultRow3 = new SalesByItemsGroupsReportModel()
                    {
                        Qty = totalQty3.ToString(settingsService.QtyFormat),                        
                        PurchaseSum = totalPurchaseSum3.ToString(settingsService.PriceFormat),
                        SaleSum = totalSaleSum3.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<SalesByItemsGroupsReportModel>)source).Add(resultRow3);

                    break;
                case EBulgarianReports.SalesByPartnersGroups:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strPartnerGroup", "PartnersGroupName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strQtty", "Qty", Avalonia.Layout.HorizontalAlignment.Right, 80),
                        new ReportDataModel("strPurchaseSum", "PurchaseSum", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSaleSum", "SaleSum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<SalesByPartnersGroupsReportModel>();
                    int i4 = 0;
                    decimal totalSaleSum4 = 0;
                    decimal totalPurchaseSum4 = 0;
                    decimal totalQty4 = 0;

                    var operByDates4 = await operationHeaderRepository.GetOperationHeadersByDatesAsync(dateFrom, dateTo, EOperTypes.Sale);

                    operByDates4.GroupBy(oh => oh.Partner.Group.Id).
                    Select(gr =>
                    {
                        i4++;
                        return new SalesByPartnersGroupsReportModel(i4)
                        {
                            PartnersGroupName = gr.First().Partner.Group.Name,
                            Qty = gr.SelectMany(oh => oh.OperationDetails).Sum(od => od.Qtty).ToString(settingsService.QtyFormat),
                            PurchaseSum = gr.SelectMany(oh => oh.OperationDetails).Sum(od => od.Qtty * od.PurchasePrice).ToString(settingsService.PriceFormat),
                            SaleSum = gr.SelectMany(oh => oh.OperationDetails).Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat),
                        };
                    }).ToList().ForEach(model => {
                        totalQty4 += decimal.Parse(model.Qty);
                        totalPurchaseSum4 += decimal.Parse(model.PurchaseSum);
                        totalSaleSum4 += decimal.Parse(model.SaleSum);
                        ((ObservableCollection<SalesByPartnersGroupsReportModel>)source).Add(model);
                    });

                    SalesByPartnersGroupsReportModel resultRow4 = new SalesByPartnersGroupsReportModel()
                    {
                        Qty = totalQty4.ToString(settingsService.QtyFormat),
                        PurchaseSum = totalPurchaseSum4.ToString(settingsService.PriceFormat),
                        SaleSum = totalSaleSum4.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<SalesByPartnersGroupsReportModel>)source).Add(resultRow4);

                    break;

                case EBulgarianReports.OutputInvoices:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strInvoiceNumber", "InvoiceNumber", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strInvoiceDate", "InvoiceDate", Avalonia.Layout.HorizontalAlignment.Center, 105),
                        new ReportDataModel("strPartner", "Partner", Avalonia.Layout.HorizontalAlignment.Right, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<OutputInvoiceReportModel>();
                    int i5 = 0;
                    decimal totalSum5 = 0;

                    var docsByDates5 = await documentsRepository.GetDocumentsByDatesAsync(dateFrom, dateTo, EDocumentTypes.Invoice);

                    docsByDates5.ToList().ForEach(async d=>
                    {
                        i5++;
                        OutputInvoiceReportModel model = new OutputInvoiceReportModel(i5)
                        {
                            InvoiceNumber = d.DocumentNumber,
                            InvoiceDate = d.DocumentDate.ToShortDateString(),
                            Partner = d.OperationHeader.Partner.Company,
                            TaxNumber = d.OperationHeader.Partner.TaxNumber,
                            Sum = d.OperationHeader.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                        };
                        totalSum5 += decimal.Parse(model.Sum);
                        ((ObservableCollection<OutputInvoiceReportModel>)source).Add(model);
                    });

                    OutputInvoiceReportModel resultRow5 = new OutputInvoiceReportModel()
                    {
                        Sum = totalSum5.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<OutputInvoiceReportModel>)source).Add(resultRow5);
                    break;

                case EBulgarianReports.OutputProformInvoices:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strProformInvoiceNumber", "ProformInvoiceNumber", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strProformInvoiceDate", "ProformInvoiceDate", Avalonia.Layout.HorizontalAlignment.Center, 105),
                        new ReportDataModel("strPartner", "Partner", Avalonia.Layout.HorizontalAlignment.Right, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };
                    source = new ObservableCollection<ProformaInvoiceReportModel>();
                    int i6 = 0;
                    decimal totalSum6 = 0;

                    var docsByDates6 = await documentsRepository.GetDocumentsByDatesAsync(dateFrom, dateTo, EDocumentTypes.ProformInvoice);

                    docsByDates6.ToList().ForEach(async d =>
                    {
                        i6++;
                        ProformaInvoiceReportModel model = new ProformaInvoiceReportModel(i6)
                        {
                            ProformaInvoiceNumber = d.DocumentNumber,
                            ProformaInvoiceDate = d.DocumentDate.ToShortDateString(),
                            Partner = d.OperationHeader.Partner.Company,
                            TaxNumber = d.OperationHeader.Partner.TaxNumber,
                            Sum = d.OperationHeader.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                        };
                        totalSum6 += decimal.Parse(model.Sum);
                        ((ObservableCollection<ProformaInvoiceReportModel>)source).Add(model);
                    });

                    ProformaInvoiceReportModel resultRow6 = new ProformaInvoiceReportModel()
                    {
                        Sum = totalSum6.ToString(settingsService.PriceFormat)
                    };
                    ((ObservableCollection<ProformaInvoiceReportModel>)source).Add(resultRow6);
                    break;
                case EBulgarianReports.OutputDebitNotes:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strDebitNoteNumber", "DebitNoteNumber", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strDebitNoteDate", "DebitNoteDate", Avalonia.Layout.HorizontalAlignment.Center, 105),
                        new ReportDataModel("strPartner", "Partner", Avalonia.Layout.HorizontalAlignment.Right, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<OutputDebitNoteReportModel>();
                    int i7 = 0;
                    decimal totalSum7 = 0;

                    var docsByDates7 = await documentsRepository.GetDocumentsByDatesAsync(dateFrom, dateTo, EDocumentTypes.DebitNote);

                    docsByDates7.ToList().ForEach(async d =>
                    {
                        i7++;
                        OutputDebitNoteReportModel model = new OutputDebitNoteReportModel(i7)
                        {
                            DebitNoteNumber = d.DocumentNumber,
                            DebitNoteDate = d.DocumentDate.ToShortDateString(),
                            Partner = d.OperationHeader.Partner.Company,
                            TaxNumber = d.OperationHeader.Partner.TaxNumber,
                            Sum = d.OperationHeader.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                        };
                        totalSum7 += decimal.Parse(model.Sum);
                        ((ObservableCollection<OutputDebitNoteReportModel>)source).Add(model);
                    });

                    OutputDebitNoteReportModel resultRow7 = new OutputDebitNoteReportModel()
                    {
                        Sum = totalSum7.ToString("F")
                    };
                    ((ObservableCollection<OutputDebitNoteReportModel>)source).Add(resultRow7);

                    break;
                case EBulgarianReports.OutputCreditNotes:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strCreditNoteNumber", "CreditNoteNumber", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strCreditNoteDate", "CreditNoteDate", Avalonia.Layout.HorizontalAlignment.Center, 105),
                        new ReportDataModel("strPartner", "Partner", Avalonia.Layout.HorizontalAlignment.Right, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<OutputCreditNoteReportModel>();
                    int i8 = 0;
                    decimal totalSum8 = 0;

                    var docsByDates8 = await documentsRepository.GetDocumentsByDatesAsync(dateFrom, dateTo, EDocumentTypes.CreditNote);

                    docsByDates8.ToList().ForEach(async d =>
                    {
                        i8++;
                        OutputCreditNoteReportModel model = new OutputCreditNoteReportModel(i8)
                        {
                            CreditNoteNumber = d.DocumentNumber,
                            CreditNoteDate = d.DocumentDate.ToShortDateString(),
                            Partner = d.OperationHeader.Partner.Company,
                            TaxNumber = d.OperationHeader.Partner.TaxNumber,
                            Sum = d.OperationHeader.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString(settingsService.PriceFormat)
                        };
                        totalSum8 += decimal.Parse(model.Sum);
                        ((ObservableCollection<OutputCreditNoteReportModel>)source).Add(model);
                    });

                    OutputCreditNoteReportModel resultRow8 = new OutputCreditNoteReportModel()
                    {
                        Sum = totalSum8.ToString("F")
                    };
                    ((ObservableCollection<OutputCreditNoteReportModel>)source).Add(resultRow8);

                    break;

                case EBulgarianReports.NomenclatureOfItems:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strCode", "Code", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strGoods", "Item", Avalonia.Layout.HorizontalAlignment.Center, double.NaN),
                    };

                    source = new ObservableCollection<NomenclatureOfItemsReportModel>();
                    int i9 = 0;
                    await foreach (var item in itemsRepository.GetItemsAsync())
                    {
                        i9++;
                        NomenclatureOfItemsReportModel model = new NomenclatureOfItemsReportModel(i9)
                        {
                            Code = item.Code,
                            Item = item.Name
                        };
                        ((ObservableCollection<NomenclatureOfItemsReportModel>)source).Add(model);
                    }
                    break;
                case EBulgarianReports.NomenclatureOfPartners:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strClientCode", "ClientCode", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strNameOfClient", "NameOfClient", Avalonia.Layout.HorizontalAlignment.Center, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strPrincipal", "Principal", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strCity", "City", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strAddress", "Address", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strPhone", "Phone", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strDiscountCard", "DiscountCard", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strDiscount", "Discount", Avalonia.Layout.HorizontalAlignment.Right, 95),
                    };                   
                    source = new ObservableCollection<NomenclatureOfPartnersReportModel>();
                    int i10 = 0;
                    await foreach (var partner in partnersRepository.GetParnersAsync())
                    {
                        i10++;
                        NomenclatureOfPartnersReportModel model = new NomenclatureOfPartnersReportModel(i10)
                        {
                            ClientCode = partner.Id.ToString(),
                            NameOfClient = partner.Company,
                            TaxNumber = partner.TaxNumber,
                            Principal = partner.Principal,
                            City = partner.City,
                            Address = partner.Address,
                            Phone = partner.Phone,
                            DiscountCard = partner.DiscountCard,
                            Discount = partner.Group.Discount.ToString(settingsService.PriceFormat),
                        };
                        ((ObservableCollection<NomenclatureOfPartnersReportModel>)source).Add(model);
                    }
                    break;
                case EBulgarianReports.NomenclatureOfPaymentTypes:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strId", "Id", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strPaymentType", "PaymentType", Avalonia.Layout.HorizontalAlignment.Center, double.NaN),
                    };

                    source = new ObservableCollection<NomenclatureOfPaymentTypesReportModel>();
                    int i11 = 0;
                    await foreach (var paymentType in paymentTypesRepository.GetPaymentTypes())
                    {
                        i11++;
                        NomenclatureOfPaymentTypesReportModel model = new NomenclatureOfPaymentTypesReportModel(i11)
                        {
                            Id = paymentType.Id.ToString(),
                            PaymentType = paymentType.Name,
                        };
                        ((ObservableCollection<NomenclatureOfPaymentTypesReportModel>)source).Add(model);
                    }

                    break;

            }
        }
    }
}
