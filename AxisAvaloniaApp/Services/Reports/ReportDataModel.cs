﻿namespace AxisAvaloniaApp.Services.Reports
{
    /// <summary>
    /// Describes data to generate DataGridColumn.
    /// </summary>
    public class ReportDataModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportDataModel"/> class.
        /// </summary>
        /// <param name="header">Key to localize header of the DataGridColumn.</param>
        /// <param name="data">Key to binding data to cells of the DataGridColumn.</param>
        /// <param name="width">Width of the DataGridColumn.</param>
        public ReportDataModel(string header, string data, double width)
        {
            HeaderKey = header;
            DataKey = data;
            ColumnWidth = width;
        }

        /// <summary>
        /// Gets key to localize header of the DataGridColumn.
        /// </summary>
        /// <date>06.06.2022.</date>
        public string HeaderKey { get; }

        /// <summary>
        /// Gets key to binding data to cells of the DataGridColumn.
        /// </summary>
        /// <date>06.06.2022.</date>
        public string DataKey { get; }

        /// <summary>
        /// Gets width of the DataGridColumn.
        /// </summary>
        /// <date>06.06.2022.</date>
        public double ColumnWidth { get; }
    }
}
