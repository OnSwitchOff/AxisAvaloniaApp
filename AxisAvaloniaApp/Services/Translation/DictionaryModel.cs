using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace AxisAvaloniaApp.Services.Translation
{
    /// <summary>
    /// Describes structure of xml dictionary.
    /// </summary>
    [Serializable]
    public class DictionaryModel
    {
        /// <summary>
        /// Root node.
        /// </summary>
        [XmlRoot("TranslationDictionaries")]
        public class RootClass
        {
            /// <summary>
            /// Gets or sets collections of availables dictionaries.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlElement("Dictionary")]
            public Collection<Dict> Dictionaries { get; set; }

            /// <summary>
            /// Gets or sets additional attribures.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyAttribute]
            public XmlAttribute[] OtherAttributes { get; set; }

            /// <summary>
            /// Gets or sets additional elements.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyElement]
            public XmlElement[] OtherElements { get; set; }
        }

        /// <summary>
        /// Describes data of a dictionary.
        /// </summary>
        public class Dict
        {
            /// <summary>
            /// Gets or sets data of the application.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAttribute("Product")]
            public string Product { get; set; }

            /// <summary>
            /// Gets or sets version of the application.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAttribute("Version")]
            public string Version { get; set; }

            /// <summary>
            /// Gets or sets translation of the supported languages.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlArray("Languages")]
            [XmlArrayItem("Language")]
            public Collection<Language> Languages { get; set; }

            /// <summary>
            /// Gets or sets list with translation data.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlElement("Data")]
            public Collection<Data> Data { get; set; }
        }

        /// <summary>
        /// Describes data of the supported language.
        /// </summary>
        public class Language
        {
            /// <summary>
            /// Gets or sets name of language.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAttribute("DisplayName")]
            public string DisplayName { get; set; }

            /// <summary>
            /// Gets or sets key of language.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlText]
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets additional attribures.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyAttribute]
            public XmlAttribute[] OtherAttributes { get; set; }

            /// <summary>
            /// Gets or sets additional elements.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyElement]
            public XmlElement[] OtherElements { get; set; }
        }

        /// <summary>
        /// Describes translation record.
        /// </summary>
        public class Data
        {
            /// <summary>
            /// Gets or sets key of translation.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAttribute("Key")]
            public string Key { get; set; }

            /// <summary>
            /// Gets or sets value of translation.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlElement("Value")]
            public Collection<Value> Values { get; set; }
        }

        /// <summary>
        /// Describes value of translation.
        /// </summary>
        public class Value
        {
            /// <summary>
            /// Gets or sets language of translation.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAttribute("Lang")]
            public string Lang { get; set; }

            /// <summary>
            /// Gets or sets value of translation for the current language.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlText]
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets additional attribures.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyAttribute]
            public XmlAttribute[] OtherAttributes { get; set; }

            /// <summary>
            /// Gets or sets additional elements.
            /// </summary>
            /// <date>12.04.2022.</date>
            [XmlAnyElement]
            public XmlElement[] OtherElements { get; set; }
        }
    }
}
