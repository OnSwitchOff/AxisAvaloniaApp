using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Reports.Bulgaria
{
    internal class NomenclatureOfPartnersReportModel : IReportModel
    {
        public NomenclatureOfPartnersReportModel(int rowNumber = 0)
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
        public string ClientCode { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string NameOfClient { get; set; }

        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string TaxNumber { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Principal { get; set; }
        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string City { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Address { get; set; }
        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Phone { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string DiscountCard { get; set; }


        /// <summary>
        /// Gets number of the current row.
        /// </summary>
        /// <date>28.06.2022.</date>
        public string Discount { get; set; }


    }
}
