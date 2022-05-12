using Microinvest.CommonLibrary.Enums;
using System.Collections.Generic;

namespace DataBase.Entities.Items
{
    public class Item : Entity
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public string Measure { get; set; } = null!;
        public ItemsGroups.ItemsGroup Group { get; set; }
        public VATGroups.VATGroup Vatgroup { get; set; }
        public EItemTypes ItemType { get; set; }
        public ENomenclatureStatuses Status { get; set; }

        public List<ItemsCodes.ItemCode> ItemsCodes { get; set; } = new List<ItemsCodes.ItemCode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        internal Item() : this("", "", "", "", null, null, EItemTypes.Standard, new List<ItemsCodes.ItemCode>())
        {
            this.Status = ENomenclatureStatuses.Active;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="code">Code of item.</param>
        /// <param name="name">Name of item.</param>
        /// <param name="barcode">Barcode of item.</param>
        /// <param name="measure">Measure of item.</param>
        /// <param name="group">Group that contains of this item.</param>
        /// <param name="vATGroup">VAT group of item.</param>
        /// <param name="itemType">Type of item.</param>
        /// <param name="itemsCodes">List with aaditional codes of item.</param>
        private Item(string code, string name, string barcode, string measure, ItemsGroups.ItemsGroup group, VATGroups.VATGroup vATGroup, EItemTypes itemType, List<ItemsCodes.ItemCode> itemsCodes)
        {
            this.Code = code;
            this.Name = name;
            this.Barcode = barcode;
            this.Measure = measure;
            this.Group = group;
            this.Vatgroup = vATGroup;
            this.ItemType = itemType;
            this.Status = ENomenclatureStatuses.Active;
            this.ItemsCodes.AddRange(itemsCodes);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="code">Code of item.</param>
        /// <param name="name">Name of item.</param>
        /// <param name="barcode">Barcode of item.</param>
        /// <param name="measure">Measure of item.</param>
        /// <param name="group">Group that contains of this item.</param>
        /// <param name="vATGroup">VAT group of item.</param>
        /// <param name="itemType">Type of item.</param>
        /// <param name="itemsCodes">List with aaditional codes of item.</param>
        /// <returns>Returns <see cref="Item"/> class if parameters are correct.</returns>
        public static Item Create(string code, string name, string barcode, string measure, ItemsGroups.ItemsGroup group, VATGroups.VATGroup vATGroup, EItemTypes itemType, List<ItemsCodes.ItemCode> itemsCodes)
        {
            // check rule

            return new Item(code, name, barcode, measure, group, vATGroup, itemType, itemsCodes);
        }
    }
}
