using System.Collections.Generic;

namespace AxisAvaloniaApp.Helpers
{
    public static class SaleProductModelExtensions
    {
        /// <summary>
        /// Fills up SaleProductModel with data from DataTable.
        /// </summary>
        /// <param name="products">
        /// Table to get data. Must include next necessary columns: Name, SalePrice, Quantity, Discount, VAT GroupID, VATGroupName, VATValue.
        /// </param>
        /// <returns>Fillled SaleProductModel list.</returns>
        /// <date>17.03.2022.</date>
        public static List<Microinvest.DeviceService.Models.SaleProductModel> ParseToList(this System.Collections.ObjectModel.ObservableCollection<Models.OperationItemModel> products)
        {
            List<Microinvest.DeviceService.Models.SaleProductModel> sales = new List<Microinvest.DeviceService.Models.SaleProductModel>();
            foreach (Models.OperationItemModel item in products)
            {
                if (item.Item.Id > 0)
                {
                    sales.Add(item);
                }
            }

            return sales;
        }
    }
}
