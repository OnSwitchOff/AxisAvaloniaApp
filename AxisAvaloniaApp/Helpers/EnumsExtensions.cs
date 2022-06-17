using Common.Enums;
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
    }
}
