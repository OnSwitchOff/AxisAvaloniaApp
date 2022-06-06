using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Translation;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class AxisComboBox : ComboBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ComboBox);
        private ITranslationService translationService;
        private IExplanationService explanationService;
        private object? selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisTextBox"/> class.
        /// </summary>
        public AxisComboBox()
        {
            InitializeComponent();

            Background = Avalonia.Media.Brushes.White;

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();

            selectedItem = null;
        }

        public static readonly StyledProperty<string> ExplanationKeyProperty =
            AvaloniaProperty.Register<AxisComboBox, string>(nameof(ExplanationKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search explanation for this ComboBox in the dictionary.
        /// </summary>
        /// <date>01.06.2022.</date>
        public string ExplanationKey
        {
            get => GetValue(ExplanationKeyProperty);
            set => SetValue(ExplanationKeyProperty, value);
        }

        public static readonly StyledProperty<bool> IsInputTextProperty =
           AvaloniaProperty.Register<AxisComboBox, bool>(nameof(IsInputText), false);

        /// <summary>
        /// Gets or sets value indicating whether user can to input data to ComboBox.
        /// </summary>
        /// <date>01.06.2022.</date>
        public bool IsInputText
        {
            get => GetValue(IsInputTextProperty);
            set => SetValue(IsInputTextProperty, value);
        }

        /// <summary>
        /// Set string with explanation of a purpose of a ComboBox during pointe enter to the ComboBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>01.06.2022.</date>
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);

            if (!string.IsNullOrEmpty(ExplanationKey))
            {
                explanationService.ExplanationStr = translationService.GetExplanation(this.ExplanationKey);
            }
        }

        /// <summary>
        /// Clear string with explanation of a purpose of a ComboBox during pointer leave from the ComboBox.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>01.06.2022.</date>
        protected override void OnPointerLeave(PointerEventArgs e)
        {
            base.OnPointerLeave(e);

            explanationService.ExplanationStr = string.Empty;
        }

        /// <summary>
        /// Set placeholder text if text is inputing and "IsInputText" property is active.
        /// </summary>
        /// <param name="e">TextInputEventArgs</param>
        /// <date>01.06.2022.</date>
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);

            if (IsInputText)
            {
                if (this.SelectedItem != null)
                {
                    selectedItem = this.SelectedItem;
                    this.SelectedItem = null;
                    this.PlaceholderText = string.Empty;
                }

                this.PlaceholderText += e.Text;
            }
        }

        /// <summary>
        /// Update list with Items or placeholder text in according to pressing key if "IsInputText" property is active.
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        /// <date>01.06.2022.</date>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (IsInputText && !string.IsNullOrEmpty(this.PlaceholderText))
            {
                switch (e.Key)
                {
                    case Key.Enter:                        
                        this.Items = (this.Items as IEnumerable<object>).Add(this.PlaceholderText);
                        this.SelectedIndex = this.ItemCount - 1;
                        selectedItem = null;
                        break;
                    case Key.Back:
                        this.PlaceholderText = this.PlaceholderText.Substring(0, this.PlaceholderText.Length - 1);
                        break;
                }
            }

            e.Handled = e.Key == Key.Enter || e.Key == Key.Back;            
        }

        /// <summary>
        /// Restore SelectedItem if focus was lost.
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        /// <date>01.06.2022.</date>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (selectedItem != null)
            {
                this.SelectedItem = selectedItem;
            }
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        /// <date>01.06.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
