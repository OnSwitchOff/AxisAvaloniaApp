using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Settings;
using DataBase.Repositories.VATGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Exchange
{
    public class PrepareOrdersList : AbstractStage
    {
        private readonly ISettingsService settingsService;
        private readonly IVATsRepository vATsRepository;
        private Action<string> newItemMessage;
        private Dictionary<int, Models.ItemModel> availableItems;
        private List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> importedItems;
        private List<Microinvest.ExchangeDataService.Models.WarehousePro.VATModel> importedVATs;
        private string countryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareOrdersList"/> class.
        /// </summary>
        /// <param name="newItemMessage">Method to show that new partnwe was created.</param>
        /// <param name="importedItems">List of items from import file.</param>
        /// <param name="importedVATs">List of VATs from import file.</param>
        /// <param name="countryName">Name of country from import file on english.</param>
        public PrepareOrdersList(
            Action<string> newItemMessage, 
            List<Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel> importedItems,
            List<Microinvest.ExchangeDataService.Models.WarehousePro.VATModel> importedVATs,
            string countryName)
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            vATsRepository = Splat.Locator.Current.GetRequiredService<IVATsRepository>();

            this.newItemMessage = newItemMessage;
            this.importedItems = importedItems;
            this.importedVATs = importedVATs;
            this.countryName = countryName;

            availableItems = new Dictionary<int, Models.ItemModel>();
            Orders = new List<Models.OperationItemModel>();
        }

        /// <summary>
        /// Sets data to search items.
        /// </summary>
        /// <date>21.07.2022.</date>
        public List<Microinvest.ExchangeDataService.Models.WarehousePro.OperationProductModel> ItemsData { private get; set; }

        /// <summary>
        /// Gets searched or created list with data to save.
        /// </summary>
        /// <date>21.07.2022.</date>
        public List<Models.OperationItemModel> Orders { get; private set; }

        /// <summary>
        /// Sets rate to exchange currency.
        /// </summary>
        /// <date>21.07.2022.</date>
        public double ExchangeRate { private get; set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Total amount of the orders list.</param>
        /// <returns>Returns a method to call the next step.</returns>
        /// <date>21.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (ItemsData == null || importedItems == null || importedVATs == null)
            {
                throw new NullReferenceException();
            }

            Microinvest.ExchangeDataService.Models.WarehousePro.VATModel? vAT;
            Models.ItemModel newItem;
            foreach (Microinvest.ExchangeDataService.Models.WarehousePro.OperationProductModel item in ItemsData)
            {
                vAT = null;
                if (availableItems.ContainsKey(item.ProductId))
                {
                    newItem = availableItems[item.ProductId];
                }
                else
                {
                    var importedItem = importedItems.FirstOrDefault(p => p.Id == item.ProductId);
                    newItem = await Models.ItemModel.FindOrCreateItemAsync(
                        (Models.ItemModel)importedItem,
                        true,
                        newItemMessage);
                    newItem.Price *= ExchangeRate;

                    // если страна в файле импорта и нашем приложении совпадают
                    if (!string.IsNullOrEmpty(countryName) &&
                        countryName.ToLower().Equals(settingsService.Country.EnglishName.ToLower()))
                    {
                        // устанавливаем реальную группу НДС
                        vAT = importedVATs.FirstOrDefault(v => v.Id == importedItem.VATGroupId);
                        Models.VATGroupModel vatGroup = await vATsRepository.GetVATGroupByKeyAsync(vAT.Name, (decimal)vAT.Rate);
                        if (vatGroup != null)
                        {
                            newItem.VATGroup.Clone(vatGroup);
                        }
                    }

                    availableItems.Add(item.ProductId, newItem);
                }

                Orders.Add(new Models.OperationItemModel()
                {
                    Item = newItem,
                    Qty = item.Qtty,
                    Price = (double)item.Price * ExchangeRate,
                    Discount = (double)item.Discount,
                    VATValue = ((double)item.Price * ExchangeRate * (1 - (double)item.Discount / 100)) - (((double)item.Price * ExchangeRate * (1 - (double)item.Discount / 100)) / (1 + (vAT == null ? newItem.VATGroup.Value : vAT.Rate) / 100)),
                });
            }

            return await base.Invoke(request);
        }
    }
}
