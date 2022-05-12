using DataBase.Repositories.Serializations;
using System;
using AxisAvaloniaApp.Enums;

namespace AxisAvaloniaApp.Services.Serialization
{
    /// <summary>
    /// Class that describes serialization data.
    /// </summary>
    public class SerializationItemModel
    {
        private readonly ISerializationRepository serialization;
        private readonly ESerializationGroups group;
        private readonly ESerializationKeys key;
        private readonly string defaultValue;
        private string? value;
        private bool valueIsChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationItemModel"/> class.
        /// </summary>
        /// <param name="serialization">Class to communicate with database.</param>
        /// <param name="key">Key to save and load serialization value from the database.</param>
        /// <param name="group">Group to classify serialization value in the database.</param>
        /// <param name="defaultValue">Default serialization value.</param>
        public SerializationItemModel(ISerializationRepository serialization, ESerializationKeys key, ESerializationGroups group, string defaultValue)
        {
            this.serialization = serialization;
            this.key = key;
            this.group = group;
            this.defaultValue = defaultValue;

            this.value = null;
        }

        /// <summary>
        /// Gets or sets serialization value.
        /// </summary>
        /// <date>28.03.2022.</date>
        public string Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = this.serialization.GetValue(this.group.ToString(), this.key.ToString(), this.defaultValue);
                }

                return this.value;
            }

            set
            {
                if (this.Value == null || !this.Value.Equals(value))
                {
                    this.value = value;

                    this.valueIsChanged = true;
                }
            }
        }

        /// <summary>
        /// Convert SerializationItemModel object to string.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static implicit operator string(SerializationItemModel serialization)
        {
            return serialization.Value;
        }

        /// <summary>
        /// Convert SerializationItemModel object to bool.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator bool(SerializationItemModel serialization)
        {
            if (bool.TryParse(serialization.Value, out bool result))
            {
                return result;
            }

            throw new FormatException(serialization.Value);
        }

        /// <summary>
        /// Convert SerializationItemModel object to int.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator int(SerializationItemModel serialization)
        {
            if (int.TryParse(serialization.Value, out int result))
            {
                return result;
            }

            throw new FormatException(serialization.Value);
        }

        /// <summary>
        /// Convert SerializationItemModel object to float.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator float(SerializationItemModel serialization)
        {
            if (float.TryParse(serialization.Value, out float result))
            {
                return result;
            }

            throw new FormatException(serialization.Value);
        }

        /// <summary>
        /// Convert SerializationItemModel object to DateTime.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator DateTime(SerializationItemModel serialization)
        {
            if (DateTime.TryParse(serialization.Value, out DateTime result))
            {
                return result;
            }

            throw new FormatException(serialization.Value);
        }

        /// <summary>
        /// Convert SerializationItemModel object to EAdditionalSaleTableColumns.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator EAdditionalSaleTableColumns(SerializationItemModel serialization)
        {
            if (int.TryParse(serialization.Value, out int res) && Enum.IsDefined(typeof(EAdditionalSaleTableColumns), res))
            {
                return (EAdditionalSaleTableColumns)res;
            }

            if (Enum.IsDefined(typeof(EAdditionalSaleTableColumns), serialization.Value))
            {
                return (EAdditionalSaleTableColumns)Enum.Parse(typeof(EAdditionalSaleTableColumns), serialization.Value);
            }

            return 0;
        }

        /// <summary>
        /// Convert SerializationItemModel object to EAdditionalItemsTableColumns.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator EAdditionalItemsTableColumns(SerializationItemModel serialization)
        {
            if (int.TryParse(serialization.Value, out int res) && Enum.IsDefined(typeof(EAdditionalItemsTableColumns), res))
            {
                return (EAdditionalItemsTableColumns)res;
            }

            if (Enum.IsDefined(typeof(EAdditionalItemsTableColumns), serialization.Value))
            {
                return (EAdditionalItemsTableColumns)Enum.Parse(typeof(EAdditionalItemsTableColumns), serialization.Value);
            }

            return 0;
        }

        /// <summary>
        /// Convert SerializationItemModel object to EAdditionalPartnersTableColumns.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator EAdditionalPartnersTableColumns(SerializationItemModel serialization)
        {
            if (int.TryParse(serialization.Value, out int res) && Enum.IsDefined(typeof(EAdditionalPartnersTableColumns), res))
            {
                return (EAdditionalPartnersTableColumns)res;
            }

            if (Enum.IsDefined(typeof(EAdditionalPartnersTableColumns), serialization.Value))
            {
                return (EAdditionalPartnersTableColumns)Enum.Parse(typeof(EAdditionalPartnersTableColumns), serialization.Value);
            }

            return 0;
        }

        /// <summary>
        /// Convert SerializationItemModel object to EAdditionalDocumentColumns.
        /// </summary>
        /// <param name="serialization">SerializationItemModel object.</param>
        /// <date>28.03.2022.</date>
        public static explicit operator EAdditionalDocumentColumns(SerializationItemModel serialization)
        {
            if (int.TryParse(serialization.Value, out int res) && Enum.IsDefined(typeof(EAdditionalDocumentColumns), res))
            {
                return (EAdditionalDocumentColumns)res;
            }

            if (Enum.IsDefined(typeof(EAdditionalDocumentColumns), serialization.Value))
            {
                return (EAdditionalDocumentColumns)Enum.Parse(typeof(EAdditionalDocumentColumns), serialization.Value);
            }

            return 0;
        }

        /// <summary>
        /// Returns a string that represents the SerializationItemModel object.
        /// </summary>
        /// <returns>String value.</returns>
        /// <date>28.03.2022.</date>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// Update data in the database if data was changed.
        /// </summary>
        /// <date>28.03.2022.</date>
        public void UpdateData()
        {
            if (this.valueIsChanged)
            {
                this.serialization.UpdateValue(this.group.ToString(), this.key.ToString(), this.Value);

                this.valueIsChanged = false;
            }
        }
    }
}
