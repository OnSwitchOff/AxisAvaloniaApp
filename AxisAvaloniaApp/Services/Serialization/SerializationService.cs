using DataBase.Repositories.Serializations;
using System;
using System.Collections.Generic;
using AxisAvaloniaApp.Enums;

namespace AxisAvaloniaApp.Services.Serialization
{
    /// <summary>
    /// Describes serialization service.
    /// </summary>
    public class SerializationService : ISerializationService
    {
        private readonly ISerializationRepository serialization;
        private Dictionary<ESerializationKeys, SerializationItemModel> serializationData;

        public SerializationService(ISerializationRepository serialization)
        {
            this.serialization = serialization;
            this.serializationData = new Dictionary<ESerializationKeys, SerializationItemModel>();
        }

        /// <summary>
        /// Gets or sets serialization value by key.
        /// </summary>
        /// <param name="key">Key to search serialization value.</param>
        /// <returns>SerializationItemModel.</returns>
        /// <date>28.03.2022.</date>
        public SerializationItemModel this[ESerializationKeys key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return this.serializationData[key];
                }

                throw new KeyNotFoundException();
            }

            set
            {
                if (this.ContainsKey(key))
                {
                    this.serializationData[key] = value;
                }
            }
        }

        /// <summary>
        /// Update serialization values in the database.
        /// </summary>
        /// <date>28.03.2022.</date>
        public void Update()
        {
            if (this.serializationData == null)
            {
                throw new Exception("Data doesn't initialized!");
            }

            foreach (KeyValuePair<ESerializationKeys, SerializationItemModel> keyValue in this.serializationData)
            {
                keyValue.Value.UpdateData();
            }
        }

        /// <summary>
        /// Determines whether the serialization data contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the serialization data.</param>
        /// <returns>Returns true if the serialization data contains an element with the specified key; otherwise returns false.</returns>
        /// <date>28.03.2022.</date>
        public bool ContainsKey(ESerializationKeys key)
        {
            return this.serializationData.ContainsKey(key);
        }

        /// <summary>
        /// Set serialization data property.
        /// </summary>
        /// <param name="group">Key to search serialization value.</param>
        /// <date>28.03.2022.</date>
        public void InitSerializationData(ESerializationGroups group)
        {
            this.serializationData = new Dictionary<ESerializationKeys, SerializationItemModel>();

            switch (group)
            {
                case ESerializationGroups.Sale:
                    this.serializationData.Add(
                        ESerializationKeys.AddColumns,
                        new SerializationItemModel(this.serialization, ESerializationKeys.AddColumns, group, "0"));
                    this.serializationData.Add(
                        ESerializationKeys.TbPartnerEnabled,
                        new SerializationItemModel(this.serialization, ESerializationKeys.TbPartnerEnabled, group, "true"));
                    this.serializationData.Add(
                        ESerializationKeys.TbPartnerID,
                        new SerializationItemModel(this.serialization, ESerializationKeys.TbPartnerID, group, "-1"));
                    this.serializationData.Add(
                        ESerializationKeys.ColRowNumberWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColRowNumberWidth, group, "35"));
                    this.serializationData.Add(
                        ESerializationKeys.ColCodeWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColCodeWidth, group, "70"));
                    this.serializationData.Add(
                        ESerializationKeys.ColBarcodeWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColBarcodeWidth, group, "110"));
                    this.serializationData.Add(
                        ESerializationKeys.ColMeasureWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColMeasureWidth, group, "80"));
                    this.serializationData.Add(
                        ESerializationKeys.ColQuantityWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColQuantityWidth, group, "80"));
                    this.serializationData.Add(
                        ESerializationKeys.ColPriceWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColPriceWidth, group, "80"));
                    this.serializationData.Add(
                        ESerializationKeys.ColDiscountWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColDiscountWidth, group, "80"));
                    this.serializationData.Add(
                        ESerializationKeys.ColTotalSumWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColTotalSumWidth, group, "145"));
                    this.serializationData.Add(
                        ESerializationKeys.ColNoteWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColNoteWidth, group, "100"));
                    break;
                case ESerializationGroups.Invoice:
                case ESerializationGroups.Proform:
                case ESerializationGroups.DebitNote:
                case ESerializationGroups.CreditNote:
                    this.serializationData.Add(
                        ESerializationKeys.ColAcctWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColAcctWidth, group, "75"));
                    this.serializationData.Add(
                        ESerializationKeys.ColDateWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColDateWidth, group, "100"));
                    this.serializationData.Add(
                        ESerializationKeys.ColCompanyWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColCompanyWidth, group, "150"));
                    this.serializationData.Add(
                        ESerializationKeys.ColCityWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColCityWidth, group, "50"));
                    this.serializationData.Add(
                        ESerializationKeys.ColAddressWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColAddressWidth, group, "70"));
                    this.serializationData.Add(
                        ESerializationKeys.ColPhoneWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColPhoneWidth, group, "50"));
                    this.serializationData.Add(
                        ESerializationKeys.ColSumWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColSumWidth, group, "66"));
                    this.serializationData.Add(
                        ESerializationKeys.ColDocumentNumberWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColDocumentNumberWidth, group, "85"));
                    this.serializationData.Add(
                        ESerializationKeys.ColDocumentDateWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColDocumentDateWidth, group, "100"));
                    this.serializationData.Add(
                        ESerializationKeys.AddColumns,
                        new SerializationItemModel(this.serialization, ESerializationKeys.AddColumns, group, "0"));
                    this.serializationData.Add(
                        ESerializationKeys.SelectedPeriod,
                        new SerializationItemModel(this.serialization, ESerializationKeys.SelectedPeriod, group, "0"));
                    this.serializationData.Add(
                        ESerializationKeys.DateFrom,
                        new SerializationItemModel(this.serialization, ESerializationKeys.DateFrom, group, DateTime.Now.ToString()));
                    this.serializationData.Add(
                        ESerializationKeys.DateTo,
                        new SerializationItemModel(this.serialization, ESerializationKeys.DateTo, group, DateTime.Now.ToString()));
                    break;
                case ESerializationGroups.Report:
                    this.serializationData.Add(
                        ESerializationKeys.SelectedGroupNodeTag,
                        new SerializationItemModel(this.serialization, ESerializationKeys.SelectedGroupNodeTag, group, "-1"));
                    break;
                case ESerializationGroups.ItemsNomenclature:
                    this.serializationData.Add(
                        ESerializationKeys.AddColumns,
                        new SerializationItemModel(this.serialization, ESerializationKeys.AddColumns, group, "0"));
                    this.serializationData.Add(
                        ESerializationKeys.SelectedGroupNodeTag,
                        new SerializationItemModel(this.serialization, ESerializationKeys.SelectedGroupNodeTag, group, "-2"));
                    this.serializationData.Add(
                        ESerializationKeys.ColCodeWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColCodeWidth, group, "35"));
                    this.serializationData.Add(
                        ESerializationKeys.ColBarcodeWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColBarcodeWidth, group, "90"));
                    this.serializationData.Add(
                        ESerializationKeys.ColMeasureWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColMeasureWidth, group, "60"));
                    this.serializationData.Add(
                        ESerializationKeys.ColPriceWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColPriceWidth, group, "60"));
                    this.serializationData.Add(
                        ESerializationKeys.ColVATGroupWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColVATGroupWidth, group, "35"));
                    break;
                case ESerializationGroups.PartnersNomenclature:
                    this.serializationData.Add(
                        ESerializationKeys.AddColumns,
                        new SerializationItemModel(this.serialization, ESerializationKeys.AddColumns, group, "0"));
                    this.serializationData.Add(
                        ESerializationKeys.SelectedGroupNodeTag,
                        new SerializationItemModel(this.serialization, ESerializationKeys.SelectedGroupNodeTag, group, "-2"));
                    this.serializationData.Add(
                        ESerializationKeys.ColPrincipalWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColPrincipalWidth, group, "90"));
                    this.serializationData.Add(
                        ESerializationKeys.ColCityWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColCityWidth, group, "100"));
                    this.serializationData.Add(
                        ESerializationKeys.ColAddressWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColAddressWidth, group, "150"));
                    this.serializationData.Add(
                        ESerializationKeys.ColPhoneWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColPhoneWidth, group, "75"));
                    this.serializationData.Add(
                        ESerializationKeys.ColEMailWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColEMailWidth, group, "75"));
                    this.serializationData.Add(
                        ESerializationKeys.ColTaxNumberWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColTaxNumberWidth, group, "65"));
                    this.serializationData.Add(
                        ESerializationKeys.ColVATNumberWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColVATNumberWidth, group, "80"));
                    this.serializationData.Add(
                        ESerializationKeys.ColDiscountCardWidth,
                        new SerializationItemModel(this.serialization, ESerializationKeys.ColDiscountCardWidth, group, "90"));
                    break;
            }
        }
    }
}
