using AxisAvaloniaApp.Services.Reports;
using AxisAvaloniaApp.Helpers;
using ReactiveUI;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using AxisAvaloniaApp.Services.Serialization;

namespace AxisAvaloniaApp.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IReportsService reportsService;
        private readonly ISerializationService serializationService;

        private bool isMainContentVisible;
        //private ObservableCollection<ReportItemModel> supportedReports;
        private ReportItemModel selectedReport;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;
        private string startAcct;
        private string endAcct;
        private DateTimeOffset? startDate = null;
        private DateTimeOffset? endDate = null;

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

        public string StartAcct
        {
            get => startAcct;
            set => this.RaiseAndSetIfChanged(ref startAcct, value);
        }

        public string EndAcct
        {
            get => endAcct;
            set => this.RaiseAndSetIfChanged(ref endAcct, value);
        }

        public DateTimeOffset? StartDate
        {
            get => startDate;
            set => this.RaiseAndSetIfChanged(ref startDate, value);
        }

        public DateTimeOffset? EndDate
        {
            get => endDate;
            set => this.RaiseAndSetIfChanged(ref endDate, value);
        }

        public void GenerateReport()
        {
            reportsService.GenerateReportData(SelectedReport.ReportKey, 0, 0, System.DateTime.Now, System.DateTime.Now);
            Source = reportsService.Source;
            ColumnsData = reportsService.ColumnsData;
        }

        public void Print_Export()
        {
            IsMainContentVisible = false;
        }
    }
}
