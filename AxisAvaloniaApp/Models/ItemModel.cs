using AxisAvaloniaApp.Actions.Item;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Translation;
using DataBase.Repositories.Items;
using DataBase.Repositories.ItemsGroups;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.VATGroups;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of item.
    /// </summary>
    public class ItemModel : BaseModel
    {
        private int id;
        private string code;
        private ObservableCollection<ItemCodeModel> codes;
        private string name;
        private string barcode;
        private string measure;
        private double price;
        private GroupModel group;
        private VATGroupModel vATGroup;
        private EItemTypes itemType;
        private ENomenclatureStatuses status;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemModel"/> class.
        /// </summary>
        public ItemModel()
        {
            this.id = 0;
            this.code = string.Empty;
            this.codes = new ObservableCollection<ItemCodeModel>();
            this.barcode = string.Empty;
            this.measure = string.Empty;
            this.price = 0.0;
            this.group = new GroupModel();
            this.vATGroup = new VATGroupModel();
            this.itemType = EItemTypes.Standard;
            this.status = ENomenclatureStatuses.Active;
        }

        /// <summary>
        /// Gets or sets id of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.RaiseAndSetIfChanged(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Code
        {
            get => this.code;
            set => this.RaiseAndSetIfChanged(ref this.code, value);
        }

        /// <summary>
        /// Gets or sets additional codes of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public ObservableCollection<ItemCodeModel> Codes
        {
            get => this.codes;
            set => this.RaiseAndSetIfChanged(ref this.codes, value);
        }

        /// <summary>
        /// Gets or sets name of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets barcode of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Barcode
        {
            get => this.barcode;
            set => this.RaiseAndSetIfChanged(ref this.barcode, value);
        }

        /// <summary>
        /// Gets or sets measure of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Measure
        {
            get => this.measure;
            set
            {
                this.RaiseAndSetIfChanged(ref this.measure, value);
            }
        }

        /// <summary>
        /// Gets or sets sale price of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public double Price
        {
            get => this.price;
            set => this.RaiseAndSetIfChanged(ref this.price, value);
        }

        /// <summary>
        /// Gets or sets group of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public GroupModel Group
        {
            get => this.group;
            set => this.RaiseAndSetIfChanged(ref this.group, value);
        }

        /// <summary>
        /// Gets or sets vAT group of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public VATGroupModel VATGroup
        {
            get => this.vATGroup;
            set => this.RaiseAndSetIfChanged(ref this.vATGroup, value);
        }

        /// <summary>
        /// Gets or sets type of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public EItemTypes ItemType
        {
            get => this.itemType;
            set => this.RaiseAndSetIfChanged(ref this.itemType, value);
        }

        /// <summary>
        /// Gets or sets status of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public ENomenclatureStatuses Status
        {
            get => this.status;
            set => this.RaiseAndSetIfChanged(ref this.status, value);
        }

        /// <summary>
        /// Casts SearchService.Models.ProductModel object to ItemModel.
        /// </summary>
        /// <param name="productModel">Product data.</param>
        /// <date>17.03.2022.</date>
        public static implicit operator ItemModel(Microinvest.SearchService.Models.ProductModel productModel)
        {
            ItemModel product = new ItemModel()
            {
                Name = productModel.Name,
                Barcode = productModel.Barcode.ToString(),
                Measure = productModel.Measure,
            };

            return product;
        }

        /// <summary>
        /// Casts Item object to ItemModel.
        /// </summary>
        /// <param name="itemModel">Product data.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator DataBase.Entities.Items.Item(ItemModel itemModel)
        {
            List<DataBase.Entities.ItemsCodes.ItemCode> itemCodes = new List<DataBase.Entities.ItemsCodes.ItemCode>();
            DataBase.Entities.Items.Item item = DataBase.Entities.Items.Item.Create(
                itemModel.Code,
                itemModel.Name,
                itemModel.Barcode,
                itemModel.Measure,
                (DataBase.Entities.ItemsGroups.ItemsGroup)itemModel.Group,
                (DataBase.Entities.VATGroups.VATGroup)itemModel.VATGroup,
                itemModel.ItemType,
                itemCodes);
            item.Id = itemModel.Id;
            item.Status = itemModel.Status;

            foreach (ItemCodeModel itemCode in itemModel.Codes)
            {
                item.ItemsCodes.Add(itemCode);
                item.ItemsCodes[item.ItemsCodes.Count - 1].Item = item;
                //itemCodes.Add((DataBase.Entities.ItemsCodes.ItemCode)itemCode);
                //itemCodes[itemCodes.Count - 1].Item = item;
            }

            return item;
        }

        /// <summary>
        /// Casts Item object to ItemModel.
        /// </summary>
        /// <param name="item">Product data from database.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator ItemModel(DataBase.Entities.Items.Item item)
        {
            ItemModel itemModel = new ItemModel()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Barcode = item.Barcode,
                Measure = item.Measure,
                Group = (GroupModel)item.Group,
                VATGroup = (VATGroupModel)item.Vatgroup,
                ItemType = item.ItemType,
                Status = item.Status,
                
            };
            itemModel.Price = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>().GetItemPrice(item.Id);

            foreach (DataBase.Entities.ItemsCodes.ItemCode itemCode in item.ItemsCodes)
            {
                itemModel.Codes.Add((ItemCodeModel)itemCode);
            }

            return itemModel;
        }

        /// <summary>
        /// Casts Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel object to ItemModel.
        /// </summary>
        /// <param name="product">Product data from the file of import.</param>
        /// <date>21.07.2022.</date>
        public static explicit operator ItemModel(Microinvest.ExchangeDataService.Models.WarehousePro.ProductModel product)
        {
            ItemModel item = new ItemModel()
            {
                Name = product.Name,
                Code = product.Code,
                Barcode = product.Barcode,
                Measure = product.Measure,
                Price = (double)product.SalePrices.RetailPrice,
                ItemType = product.Type,
            };

            if (product.AdditionalCodes != null)
            {
                foreach (var code in product.AdditionalCodes)
                {
                    item.Codes.Add(new ItemCodeModel()
                    {
                        Code = code.Value,
                        Measure = code.Measure,
                        Multiplier = code.Ratio,
                    });
                }
            }

            return item;
        }

        /// <summary>
        /// Searches or creates ItemModel object by barcode or name.
        /// </summary>
        /// <param name="itemData">Data of item to search or create ItemModel object.</param>
        /// <param name="onlyDatabase">Flag indicating whether search should be only into database (without search on Microinvest club).</param>
        /// <param name="newItemMessage">Method to show that new item was created.</param>
        /// <returns>Returns ItemModel.</returns>
        /// <exception cref="FormatException">Throws exception if data of item is empty.</exception>
        /// <date>19.07.2022.</date>
        public static async Task<ItemModel> FindOrCreateItemAsync(ItemModel itemData, bool onlyDatabase, Action<string> newItemMessage)
        {
            IItemRepository itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();

            IStage searchItemIntoDatabaseByBarcode = new SearchItemIntoDatabaseByBarcode(itemData.Barcode, itemRepository);
            IStage searchItemOnMicroinvestClub = new SearchItemOnMicroinvestClub(itemData.Barcode);
            IStage searchItemIntoDatabaseByName = new SearchItemIntoDatabaseByName(itemData.Name, itemRepository);
            IStage createNewItem = new CreateNewItem(itemData);

            if (onlyDatabase)
            {
                searchItemIntoDatabaseByBarcode.
                    SetNext(searchItemIntoDatabaseByName).
                    SetNext(createNewItem);
            }
            else
            {
                searchItemIntoDatabaseByBarcode.
                    SetNext(searchItemOnMicroinvestClub).
                    SetNext(searchItemIntoDatabaseByName).
                    SetNext(createNewItem);
            }

            var result = await searchItemIntoDatabaseByBarcode.Invoke(new object());

            if (result != null && result is Models.ItemModel product)
            {
                if (product.Id == 0)
                {
                    IItemsGroupsRepository itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
                    IVATsRepository vATsRepository = Splat.Locator.Current.GetRequiredService<IVATsRepository>();

                    product.Price = itemData.Price;
                    product.Group = (GroupModel)await itemsGroupsRepository.GetGroupByIdAsync(1);
                    product.VATGroup = await vATsRepository.GetVATGroupByIdAsync(1);
                    if ((product.Id = await itemRepository.AddItemAsync((DataBase.Entities.Items.Item)product)) > 0)
                    {
                        IStage addRevaluationData = new AddRevaluationData(product, 0);
                        await addRevaluationData.Invoke(new object());
                        if (newItemMessage != null)
                        {
                            ITranslationService translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
                            newItemMessage.Invoke(
                                string.Format("{0} ({1})", 
                                translationService.Localize("msgNewItemWasCreated"), 
                                product.Name));
                        }
                    }
                }

                return product;
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}
