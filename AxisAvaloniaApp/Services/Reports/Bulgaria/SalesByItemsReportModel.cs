using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;

namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    public class SalesByItemsReportModel : IReportModel
    {
        private ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesByItemsReportModel"/> class.
        /// </summary>
        /// <param name="rowNumber">Number of the current row.</param>
        /// <remarks>Number of a row is not needed for the row with total data.</remarks>
        public SalesByItemsReportModel(int rowNumber = 0)
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            PurchaseSum = 0.ToString(settingsService.PriceFormat);

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
        /// Gets or sets name of an item.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets measure of an item.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string Measure { get; set; }

        /// <summary>
        /// Gets or sets quantity of items that were sold.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string Qty { get; set; }

        /// <summary>
        /// Gets or sets purchase sum of items that were sold.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string PurchaseSum { get; set; }

        /// <summary>
        /// Gets or sets sale sum of items that were sold.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string SaleSum { get; set; }
    }
}
