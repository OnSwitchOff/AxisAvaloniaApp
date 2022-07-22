using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class SearchItemOnMicroinvestClub : AbstractStage
    {
        private readonly ISearchData searchService;
        private string barcode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchItemOnMicroinvestClub"/> class.
        /// </summary>
        /// <param name="barcode">Barcode to search item on the Microinvest club.</param>
        public SearchItemOnMicroinvestClub(string barcode)
        {
            searchService = Splat.Locator.Current.GetRequiredService<ISearchData>();
            this.barcode = barcode;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if item wasn't searched; otherwise returns item.</returns>
        /// <date>19.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                Models.ItemModel item = await searchService.GetItemData(barcode);
                if (item != null)
                {
                    return item;
                }
            }

            return await base.Invoke(request);
        }
    }
}
