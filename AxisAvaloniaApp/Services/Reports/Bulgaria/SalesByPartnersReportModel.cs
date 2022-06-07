namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    internal class SalesByPartnersReportModel : IReportModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalesByPartnersReportModel"/> class.
        /// </summary>
        /// <param name="rowNumber">Number of the current row.</param>
        /// <remarks>Number of a row is not needed for the row with total data.</remarks>
        public SalesByPartnersReportModel(int rowNumber = 0)
        {
            if (rowNumber > 0)
            {
                RowNumber = rowNumber.ToString();
            }

            IsTotalRow = rowNumber == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the row includes summarized data.
        /// </summary>
        /// <date>07.06.2022.</date>
        public bool IsTotalRow { get; }

        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string RowNumber { get; }

        /// <summary>
        /// Gets or sets name of a partner.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string PartnerName { get; set; }

        /// <summary>
        /// Gets or sets tax number of a partner.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Gets or sets sum of sales by the partner.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string Sum { get; set; }
    }
}
