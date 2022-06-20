using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        private decimal price;
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
            this.price = 0.0M;
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
        public decimal Price
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
            foreach (ItemCodeModel itemCode in itemModel.Codes)
            {
                itemCodes.Add((DataBase.Entities.ItemsCodes.ItemCode)itemCode);
                itemCodes[itemCodes.Count - 1].Item = (DataBase.Entities.Items.Item)itemModel;
            }

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

            foreach (DataBase.Entities.ItemsCodes.ItemCode itemCode in item.ItemsCodes)
            {
                itemModel.Codes.Add((ItemCodeModel)itemCode);
            }

            return itemModel;
        }
    }
}
