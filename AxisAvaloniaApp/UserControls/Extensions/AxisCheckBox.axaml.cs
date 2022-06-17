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
    public partial class AxisCheckBox : CheckBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(CheckBox);
        private ITranslationService translationService;
        private IExplanationService explanationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisCheckBox"/> class.
        /// </summary>
        public AxisCheckBox()
        {
            InitializeComponent();

            this.Background = Avalonia.Media.Brushes.Transparent;

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
        }

        public static readonly StyledProperty<string> ExplanationKeyProperty =
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(ExplanationKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search explanation for this TextBox in the dictionary.
        /// </summary>
        /// <date>14.06.2022.</date>
        public string ExplanationKey
        {
            get => GetValue(ExplanationKeyProperty);
            set => SetValue(ExplanationKeyProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty =
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Gets or sets key to search checkbox content in the dictionary.
        /// </summary>
        /// <date>27.05.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set
            {
                SetValue(LocalizeTextKeyProperty, value);
            }

        }

        /// <summary>
        /// Subscribes to LanguageChanged event when the AxisCheckBox is added to a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            this.translationService.LanguageChanged += Localize;
            base.OnAttachedToVisualTree(e);
        }

        /// <summary>
        /// Unsubscribes for LanguageChanged event when the AxisCheckBox is removed from a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            this.translationService.LanguageChanged -= Localize;
            base.OnDetachedFromVisualTree(e);
        }

        /// <summary>
        /// Invoke "Localize" method if LocalizeTextKey was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>27.05.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(LocalizeTextKey):
                    Localize();
                    break;
            }
            base.OnPropertyChanged(change);
        }

        /// <summary>
        /// Invoke "Localize" method when AxisCheckBox is initialized.
        /// </summary>
        /// <date>27.05.2022.</date>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Localize();
        }

        /// <summary>
        /// Set string with explanation of a purpose of a CheckBox during pointe enter to the CheckBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>14.06.2022.</date>
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);

            if (!string.IsNullOrEmpty(ExplanationKey))
            {
                explanationService.ExplanationStr = translationService.GetExplanation(this.ExplanationKey);
            }
        }

        /// <summary>
        /// Clear string with explanation of a purpose of a CheckBox during pointer leave from the CheckBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>14.06.2022.</date>
        protected override void OnPointerLeave(PointerEventArgs e)
        {
            base.OnPointerLeave(e);

            explanationService.ExplanationStr = string.Empty;
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        /// <date>27.05.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        ///  Sets localized text if LocalizeTextKey was changed.
        /// </summary>
        /// <date>12.05.2022.</date>
        private void Localize()
        {
            if (!string.IsNullOrEmpty(this.LocalizeTextKey))
            {
                this.Content = translationService.Localize(this.LocalizeTextKey);
            }
        }
    }
}
