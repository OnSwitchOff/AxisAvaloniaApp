using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.Services.Reports
{
    public class BulgarianReportsService : IReportsService
    {
        private ObservableCollection<ReportItemModel> supportedReports;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;

        public BulgarianReportsService()
        {
            supportedReports = new ObservableCollection<ReportItemModel>();
            columnsData = new ObservableCollection<ReportDataModel>()
            {
                new ReportDataModel("Item 1", "Title", 200),
                new ReportDataModel("Item 2", "Title2", 200),
            };
            source = new ObservableCollection<Test>()
            {
                new Test("1", "1.1"),
                new Test("2", "2.1"),
            };
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
            //throw new NotImplementedException();
        }

        private class Test
        {
            public Test(string t, string tt)
            {
                Title = t;
                Title2 = tt;
            }
            public string Title { get; set; }
            public string Title2 { get; set; }
        }
    }
}
