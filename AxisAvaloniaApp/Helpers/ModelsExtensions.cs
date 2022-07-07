using AxisAvaloniaApp.Models;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.Helpers
{
    public static class ModelsExtensions
    {
        /// <summary>
        /// Creates clone of GroupModel object.
        /// </summary>
        /// <param name="group">Object to clone data.</param>
        /// <param name="clonedGroup">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this GroupModel group, GroupModel clonedGroup)
        {
            group.Id = clonedGroup.Id;
            group.Name = clonedGroup.Name;
            group.Discount = clonedGroup.Discount;
        }

        /// <summary>
        /// Creates clone of PartnerModel object.
        /// </summary>
        /// <param name="partner">Object to set new data.</param>
        /// <param name="clonedPartner">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this PartnerModel partner, PartnerModel clonedPartner)
        {
            partner.Id = clonedPartner.Id;
            partner.Name = clonedPartner.Name;
            partner.Principal = clonedPartner.Principal;
            partner.City = clonedPartner.City;
            partner.Address = clonedPartner.Address;
            partner.Phone = clonedPartner.Phone;
            partner.Email = clonedPartner.Email;
            partner.TaxNumber = clonedPartner.TaxNumber;
            partner.VATNumber = clonedPartner.VATNumber;
            partner.BankName = clonedPartner.BankName;
            partner.BankBIC = clonedPartner.BankBIC;
            partner.IBAN = clonedPartner.IBAN;
            partner.DiscountCardNumber = clonedPartner.DiscountCardNumber;
            partner.Group.Clone(clonedPartner.Group);
            partner.Status = clonedPartner.Status;
        }

        /// <summary>
        /// Creates clone of ItemModel object.
        /// </summary>
        /// <param name="item">Object to set new data.</param>
        /// <param name="clonedItem">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this ItemModel item, ItemModel clonedItem)
        {
            item.Id = clonedItem.Id;
            item.Name = clonedItem.Name;
            item.Code = clonedItem.Code;
            item.Codes.Clone(clonedItem.Codes);
            item.Barcode = clonedItem.Barcode;
            item.Measure = clonedItem.Measure;
            item.Price = clonedItem.Price;
            item.Group.Clone(clonedItem.Group);
            item.VATGroup.Clone(clonedItem.VATGroup);
            item.ItemType = clonedItem.ItemType;
            item.Status = clonedItem.Status;
        }

        /// <summary>
        /// Creates clone of ItemCodeModel object.
        /// </summary>
        /// <param name="itemCode">Object to set new data.</param>
        /// <param name="clonedItemCode">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this ItemCodeModel itemCode, ItemCodeModel clonedItemCode)
        {
            itemCode.Id = clonedItemCode.Id;
            itemCode.Code = clonedItemCode.Code;
            itemCode.Measure = clonedItemCode.Measure;
            itemCode.Multiplier = clonedItemCode.Multiplier;
        }

        /// <summary>
        /// Creates clone of collection of ItemCodeModel objects.
        /// </summary>
        /// <param name="itemCodes">Object to set new data.</param>
        /// <param name="clonedItemsCodes">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this ObservableCollection<ItemCodeModel> itemCodes, ObservableCollection<ItemCodeModel> clonedItemsCodes)
        {
            itemCodes.Clear();

            foreach (ItemCodeModel itemCode in clonedItemsCodes)
            {
                itemCodes.Add(itemCode.Clone());
            }
        }

        /// <summary>
        /// Creates clone of VATGroupModel object.
        /// </summary>
        /// <param name="vATGroup">Object to set new data.</param>
        /// <param name="clonedVATGroup">Object from which data will be cloned.</param>
        /// <date>23.06.2022.</date>
        public static void Clone(this VATGroupModel vATGroup, VATGroupModel clonedVATGroup)
        {
            vATGroup.Id = clonedVATGroup.Id;
            vATGroup.Name = clonedVATGroup.Name;
            vATGroup.Value = clonedVATGroup.Value;
        }
    }
}
