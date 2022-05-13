using System;
using System.Collections.Generic;
using static AxisAvaloniaApp.Services.Translation.DictionaryModel;

namespace AxisAvaloniaApp.Services.Translation
{
    /// <summary>
    /// Gets tools to localize the application.
    /// </summary>
    public class TranslationService : ITranslationService
    {
        private Dictionary<string, string> mainDictionary;
        private Dictionary<string, string> helpDictionary;
        private Dictionary<int, string> wishesDictionary;

        public TranslationService(Settings.ISettingsService settings)
        {
            this.mainDictionary = new Dictionary<string, string>();
            this.helpDictionary = new Dictionary<string, string>();
            this.wishesDictionary = new Dictionary<int, string>();
            this.SupportedLanguages = new Dictionary<string, string>();

            InitializeDictionary(settings.AppLanguage.CombineCode);
        }

        /// <summary>
        /// Gets languages suppotred by the application.
        /// </summary>
        /// <date>12.04.2022.</date>
        public Dictionary<string, string> SupportedLanguages { get; private set; }

        /// <summary>
        /// LanguageChanged event.
        /// </summary>
        /// <date>12.04.2022.</date>
        public event Action LanguageChanged;

        /// <summary>
        /// Gets localized string in according by key.
        /// </summary>
        /// <param name="key">Key to get localized string.</param>
        /// <returns>Localized string.</returns>
        /// <date>12.04.2022.</date>
        public string Localize(string key)
        {
            if (this.mainDictionary.ContainsKey(key))
            {
                return this.mainDictionary[key];
            }
            else
            {
                return string.Format("There is no translation for the keyword \"{0}\"!", key);
            }
        }

        /// <summary>
        /// Gets localized explanation string in according by key.
        /// </summary>
        /// <param name="key">Key to get explanation string.</param>
        /// <returns>Explanation string.</returns>
        /// <date>12.04.2022.</date>
        public string GetExplanation(string key)
        {
            if (this.helpDictionary.ContainsKey(key))
            {
                return this.helpDictionary[key];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets localized wish string in according by key.
        /// </summary>
        /// <param name="key">Key to get wish string.</param>
        /// <returns>Wish string.</returns>
        /// <date>12.04.2022.</date>
        public string GetWish(string key)
        {
            return this.wishesDictionary[new Random().Next(1, this.wishesDictionary.Count) - 1];
        }

        /// <summary>
        /// Initialize dictionary with values to localize the application.
        /// </summary>
        /// <param name="languageCode">Combine code of item of Elanguages to fill dictionaries.</param>
        /// <date>12.04.2022.</date>
        public void InitializeDictionary(string languageCode)
        {
            bool firstInitialize = false;
            if (this.SupportedLanguages.Count == 0)
            {
                firstInitialize = true;
            }

            this.mainDictionary.Clear();
            this.helpDictionary.Clear();
            this.wishesDictionary.Clear();
            this.SupportedLanguages.Clear();

            RootClass? fullDictionary = Helpers.XmlExtensions.DeserializeData<RootClass>(Resources.Dictionary);
            if (fullDictionary != null)
            {
                foreach (Dict dict in fullDictionary.Dictionaries)
                {
                    if (dict.Product == "Axis My100R")
                    {
                        foreach (Language lang in dict.Languages)
                        {
                            this.SupportedLanguages.Add(lang.Text, lang.DisplayName);
                        }

                        foreach (Data data in dict.Data)
                        {
                            foreach (Value value in data.Values)
                            {
                                if (value.Lang.ToUpper().Equals(languageCode.ToUpper()) && !this.mainDictionary.ContainsKey(data.Key))
                                {
                                    this.mainDictionary.Add(data.Key, value.Text);
                                    break;
                                }
                            }
                        }
                    }

                    if (dict.Product == "Axis My100R Help")
                    {
                        foreach (Data data in dict.Data)
                        {
                            foreach (Value value in data.Values)
                            {
                                if (value.Lang.ToUpper().Equals(languageCode.ToUpper()) && !this.helpDictionary.ContainsKey(data.Key))
                                {
                                    this.helpDictionary.Add(data.Key, value.Text);
                                    break;
                                }
                            }
                        }
                    }

                    if (dict.Product == "Axis My100R RandomDictionary")
                    {
                        foreach (Data data in dict.Data)
                        {
                            foreach (Value value in data.Values)
                            {
                                int wishKey = int.Parse(data.Key);
                                if (value.Lang.ToUpper().Equals(languageCode.ToUpper()) && !this.wishesDictionary.ContainsKey(wishKey))
                                {
                                    this.wishesDictionary.Add(wishKey, value.Text);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (!firstInitialize)
            {
                if (this.LanguageChanged != null)
                {
                    this.LanguageChanged.Invoke();
                }
            }
        }
    }
}
