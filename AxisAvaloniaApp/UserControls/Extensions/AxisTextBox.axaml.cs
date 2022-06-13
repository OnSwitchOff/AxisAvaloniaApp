using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Validation;
using System;
using System.Reflection;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class AxisTextBox : TextBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(TextBox);
        private ITranslationService translationService;
        private IExplanationService explanationService;
        private IValidationService validationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisTextBox"/> class.
        /// </summary>
        public AxisTextBox()
        {
            InitializeComponent();
            Background = Avalonia.Media.Brushes.White;

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
            this.validationService = Splat.Locator.Current.GetRequiredService<IValidationService>();
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

        public static readonly StyledProperty<EInputDataCheckers> InputDataCheckerProperty =
           AvaloniaProperty.Register<AxisTextBlock, EInputDataCheckers>(nameof(InputDataChecker), EInputDataCheckers.AllData);

        /// <summary>
        /// Gets or sets parameter to validate input data.
        /// </summary>
        /// <date>10.06.2022.</date>
        public EInputDataCheckers InputDataChecker
        {
            get => GetValue(InputDataCheckerProperty);
            set => SetValue(InputDataCheckerProperty, value);
        }

        public static readonly StyledProperty<bool> IsErrorIconVisibleProperty =
            AvaloniaProperty.Register<AxisTextBlock, bool>(nameof(IsErrorIconVisible), false);

        /// <summary>
        /// Gets or sets value indicating whether icon of error is visible.
        /// </summary>
        /// <date>10.06.2022.</date>
        private bool IsErrorIconVisible
        {
            get => GetValue(IsErrorIconVisibleProperty);
            set => SetValue(IsErrorIconVisibleProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeErrorDescriptionKeyProperty =
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(LocalizeErrorDescriptionKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search description of error in the dictionary.
        /// </summary>
        /// <date>10.06.2022.</date>
        private string LocalizeErrorDescriptionKey
        {
            get => GetValue(LocalizeErrorDescriptionKeyProperty);
            set => SetValue(LocalizeErrorDescriptionKeyProperty, value);
        }

        /// <summary>
        /// Subscribes to LanguageChanged event when the AxisTextBox is added to a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            this.translationService.LanguageChanged += Localize;
            base.OnAttachedToVisualTree(e);
        }

        /// <summary>
        /// Unsubscribes for LanguageChanged event when the AxisTextBox is removed from a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            this.translationService.LanguageChanged -= Localize;
            base.OnDetachedFromVisualTree(e);
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
        /// Check input data.
        /// </summary>
        /// <param name="e">TextInputEventArgs.</param>
        /// <date>10.06.2022.</date>
        protected override void OnTextInput(TextInputEventArgs e)
        {
            if (e.Text != null)
            {
                switch (InputDataChecker)
                {
                    case EInputDataCheckers.OnlyDigits:
                        e.Handled = !validationService.IsDigit(e.Text[0]);
                        break;
                    case EInputDataCheckers.OnlyLetters:
                        e.Handled = !validationService.IsLetter(e.Text[0]);
                        break;
                    case EInputDataCheckers.OnlyLettersAndSpaces:
                        e.Handled = !validationService.IsLetterOrSpace(e.Text[0]);
                        break;
                    case EInputDataCheckers.OnlyDigitsAndLetters:
                        e.Handled = !(validationService.IsDigit(e.Text[0]) || validationService.IsLetter(e.Text[0]));
                        break;
                    case EInputDataCheckers.OnlyDigitsAndPoint:
                        e.Handled = !(validationService.IsDigit(e.Text[0]) || 
                            (e.Text[0].Equals('.') && !string.IsNullOrEmpty(this.Text) && this.Text.Replace(',', '.').IndexOf('.') == -1) || 
                            (e.Text[0].Equals(',') && !string.IsNullOrEmpty(this.Text) && this.Text.Replace('.', ',').IndexOf(',') == -1));
                        break;
                }
            }

            base.OnTextInput(e);
        }

        /// <summary>
        /// Shows icon and description of error when data is not valid.
        /// </summary>
        /// <typeparam name="T">type of valoniaProperty.</typeparam>
        /// <param name="property">AvaloniaProperty.</param>
        /// <param name="value">BindingValue.</param>
        /// <date>10.06.2022.</date>
        protected override void UpdateDataValidation<T>(AvaloniaProperty<T> property, BindingValue<T> value)
        {
            IsErrorIconVisible = value.Type == BindingValueType.DataValidationErrorWithFallback;

            if (value.HasError && value.Type == BindingValueType.DataValidationErrorWithFallback)
            {
                LocalizeErrorDescriptionKey = value.Error.Message;
                var ctor = value.GetType().GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(BindingValueType), typeof(T), typeof(Exception) });//.GetProperty("Error");
                if (ctor != null)
                {
                    value = (BindingValue<T>)ctor.Invoke(new object[] { value.Type, value.Value, new DataValidationException("") });
                }
            }

            base.UpdateDataValidation(property, value);
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
