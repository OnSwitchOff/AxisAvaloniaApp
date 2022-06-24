using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService.CustomTypes;
using System.Collections.Generic;
using System.Globalization;
using AxisAvaloniaApp.Enums;

namespace AxisAvaloniaApp.Services.Settings
{
    /// <summary>
    /// Describes setting service.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets culture used on the PC.
        /// </summary>
        /// <date>16/03/2022.</date>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets decimal separator.
        /// </summary>
        /// <date>16/03/2022.</date>
        char DecimalSeparator { get; }

        /// <summary>
        /// Gets price format (number of digits after decimal point).
        /// </summary>
        /// <date>16/03/2022.</date>
        string PriceFormat { get; }

        /// <summary>
        /// Gets quantity format (number of digits after decimal point).
        /// </summary>
        /// <date>16/03/2022.</date>
        string QtyFormat { get; }


        /// <summary>
        /// Gets or sets path to Logfile.
        /// </summary>
        /// <date>24/06/2022.</date>
        string LogfilePath { get; set; }

        /// <summary>
        /// Gets or sets language of the application.
        /// </summary>
        /// <date>16/03/2022.</date>
        ELanguages AppLanguage { get; set; }

        /// <summary>
        /// Gets or sets country for which the app was installed.
        /// </summary>
        /// <date>16/03/2022.</date>
        ECountries Country { get; set; }

        /// <summary>
        /// Gets supported languages.
        /// </summary>
        /// <date>16/03/2022.</date>
        List<ELanguages> SupportedLanguages { get; }

        /// <summary>
        /// Gets supported countries.
        /// </summary>
        /// <date>16/03/2022.</date>
        List<ECountries> SupportedCountries { get; }

        /// <summary>
        /// Gets settings of a fiscal printer.
        /// </summary>
        /// <date>16/03/2022.</date>
        Dictionary<ESettingKeys, SettingsItemModel> FiscalPrinterSettings { get; }

        /// <summary>
        /// Gets settings of a POS-terminal.
        /// </summary>
        /// <date>16/03/2022.</date>
        Dictionary<ESettingKeys, SettingsItemModel> POSTerminalSettings { get; }

        /// <summary>
        /// Gets settings of a COM scanner.
        /// </summary>
        /// <date>16/03/2022.</date>
        Dictionary<ESettingKeys, SettingsItemModel> COMScannerSettings { get; }

        /// <summary>
        /// Gets settings of the application.
        /// </summary>
        /// <date>16/03/2022.</date>
        Dictionary<ESettingKeys, SettingsItemModel> AppSettings { get; }

        /// <summary>
        /// Gets settings of an Axis-cloud.
        /// </summary>
        /// <date>16/03/2022.</date>
        Dictionary<ESettingKeys, SettingsItemModel> AxisCloudSettings { get; }

        /// <summary>
        /// Gets or sets unique sale number (will be printed on the receipt).
        /// </summary>
        /// <date>16/03/2022.</date>
        UniqueSaleNumber UniqueSaleNumber { get; set; }

        /// <summary>
        /// Update settings in the database.
        /// </summary>
        /// <param name="settingsGroup">Group of settings to update.</param>
        /// <date>16/03/2022.</date>
        void UpdateSettings(ESettingGroups settingsGroup);
    }
}
