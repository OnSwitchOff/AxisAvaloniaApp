using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Settings;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Translation;

namespace AxisAvaloniaApp.Services.Settings
{
    /// <summary>
    /// Describes setting service.
    /// </summary>
    public partial class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository settingsRepository;
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IPaymentService paymentService;
        private char? decimalSeparator = null;
        private string priceFormat = null;
        private string qtyFormat = null;
        private Dictionary<ESettingKeys, SettingsItemModel> fiscalPrinterSettings;
        private Dictionary<ESettingKeys, SettingsItemModel> pOSTerminalSettings;
        private Dictionary<ESettingKeys, SettingsItemModel> cOMScannerSettings;
        private Dictionary<ESettingKeys, SettingsItemModel> appSettings;
        private Dictionary<ESettingKeys, SettingsItemModel> axisCloudSettings;
        private Microinvest.DeviceService.CustomTypes.UniqueSaleNumber uniqueSaleNumber;

        public SettingsService(
            ISettingsRepository settingsRepository,
            IOperationHeaderRepository headerRepository,
            IPaymentService paymentService)
        {
            this.settingsRepository = settingsRepository;
            this.headerRepository = headerRepository;
            this.paymentService = paymentService;
        }

        /// <summary>
        /// Gets culture used on the PC.
        /// </summary>
        /// <date>16/03/2022.</date>
        public CultureInfo Culture
        {
            get => CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets decimal separator.
        /// </summary>
        /// <date>24.02.2020.</date>
        public char DecimalSeparator
        {
            get => (char)(this.decimalSeparator == null ? this.decimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] : decimalSeparator);
        }

        /// <summary>
        /// Gets price format (number of digits after decimal point).
        /// </summary>
        /// <date>16/03/2022.</date>
        public string PriceFormat
        {
            get => this.priceFormat == null ? (this.priceFormat = string.Concat("N", this.Culture.NumberFormat.CurrencyDecimalDigits)) : this.priceFormat;
        }

        /// <summary>
        /// Gets quantity format (number of digits after decimal point).
        /// </summary>
        /// <date>16/03/2022.</date>
        public string QtyFormat
        {
            get => this.qtyFormat == null ? (this.qtyFormat = string.Concat("N", this.Culture.NumberFormat.NumberDecimalDigits)) : this.qtyFormat;
        }

        /// <summary>
        /// Gets or sets path to company logo image.
        /// </summary>
        /// <date>18/03/2022.</date>
        public string LogoPath { get; set; }

        /// <summary>
        /// Gets or sets language of the application.
        /// </summary>
        /// <date>16/03/2022.</date>
        public ELanguages AppLanguage
        {
            get
            {
                try
                {
                    return (ELanguages)this.AppSettings[ESettingKeys.Language];
                }
                catch
                {
                    string tmpLanguage = this.Culture.EnglishName.Substring(0, this.Culture.EnglishName.IndexOf("(") - 1).Trim();
                    return ELanguages.IsDefined(tmpLanguage) ? ELanguages.TryParse(tmpLanguage) : ELanguages.Bulgarian;
                }
            }

            set
            {
                // если значение из списка поддерживаемых языков и оно отличается от установленного
                if (this.SupportedLanguages.Contains(value) && (ELanguages)this.AppSettings[ESettingKeys.Language] != value)
                {
                    // актуализируем язык
                    this.AppSettings[ESettingKeys.Language].Value = ((int)value).ToString();
                    this.AppSettings[ESettingKeys.Language].UpdateData();
                }
            }
        }

        /// <summary>
        /// Gets or sets country for which the app was installed.
        /// </summary>
        /// <date>16/03/2022.</date>
        public ECountries Country
        {
            get => (ECountries)this.AppSettings[ESettingKeys.Country];

            set
            {
                if (this.SupportedCountries.Contains(value))
                {
                    this.AppSettings[ESettingKeys.Country].Value = ((int)value).ToString();
                    this.AppSettings[ESettingKeys.Country].UpdateData();
                }
            }
        }

        /// <summary>
        /// Gets supported languages.
        /// </summary>
        /// <date>16/03/2022.</date>
        public List<ELanguages> SupportedLanguages
        {
            get
            {
                return new List<ELanguages>()
                    {
                        ELanguages.Bulgarian,
                        ELanguages.English,
                        ELanguages.Russian,
                        ELanguages.Ukrainian,
                    };
            }
        }

        /// <summary>
        /// Gets supported countries.
        /// </summary>
        /// <date>16/03/2022.</date>
        public List<ECountries> SupportedCountries
        {
            get
            {
                return new List<ECountries>()
                    {
                        ECountries.Bulgaria,
                    };
            }
        }

        /// <summary>
        /// Gets settings of a fiscal printer.
        /// </summary>
        /// <date>16/03/2022.</date>
        public Dictionary<ESettingKeys, SettingsItemModel> FiscalPrinterSettings
        {
            get
            {
                if (this.fiscalPrinterSettings == null)
                {
                    this.fiscalPrinterSettings = new Dictionary<ESettingKeys, SettingsItemModel>()
                    {
                        {
                            ESettingKeys.DeviceIsUsed,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIsUsed,
                                ESettingGroups.FiscalPrinter,
                                "false")
                        },
                        {
                            ESettingKeys.DeviceManufacturer,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceManufacturer,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceModel,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceModel,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceProtocol,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceProtocol,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.ComPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.ComPort,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceBaudRate,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceBaudRate,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceIPAddress,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIPAddress,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceIPPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIPPort,
                                ESettingGroups.FiscalPrinter,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceLogin,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceLogin,
                                ESettingGroups.FiscalPrinter,
                                "01")
                        },
                        {
                            ESettingKeys.DevicePassword,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DevicePassword,
                                ESettingGroups.FiscalPrinter,
                                "1")
                        },
                        {
                            ESettingKeys.OperatorCode,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.OperatorCode,
                                ESettingGroups.FiscalPrinter,
                                "0001")
                        },
                    };
                }

                return this.fiscalPrinterSettings;
            }
        }

        /// <summary>
        /// Gets settings of a POS-terminal.
        /// </summary>
        /// <date>16/03/2022.</date>
        public Dictionary<ESettingKeys, SettingsItemModel> POSTerminalSettings
        {
            get
            {
                if (this.pOSTerminalSettings == null)
                {
                    this.pOSTerminalSettings = new Dictionary<ESettingKeys, SettingsItemModel>()
                    {
                        {
                            ESettingKeys.DeviceIsUsed,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIsUsed,
                                ESettingGroups.POSTerminal,
                                "false")
                        },
                        {
                            ESettingKeys.DeviceManufacturer,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceManufacturer,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceModel,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceModel,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceProtocol,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceProtocol,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.ComPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.ComPort,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceBaudRate,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceBaudRate,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceIPAddress,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIPAddress,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DeviceIPPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIPPort,
                                ESettingGroups.POSTerminal,
                                string.Empty)
                        },
                    };
                }

                return this.pOSTerminalSettings;
            }
        }

        /// <summary>
        /// Gets settings of a COM scanner.
        /// </summary>
        /// <date>16/03/2022.</date>
        public Dictionary<ESettingKeys, SettingsItemModel> COMScannerSettings
        {
            get
            {
                if (this.cOMScannerSettings == null)
                {
                    this.cOMScannerSettings = new Dictionary<ESettingKeys, SettingsItemModel>()
                    {
                        {
                            ESettingKeys.ComPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.ComPort,
                                ESettingGroups.COMScanner,
                                "strNotActive")
                        },
                    };
                }

                return this.cOMScannerSettings;
            }
        }

        /// <summary>
        /// Gets settings of the application.
        /// </summary>
        /// <date>16/03/2022.</date>
        public Dictionary<ESettingKeys, SettingsItemModel> AppSettings
        {
            get
            {
                if (this.appSettings == null)
                {
                    string tmpLanguage = this.Culture.EnglishName.Substring(0, this.Culture.EnglishName.IndexOf("(") - 1).Trim();
                    ELanguages defaultLanguage = ELanguages.IsDefined(tmpLanguage) ? ELanguages.TryParse(tmpLanguage) : ELanguages.Bulgarian;
                    this.appSettings = new Dictionary<ESettingKeys, SettingsItemModel>()
                    {
                        {
                            ESettingKeys.Country,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Country,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Language,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Language,
                                ESettingGroups.App,
                                (this.SupportedLanguages.Contains(defaultLanguage) ? defaultLanguage : ELanguages.Bulgarian).ToString())
                        },
                        {
                            ESettingKeys.UserName,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.UserName,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.DBVersion,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DBVersion,
                                ESettingGroups.App,
                                "1")
                        },
                        {
                            ESettingKeys.Company,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Company,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Principal,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Principal,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.City,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.City,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Address,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Address,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Phone,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Phone,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Email,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Email,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.TaxNumber,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.TaxNumber,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.VATNumber,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.VATNumber,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.BankName,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.BankName,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.BankBIC,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.BankBIC,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.IBAN,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.IBAN,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.LastBackup,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.LastBackup,
                                ESettingGroups.App,
                                new DateTime(2021, 11, 1).ToString())
                        },
                        {
                            ESettingKeys.SoftwareID,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.SoftwareID,
                                ESettingGroups.App,
                                "0000000000")
                        },
                        {
                            ESettingKeys.SoftwareVersion,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.SoftwareVersion,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.UniqueSaleNumber,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.UniqueSaleNumber,
                                ESettingGroups.App,
                                "1")
                        },
                        {
                            ESettingKeys.OnlineShopNumber,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.OnlineShopNumber,
                                ESettingGroups.App,
                                string.Empty)
                        },
                        {
                            ESettingKeys.OnlineShopType,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.OnlineShopType,
                                ESettingGroups.App,
                                "-1")
                        },
                        {
                            ESettingKeys.OnlineShopDomainName,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.OnlineShopDomainName,
                                ESettingGroups.App,
                                string.Empty)
                        },
                    };
                }

                return this.appSettings;
            }
        }

        /// <summary>
        /// Gets settings of an Axis-cloud.
        /// </summary>
        /// <date>16/03/2022.</date>
        public Dictionary<ESettingKeys, SettingsItemModel> AxisCloudSettings
        {
            get
            {
                if (this.axisCloudSettings == null)
                {
                    this.axisCloudSettings = new Dictionary<ESettingKeys, SettingsItemModel>()
                    {
                        {
                            ESettingKeys.DeviceIsUsed,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.DeviceIsUsed,
                                ESettingGroups.AxisCloud,
                                "false")
                        },
                        {
                            ESettingKeys.ComPort,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.ComPort,
                                ESettingGroups.AxisCloud,
                                "8734")
                        },
                        {
                            ESettingKeys.UserName,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.UserName,
                                ESettingGroups.AxisCloud,
                                string.Empty)
                        },
                        {
                            ESettingKeys.Password,
                            new SettingsItemModel(
                                this.settingsRepository,
                                ESettingKeys.Password,
                                ESettingGroups.AxisCloud,
                                string.Empty)
                        },
                    };
                }

                return this.axisCloudSettings;
            }
        }

        /// <summary>
        /// Gets or sets unique sale number (will be printed on the receipt).
        /// </summary>
        /// <date>16/03/2022.</date>
        public Microinvest.DeviceService.CustomTypes.UniqueSaleNumber UniqueSaleNumber
        {
            get => uniqueSaleNumber;
            set => this.uniqueSaleNumber = value;
        }

        /// <summary>
        /// Update settings in the database.
        /// </summary>
        /// <param name="settingsGroup">Group of settings to update.</param>
        /// <date>16/03/2022.</date>
        public void UpdateSettings(ESettingGroups settingsGroup)
        {
            switch (settingsGroup)
            {
                case ESettingGroups.App:
                    foreach (KeyValuePair<ESettingKeys, SettingsItemModel> keyValue in this.AppSettings)
                    {
                        keyValue.Value.UpdateData();
                    }

                    break;
                case ESettingGroups.FiscalPrinter:
                    foreach (KeyValuePair<ESettingKeys, SettingsItemModel> keyValue in this.FiscalPrinterSettings)
                    {
                        keyValue.Value.UpdateData();
                    }

                    break;
                case ESettingGroups.POSTerminal:
                    foreach (KeyValuePair<ESettingKeys, SettingsItemModel> keyValue in this.POSTerminalSettings)
                    {
                        keyValue.Value.UpdateData();
                    }

                    break;
                case ESettingGroups.COMScanner:
                    foreach (KeyValuePair<ESettingKeys, SettingsItemModel> keyValue in this.COMScannerSettings)
                    {
                        keyValue.Value.UpdateData();
                    }

                    break;
                case ESettingGroups.AxisCloud:
                    foreach (KeyValuePair<ESettingKeys, SettingsItemModel> keyValue in this.AxisCloudSettings)
                    {
                        keyValue.Value.UpdateData();
                    }

                    break;
            }
        }
    }
}
