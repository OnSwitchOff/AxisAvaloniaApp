using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Translation;
using System;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class AxisTextBox : TextBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(TextBox);
        private ITranslationService translationService;
        private IExplanationService explanationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisTextBox"/> class.
        /// </summary>
        public AxisTextBox()
        {
            InitializeComponent();

            Background = Avalonia.Media.Brushes.White;

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.translationService.LanguageChanged += Localize;
            this.explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
        }

        public static readonly StyledProperty<string> ExplanationKeyProperty =
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(ExplanationKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search explanation for this TextBox in the dictionary.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string ExplanationKey
        {
            get => GetValue(ExplanationKeyProperty);
            set => SetValue(ExplanationKeyProperty, value);
        }

        public static readonly StyledProperty<string> LocalizePlaceholderKeyProperty =
           AvaloniaProperty.Register<AxisTextBlock, string>(nameof(LocalizePlaceholderKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search text of placeholder for this TextBox in the dictionary.
        /// </summary>
        /// <date>31.05.2022.</date>
        public string LocalizePlaceholderKey
        {
            get => GetValue(LocalizePlaceholderKeyProperty);
            set => SetValue(LocalizePlaceholderKeyProperty, value);
        }

        /// <summary>
        /// Invoke "Localize" method if LocalizePlaceholderKey was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>31.05.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(LocalizePlaceholderKey):
                    Localize();
                    break;
            }
            base.OnPropertyChanged(change);
        }

        /// <summary>
        /// Invoke "Localize" method when AxisTextBox is initialized.
        /// </summary>
        /// <date>31.05.2022.</date>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Localize();
        }

        /// <summary>
        /// Set string with explanation of a purpose of a TextBox during pointe enter to the TextBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>26.05.2022.</date>
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);

            if (!string.IsNullOrEmpty(ExplanationKey))
            {
                explanationService.ExplanationStr = translationService.GetExplanation(this.ExplanationKey);
            }
        }

        /// <summary>
        /// Clear string with explanation of a purpose of a TextBox during pointer leave from the TextBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>26.05.2022.</date>
        protected override void OnPointerLeave(PointerEventArgs e)
        {
            base.OnPointerLeave(e);

            explanationService.ExplanationStr = string.Empty;
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        /// <date>26.05.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        ///  Sets localized text if LocalizeTextKey was changed.
        /// </summary>
        /// <date>31.05.2022.</date>
        private void Localize()
        {
            if (!string.IsNullOrEmpty(this.LocalizePlaceholderKey))
            {
                this.Watermark = translationService.Localize(this.LocalizePlaceholderKey);
            }
        }
    }
}
