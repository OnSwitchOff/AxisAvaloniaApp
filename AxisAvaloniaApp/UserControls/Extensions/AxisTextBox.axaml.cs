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
    }
}
