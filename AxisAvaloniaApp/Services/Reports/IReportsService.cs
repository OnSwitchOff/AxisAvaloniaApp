using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Reports
{
    public interface IReportsService
    {
        /// <summary>
        /// Gets list with supported reports.
        /// </summary>
        /// <date>16.06.2022.</date>
        ObservableCollection<ReportItemModel> SupportedReports { get; }

        /// <summary>
        /// Gets list with data to generate DataGridColumn.
        /// </summary>
        /// <date>16.06.2022.</date>
        ObservableCollection<ReportDataModel> ColumnsData { get; }

        /// <summary>
        /// Gets list with source to show.
        /// </summary>
        /// <date>16.06.2022.</date>
        System.Collections.IEnumerable Source { get; }

        /// <summary>
        /// Generates data to show report.
        /// </summary>
        /// <param name="reportKey">Key to get report data.</param>
        /// <param name="acctFrom">Start act number to filter data.</param>
        /// <param name="acctTo">End act number to filter data.</param>
        /// <param name="dateFrom">Start date to filter data</param>
        /// <param name="dateTo">End date to filter data.</param>
        /// <date>16.06.2022.</date>
        Task<IEnumerable> GenerateReportData(int reportKey, ulong acctFrom, ulong acctTo, DateTime dateFrom, DateTime dateTo);

    }
}
