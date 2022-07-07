using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    internal class ProformaInvoiceReportModel : IReportModel
    {
        public ProformaInvoiceReportModel(int rowNumber = 0)
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
        /// <date>28.06.2022.</date>
        public bool IsTotalRow { get; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string RowNumber { get; }

        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string ProformaInvoiceNumber { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string ProformaInvoiceDate { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Partner { get; set; }

        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string TaxNumber { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Sum { get; set; }
    }
}
