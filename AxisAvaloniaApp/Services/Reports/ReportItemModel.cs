using System.Collections.Generic;

namespace AxisAvaloniaApp.Services.Reports
{
    /// <summary>
    /// Describes fields of report (including inserted reports).
    /// </summary>
    public class ReportItemModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportItemModel"/> class.
        /// </summary>
        public ReportItemModel()
        {
            ReportKey = -1;
        }

        /// <summary>
        /// Key to identify report.
        /// </summary>
        /// <date>06.06.2022.</date>
        public int ReportKey { get; set; }

        /// <summary>
        /// Key to localize name of a report.
        /// </summary>
        /// <date>06.06.2022.</date>
        public string LocalizeReportNameKey { get; set; }

        /// <summary>
        /// Reports list.
        /// </summary>
        /// <date>06.06.2022.</date>
        public List<ReportItemModel> SubReports { get; set; }
    }
}
