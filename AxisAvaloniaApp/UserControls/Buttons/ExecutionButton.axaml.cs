using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Translation;
using System.Windows.Input;

namespace AxisAvaloniaApp.UserControls.Buttons
{
    public class ExecutionButton : TemplatedControl
    {
        private ITranslationService translationService;
        private IExplanationService explanationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionButton"/> class.
        /// </summary>
        public ExecutionButton()
        {
            Foreground = Brushes.White;
            Background = Brushes.Transparent;
            BorderBrush = Brushes.White;

            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
            translationService.LanguageChanged += Localize;
        }

        public new static readonly StyledProperty<IBrush> ForegroundProperty =
           AvaloniaProperty.Register<ExecutionButton, IBrush>(nameof(Foreground));

        /// <summary>
        /// Gets or sets a foreground of the text of button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public new IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public new static readonly StyledProperty<IBrush> BackgroundProperty =
           AvaloniaProperty.Register<ExecutionButton, IBrush>(nameof(Background));

        /// <summary>
        /// Gets or sets a foreground of the text of button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public new IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public new static readonly StyledProperty<IBrush> BorderBrushProperty =
           AvaloniaProperty.Register<ExecutionButton, IBrush>(nameof(BorderBrush));

        /// <summary>
        /// Gets or sets a border brush of button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public new IBrush BorderBrush
        {
            get => GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly StyledProperty<ICommand> CommandProperty =
           AvaloniaProperty.Register<ExecutionButton, ICommand>(nameof(Command));

        /// <summary>
        /// Gets or sets a command to invoke when button was pressed.
        /// </summary>
        /// <date>30.05.2022.</date>
        public ICommand Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly StyledProperty<object> CommandParameterProperty =
           AvaloniaProperty.Register<ExecutionButton, object>(nameof(CommandParameter));

        /// <summary>
        /// Gets or sets a parameter of the command.
        /// </summary>
        /// <date>30.05.2022.</date>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty =
           AvaloniaProperty.Register<ExecutionButton, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Gets or sets key to search text of button in the dictionary.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set => SetValue(LocalizeTextKeyProperty, value);
        }

        public static readonly StyledProperty<string> TextProperty =
           AvaloniaProperty.Register<ExecutionButton, string>(nameof(Text));

        /// <summary>
        /// Gets or sets text of the button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly StyledProperty<string> ExplanationKeyProperty =
           AvaloniaProperty.Register<ExecutionButton, string>(nameof(ExplanationKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search explanation for this ExecutionButton in the dictionary.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string ExplanationKey
        {
            get => GetValue(ExplanationKeyProperty);
            set => SetValue(ExplanationKeyProperty, value);
        }

        /// <summary>
        /// Invoke "Localize" method if LocalizeTextKey was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>30.05.2022.</date>
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
        /// Set string with explanation of a purpose of a ExecutionButton during pointe enter to the ExecutionButton.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>30.05.2022.</date>
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);

            if (!string.IsNullOrEmpty(ExplanationKey))
            {
                explanationService.ExplanationStr = translationService.GetExplanation(this.ExplanationKey);
            }
        }

        /// <summary>
        /// Clear string with explanation of a purpose of a ExecutionButton during pointer leave from the ExecutionButton.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>30.05.2022.</date>
        protected override void OnPointerLeave(PointerEventArgs e)
        {
            base.OnPointerLeave(e);

            explanationService.ExplanationStr = string.Empty;
        }

        // <summary>
        /// Invoke "Localize" method when PaymentButton is initialized.
        /// </summary>
        /// <date>30.05.2022.</date>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Localize();
        }

        /// <summary>
        ///  Sets localized text if LocalizeTextKey was changed.
        /// </summary>
        /// <date>30.05.2022.</date>
        private void Localize()
        {
            if (!string.IsNullOrEmpty(this.LocalizeTextKey))
            {
                this.Text = translationService.Localize(this.LocalizeTextKey);
            }
        }
    }
}
