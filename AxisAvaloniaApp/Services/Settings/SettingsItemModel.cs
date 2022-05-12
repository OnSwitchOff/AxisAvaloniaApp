using DataBase.Repositories.Settings;
using Microinvest.CommonLibrary.Enums;
using System;
using AxisAvaloniaApp.Enums;

namespace AxisAvaloniaApp.Services.Settings
{
    /// <summary>
    /// Describes data of settings item.
    /// </summary>
    public class SettingsItemModel
    {
        private readonly ISettingsRepository settings;
        private readonly ESettingGroups group;
        private readonly ESettingKeys key;
        private readonly string defaultValue;
        private string? value;
        private bool valueIsChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsItemModel"/> class.
        /// </summary>
        /// <param name="settings">Class to communicate with database.</param>
        /// <param name="key">Key to save and load setting from the database.</param>
        /// <param name="group">Group to classify setting in the database.</param>
        /// <param name="defaultValue">Default setting.</param>
        public SettingsItemModel(ISettingsRepository settings, ESettingKeys key, ESettingGroups group, string defaultValue)
        {
            this.settings = settings;
            this.key = key;
            this.group = group;
            this.value = null;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Gets or sets settings value.
        /// </summary>
        /// <date>16.03.2022.</date>
        public string Value
        {
            get
            {
                if (this.value == null)
                {
                    try
                    {
                        this.value = this.settings.GetValue(this.group.ToString(), this.key.ToString(), this.defaultValue);
                    }
                    catch
                    {

                    }
                }

                return this.value;
            }

            set
            {
                if (this.value == null || !this.Value.Equals(value))
                {
                    this.value = value;

                    this.valueIsChanged = true;
                }
            }
        }

        /// <summary>
        /// Convert SettingsItemModel object to string.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static implicit operator string(SettingsItemModel settingsItem)
        {
            return settingsItem.Value;
        }

        /// <summary>
        /// Convert SettingsItemModel object to bool.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator bool(SettingsItemModel settingsItem)
        {
            if (bool.TryParse(settingsItem.Value, out bool result))
            {
                return result;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Convert SettingsItemModel object to int.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator int(SettingsItemModel settingsItem)
        {
            if (int.TryParse(settingsItem.Value, out int result))
            {
                return result;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Convert SettingsItemModel object to DateTime.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator DateTime(SettingsItemModel settingsItem)
        {
            if (DateTime.TryParse(settingsItem.Value, out DateTime result))
            {
                return result;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Convert SettingsItemModel object to ELanguages.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator ELanguages(SettingsItemModel settingsItem)
        {
            if (ELanguages.IsDefined(settingsItem.Value))
            {
                return ELanguages.TryParse(settingsItem.Value);
            }

            return ELanguages.Bulgarian;
        }

        /// <summary>
        /// Convert SettingsItemModel object to ECountries.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator ECountries(SettingsItemModel settingsItem)
        {
            if (ECountries.IsDefined(settingsItem.Value))
            {
                return ECountries.TryParse(settingsItem.Value);
            }

            return ECountries.Bulgaria;
        }

        /// <summary>
        /// Convert SettingsItemModel object to EOnlineShopTypes.
        /// </summary>
        /// <param name="settingsItem">SettingsItemModel object.</param>
        /// <date>16.03.2022.</date>
        public static explicit operator EOnlineShopTypes(SettingsItemModel settingsItem)
        {
            if (int.TryParse(settingsItem.Value, out int value) && Enum.IsDefined(typeof(EOnlineShopTypes), value))
            {
                return (EOnlineShopTypes)value;
            }
            else if (Enum.IsDefined(typeof(EOnlineShopTypes), settingsItem.Value))
            {
                return (EOnlineShopTypes)Enum.Parse(typeof(EOnlineShopTypes), settingsItem.Value);
            }

            throw new FormatException();
        }

        /// <summary>
        /// Returns a string that represents the SettingsDataHelper object.
        /// </summary>
        /// <returns>String value.</returns>
        /// <date>16.03.2022.</date>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// Update data in the database if data was changed.
        /// </summary>
        /// <date>16.03.2022.</date>
        public void UpdateData()
        {
            if (this.valueIsChanged)
            {
                this.settings.UpdateValue(this.group.ToString(), this.key.ToString(), this.Value);

                this.valueIsChanged = false;
            }
        }
    }
}
