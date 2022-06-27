using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    public class BulgarianReportsService : IReportsService
    {
        private ObservableCollection<ReportItemModel> supportedReports;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;

        public BulgarianReportsService()
        {
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
                    }
                }
            }) ;

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
        public void GenerateReportData(int reportKey, ulong acctFrom, ulong acctTo, DateTime dateFrom, DateTime dateTo)
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

                    source = new ObservableCollection<SalesReportModel>()
                    {
                        new SalesReportModel(1)
                        {
                            Acct = "1",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 1",
                            Qty = "1.00",
                            SaleSum = "1.00",
                        },
                        new SalesReportModel(2)
                        {
                            Acct = "2",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 2",
                            Qty = "10.00",
                            SaleSum = "51.00",
                        },
                        new SalesReportModel(3)
                        {
                            Acct = "3",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 3",
                            Qty = "10.00",
                            SaleSum = "51.00",
                        },
                        new SalesReportModel(4)
                        {
                            Acct = "4",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 4",
                            Qty = "1.00",
                            SaleSum = "1.00",
                        },
                        new SalesReportModel(5)
                        {
                            Acct = "5",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 5",
                            Qty = "1.00",
                            SaleSum = "1.00",
                        },
                        new SalesReportModel(6)
                        {
                            Acct = "6",
                            Date = DateTime.Now.ToShortDateString(),
                            ItemName = "Item 6",
                            Qty = "1.00",
                            SaleSum = "1.00",
                        },
                        new SalesReportModel()
                        {
                            Qty = "24.00",
                            SaleSum = "106.00",
                        },
                    };

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
                        new ReportDataModel("strEIK", "EIK", Avalonia.Layout.HorizontalAlignment.Right, 95),
                        new ReportDataModel("strSum", "Sum", Avalonia.Layout.HorizontalAlignment.Right, 100),
                    };

                    source = new ObservableCollection<OutputInvoiceReportModel>()
                    {
                        new OutputInvoiceReportModel(1)
                        {
                            InvoiceNumber = "001",
                            InvoiceDate =  DateTime.Now.ToShortDateString(),
                            Partner = "MyPartner",
                            EIK = "12312312",
                            Sum = "100,00"
                        },
                        new OutputInvoiceReportModel()
                        {
                            Sum = "100,00"
                        }

                    };
                    break;
            }
        }
    }
}
