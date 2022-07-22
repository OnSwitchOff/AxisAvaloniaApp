using AxisAvaloniaApp.Services.Reports;
using ReactiveUI;
using System.Collections;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using AxisAvaloniaApp.Services.Serialization;

namespace AxisAvaloniaApp.ViewModels
{
    public class ReportsViewModel : OperationViewModelBase
    {
        private readonly IReportsService reportsService;
        private readonly ISerializationService serializationService;

        private bool isMainContentVisible;
        private ReportItemModel selectedReport;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;
        private string acctFrom;
        private string acctTo;
        private DateTime dateFrom = new DateTime(2022, 1, 1);
        private DateTime dateTo = DateTime.Today;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportsService"></param>
        /// <param name="serializationService"></param>
        public ReportsViewModel(IReportsService reportsService, ISerializationService serializationService)
        {
            this.reportsService = reportsService;
            this.serializationService = serializationService;
            serializationService.InitSerializationData(Enums.ESerializationGroups.Report);

            IsMainContentVisible = true;

            RegisterValidationData<ReportsViewModel, string>(this, nameof(AcctFrom), () => 
            {
                return AcctFrom != null && AcctFrom.Equals("22");
            }, "strPartner");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>07.06.2022.</date>
        public bool IsMainContentVisible
        {
            get => isMainContentVisible;
            set => this.RaiseAndSetIfChanged(ref isMainContentVisible, value);
        }

        /// <summary>
        /// Gets list with supported reports.
        /// </summary>
        /// <date>16.06.2022.</date>
        public ObservableCollection<ReportItemModel> SupportedReports => reportsService.SupportedReports;

        /// <summary>
        /// Gets or sets report that is selected by user.
        /// </summary>
        /// <date>16.06.2022.</date>
        public ReportItemModel SelectedReport
        {
            get => selectedReport;
            set => this.RaiseAndSetIfChanged(ref selectedReport, value);
        }

        /// <summary>
        /// Gets or sets list with data to generate DataGridColumn.
        /// </summary>
        /// <date>16.06.2022.</date>
        public ObservableCollection<ReportDataModel> ColumnsData
        {
            get => columnsData;
            set => this.RaiseAndSetIfChanged(ref columnsData, value);
        }

        /// <summary>
        /// Gets list with source to show.
        /// </summary>
        /// <date>16.06.2022.</date>
        public IEnumerable Source
        {
            get => source;
            set => this.RaiseAndSetIfChanged(ref source, value);
        }


        public string AcctFrom
        {
            get => acctFrom;
            set => this.RaiseAndSetIfChanged(ref acctFrom, value);
        }

        public string AcctTo
        {
            get => acctTo;
            set => this.RaiseAndSetIfChanged(ref acctTo, value);
        }

        public DateTime DateFrom
        {
            get => dateFrom;
            set => this.RaiseAndSetIfChanged(ref dateFrom, value);
        }

        public DateTime DateTo
        {
            get => dateTo;
            set => this.RaiseAndSetIfChanged(ref dateTo, value);
        }

        public async void GenerateReport()
        {
            Source = await reportsService.GenerateReportData(SelectedReport.ReportKey, 0, 0, DateFrom, DateTo);
            ColumnsData = reportsService.ColumnsData;
        }

        public void Print_Export()
        {
            IsMainContentVisible = false;
        }
    }
}
