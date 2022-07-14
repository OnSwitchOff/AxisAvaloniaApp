using AxisAvaloniaApp.Services.Reports;
using ReactiveUI;
using System.Collections;
using System.Collections.ObjectModel;
using System;
using AxisAvaloniaApp.Services.Serialization;
using System.Data;
using System.Linq;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Document;

namespace AxisAvaloniaApp.ViewModels
{
    public class ReportsViewModel : OperationViewModelBase
    {
        private readonly IReportsService reportsService;
        private readonly ISerializationService serializationService;
        private readonly ITranslationService translationService;
        private readonly IDocumentService documentService;

        private bool isMainContentVisible;
        private ReportItemModel selectedReport;
        private ObservableCollection<ReportDataModel> columnsData;
        private IEnumerable source;
        private string acctFrom;
        private string acctTo;
        private DateTime dateFrom = new DateTime(2022, 1, 1);
        private DateTime dateTo = DateTime.Today;
        private ObservableCollection<System.Drawing.Image> pages;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportsService"></param>
        /// <param name="serializationService"></param>
        public ReportsViewModel(IReportsService reportsService, ISerializationService serializationService)
        {
            this.reportsService = reportsService;
            this.serializationService = serializationService;
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            documentService = Splat.Locator.Current.GetRequiredService<IDocumentService>();
            documentService.PageParameters.PageOrientation = Microinvest.PDFCreator.Enums.EPageOrientations.Landscape;
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

        public ObservableCollection<System.Drawing.Image> Pages
        {
            get => pages == null ? pages = new ObservableCollection<System.Drawing.Image>() : pages;
            set => this.RaiseAndSetIfChanged(ref pages, value);
        }

        public IDocumentService DocumentService { get => documentService; }

        public async void GenerateReport()
        {
            if(!await reportsService.GenerateReportDataAsync(SelectedReport.ReportKey, 0, 0, DateFrom, DateTo))
            {
                return;
            }
            Source = reportsService.Source;
            ColumnsData = reportsService.ColumnsData;
        }

        public void Print_Export()
        {
            DataTable ReportTable = new System.Data.DataTable();
            ColumnsData.ToList().ForEach(cd => ReportTable.Columns.Add(translationService.Localize(cd.HeaderKey)));

            foreach (var item in Source)
            {
                object[] rowData = new object[ColumnsData.Count];
                int i = 0;
                foreach (ReportDataModel dataModel in ColumnsData)
                {
                    rowData[i] = item.GetType().GetProperty(dataModel.DataKey).GetValue(item, null);
                    i++;
                }
                ReportTable.Rows.Add(rowData);
            }

            documentService.ItemsData = ReportTable;



            // генерируем документ
            if (documentService.GenerateReport())
            {
                Pages = documentService.ConvertDocumentToImageList().Clone();
            }
            else
            {
                //loggerService.ShowDialog("msgErrorDuringReceiptGeneration", "strWarning", UserControls.MessageBox.EButtonIcons.Warning);
            }

            IsMainContentVisible = Pages.Count == 0;
        }
    }
}
