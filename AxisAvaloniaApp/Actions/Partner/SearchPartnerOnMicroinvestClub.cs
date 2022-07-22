using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class SearchPartnerOnMicroinvestClub : AbstractStage
    {
        private readonly ISearchData searchService;
        private string[] keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPartnerOnMicroinvestClub"/> class.
        /// </summary>
        /// <param name="keys">Tax number and VAT number to search partner.</param>
        public SearchPartnerOnMicroinvestClub(params string[] keys)
        {
            searchService = Splat.Locator.Current.GetRequiredService<ISearchData>();
            this.keys = keys;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if partner wasn't searched; otherwise returns partner.</returns>
        /// <date>20.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var result = await searchService.GetPartnerData(key);

                    if (result != null && result is Models.PartnerModel partner)
                    {
                        return partner;
                    }
                }
            }

            return await base.Invoke(request);
        }
    }
}
