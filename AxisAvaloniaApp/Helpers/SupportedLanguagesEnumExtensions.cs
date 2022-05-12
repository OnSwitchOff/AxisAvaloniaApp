using Common.Enums;
using System;

namespace AxisAvaloniaApp.Helpers
{
    public static class SupportedLanguagesEnumExtensions
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
    }
}
