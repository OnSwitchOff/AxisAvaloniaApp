using AxisAvaloniaApp.Helpers;
using DataBase.Repositories.Documents;
using DataBase.Repositories.OperationHeader;
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

        public BulgarianReportsService()
        {
            operationHeaderRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            documentsRepository = Splat.Locator.Current.GetRequiredService<IDocumentsRepository>();

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
                                        Qty = od.Qtty.ToString(),
                                        Measure = od.Goods.Measure,
                                        PurchaseSum = (od.Qtty * od.PurchasePrice).ToString("F"),
                                        SaleSum = (od.Qtty * od.SalePrice).ToString("F")
                                    }); 

                                }
                            );
                            
                        }
                    );                  

                    SalesReportModel resultRow = new SalesReportModel()
                    {
                        Qty = totalQtty.ToString("F"),
                        SaleSum = totalSaleSum.ToString("F"),
                        PurchaseSum = totalPurchaseSum.ToString("F")
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
                    break;
                case EBulgarianReports.SalesByPartners:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strPartner", "PartnerName", Avalonia.Layout.HorizontalAlignment.Left, double.NaN),
                        new ReportDataModel("strTaxNumber", "TaxNumber", Avalonia.Layout.HorizontalAlignment.Left, 75),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };
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
                    break;

                case EBulgarianReports.NomenclatureOfItems:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strCode", "Code", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strGoods", "Item", Avalonia.Layout.HorizontalAlignment.Center, double.NaN),
                    };
                    break;
                case EBulgarianReports.NomenclatureOfPaymentTypes:
                    columnsData = new ObservableCollection<ReportDataModel>()
                    {
                        new ReportDataModel("strRowNumber", "RowNumber", Avalonia.Layout.HorizontalAlignment.Right, 50),
                        new ReportDataModel("strId", "Id", Avalonia.Layout.HorizontalAlignment.Left, 100),
                        new ReportDataModel("strPaymentType", "PaymentType", Avalonia.Layout.HorizontalAlignment.Center, double.NaN),
                    };
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
                    break;

            }
        }
    }
}
