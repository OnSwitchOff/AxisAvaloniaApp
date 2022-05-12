using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Services.Translation
{
    /// <summary>
    /// Gets tools to localize the application.
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        /// LanguageChanged event.
        /// </summary>
        /// <date>12.04.2022.</date>
        event Action LanguageChanged;

        /// <summary>
        /// Gets languages suppotred by the application.
        /// </summary>
        /// <date>12.04.2022.</date>
        Dictionary<string, string> SupportedLanguages { get; }

        /// <summary>
        /// Gets localized string in according by key.
        /// </summary>
        /// <param name="key">Key to get localized string.</param>
        /// <returns>Localized string.</returns>
        /// <date>12.04.2022.</date>
        string Localize(string key);

        /// <summary>
        /// Gets localized explanation string in according by key.
        /// </summary>
        /// <param name="key">Key to get explanation string.</param>
        /// <returns>Explanation string.</returns>
        /// <date>12.04.2022.</date>
        string GetExplanation(string key);

        /// <summary>
        /// Gets localized wish string in according by key.
        /// </summary>
        /// <param name="key">Key to get wish string.</param>
        /// <returns>Wish string.</returns>
        /// <date>12.04.2022.</date>
        string GetWish(string key);

        /// <summary>
        /// Initialize dictionary with values to localize the application.
        /// </summary>
        /// <param name="languageCode">Combine code of item of Elanguages to fill dictionaries.</param>
        /// <date>12.04.2022.</date>
        void InitializeDictionary(string languageCode);
    }
}
