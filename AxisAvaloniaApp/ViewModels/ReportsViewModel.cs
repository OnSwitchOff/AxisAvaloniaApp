using AxisAvaloniaApp.Services.Reports;
using System.Collections;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IReportsService reportsService;
        private ObservableCollection<ReportItemModel> supportedReports;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;

        public ReportsViewModel(IReportsService reportsService)
        {
            this.reportsService = reportsService;
            source = reportsService.Source;

            this.Title = "Reports";
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
    }
}
