using Microinvest.CommonLibrary.Enums;
using Microinvest.SearchService;
using Microinvest.SearchService.Models;
using System;
using AxisAvaloniaApp.Models;
using CompanyModel = Microinvest.SearchService.Models.CompanyModel;

namespace AxisAvaloniaApp.Services.SearchNomenclatureData
{
    /// <summary>
    /// Describes service to search data of nomenclature.
    /// </summary>
    public class SearchDataService : ISearchData
    {
        private Search? searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDataService"/> class.
        /// </summary>
        public SearchDataService()
        {
            this.searchService = null;
        }

        /// <summary>
        /// Initialize connection to Microinvest club to search items and partners by key.
        /// </summary>
        /// <param name="language">Used application language (will be used to search data in according to language).</param>
        public void InitializeSearchDataTool(ELanguages language)
        {
            int applicationId = 0;
            ELanguages searchEnum = ELanguages.Bulgarian;
            if (ELanguages.IsDefined(language.ToString()))
            {
                searchEnum = ELanguages.TryParse(language.ToString());
            }

            this.searchService = new Search(applicationId, searchEnum);
        }

        /// <summary>
        /// Search partner data on the Microinvest club service by tax number or VAT number.
        /// </summary>
        /// <param name="searchKey">Partner tax number or VAT number.</param>
        /// <returns>Returns the filled partner model if the search was successful; otherwise returns the partner model with default data.</returns>
        /// <date>17.03.2022.</date>
        public async System.Threading.Tasks.Task<PartnerModel> GetPartnerData(string searchKey)
        {
            if (this.searchService == null)
            {
                throw new Exception("Service to search data doesn't initialized!");
            }

            ResponseModel<CompanyModel> response = await this.searchService.GetInfo<CompanyModel>(searchKey);

            if (response.Status == System.Net.HttpStatusCode.OK && response.Error == null)
            {
                return response.Data;
            }

            return new PartnerModel();
        }

        /// <summary>
        /// Search product data on the Microinvest club service by barcode.
        /// </summary>
        /// <param name="barcode">Product barcode.</param>
        /// <returns>Returns the filled product model if the search was successful; otherwise returns the product model with default data.</returns>
        /// <date>17.03.2022.</date>
        public async System.Threading.Tasks.Task<ItemModel> GetItemData(string barcode)
        {
            if (this.searchService == null)
            {
                throw new Exception("Service to search data doesn't initialized!");
            }

            ResponseModel<ProductModel> response = await this.searchService.GetInfo<ProductModel>(barcode);

            if (response.Status == System.Net.HttpStatusCode.OK && response.Error == null)
            {
                return response.Data;
            }

            return new ItemModel();
        }
    }
}
