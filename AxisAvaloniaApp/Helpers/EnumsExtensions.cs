using Common.Enums;
using Microinvest.CommonLibrary.CustomTypes;
using Microinvest.CommonLibrary.Enums;
using Microinvest.DeviceService.Enums;
using System;

namespace AxisAvaloniaApp.Helpers
{
    public static class EnumsExtensions
    {
        /// <summary>
        /// Converts languages enum of the current app to languages enum of the PrinterService.
        /// </summary>
        /// <param name="language">Language of the current app.</param>
        /// <returns>Language of the PrinterService.</returns>
        /// <date>17.03.2022.</date>
        public static SupportedLanguagesEnum Convert(this Microinvest.CommonLibrary.Enums.ELanguages language)
        {
            if (Enum.IsDefined(typeof(SupportedLanguagesEnum), language.ToString()))
            {
                return (SupportedLanguagesEnum)Enum.Parse(typeof(SupportedLanguagesEnum), language.ToString());
            }
            else
            {
                return SupportedLanguagesEnum.Bulgarian;
            }
        }

        /// <summary>
        /// Converts countries enum of the current app to countries enum of the PrinterService.
        /// </summary>
        /// <param name="country">Country of the current app.</param>
        /// <returns>Country of the PrinterService.</returns>
        /// <exception cref="Exception">Throw exception if country of the current app is not supported by PrinterService.</exception>
        /// <date>15.06.2022.</date>
        public static SupportedCountriesEnum Convert(this Microinvest.CommonLibrary.Enums.ECountries country)
        {
            if (Enum.IsDefined(typeof(SupportedCountriesEnum), country.ToString()))
            {
                return (SupportedCountriesEnum)Enum.Parse(typeof(SupportedCountriesEnum), country.ToString());
            }

            throw new Exception(string.Format("{0} is not supported by Fisco dll!", country));
        }

        /// <summary>
        /// Converts EECCheckTypes enum to EECRReceiptType enum.
        /// </summary>
        /// <param name="receiptType">Type of receipt to convert.</param>
        /// <returns>EECRReceiptType.</returns>
        /// <date>23.06.2022.</date>
        public static EECCheckTypes Convert(this EECRReceiptType receiptType)
        {
            switch (receiptType)
            {
                case Microinvest.DeviceService.Enums.EECRReceiptType.FiscalReceipt:
                    return EECCheckTypes.FiscalCheck;
                case Microinvest.DeviceService.Enums.EECRReceiptType.NonFiscalReceipt:
                    return EECCheckTypes.NonFiscalCheck;
                case Microinvest.DeviceService.Enums.EECRReceiptType.ReturnReceipt:
                    return EECCheckTypes.ReturnCheck;
                case Microinvest.DeviceService.Enums.EECRReceiptType.DuplicateReceipt:
                    return EECCheckTypes.DuplicateCheck;
                default:
                    return EECCheckTypes.Unknown;
            }
        }

        /// <summary>
        /// Converts currency code to ECurrencies enum.
        /// </summary>
        /// <param name="currencyCode">Currency code.</param>
        /// <returns>ECurrencies</returns>
        /// <exception cref="Exception">Throws Exception if currency code is not included to ECurrencies.</exception>
        /// <date>06.07.2022.</date>
        public static ECurrencies Convert(this string currencyCode)
        {
            switch (currencyCode)
            {
                case "BGN":
                    return ECurrencies.BGN;
                case "USD":
                    return ECurrencies.USD;
                case "RUB":
                    return ECurrencies.RUB;
                case "UAH":
                    return ECurrencies.UAH;
                default:
                    throw new Exception(String.Format("The currency \"{0}\" is not supported", currencyCode));
            }
        }

        /// <summary>
        /// Convert XmlSerializerEECCheckType object to EECRReceiptType enum
        /// </summary>
        /// <param name="checkType">Value to convert</param>
        /// <returns>EECRReceiptType</returns>
        /// <developer>Serhii Rozniuk</developer>
        /// <date>21.07.2022.</date>
        public static EECRReceiptType Convert(this XmlSerializerEECCheckType checkType)
        {
            switch ((EECCheckTypes)checkType)
            {
                case EECCheckTypes.FiscalCheck:
                    return EECRReceiptType.FiscalReceipt;
                case EECCheckTypes.NonFiscalCheck:
                    return EECRReceiptType.NonFiscalReceipt;
                case EECCheckTypes.ReturnCheck:
                    return EECRReceiptType.ReturnReceipt;
                case EECCheckTypes.DuplicateCheck:
                    return EECRReceiptType.DuplicateReceipt;
                default:
                    return EECRReceiptType.Unknown;
            }
        }
    }
}
