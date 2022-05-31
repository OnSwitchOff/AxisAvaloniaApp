using Avalonia;
using Avalonia.Controls.Primitives;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using Microinvest.CommonLibrary.Enums;
using System.Windows.Input;

namespace AxisAvaloniaApp.UserControls.Buttons
{
    public class PaymentButton : TemplatedControl
    {
        private ITranslationService translationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentButton"/> class.
        /// </summary>
        public PaymentButton()
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            translationService.LanguageChanged += Localize;
        }

        public static readonly StyledProperty<ICommand> ButtonClickProperty =
           AvaloniaProperty.Register<PaymentButton, ICommand>(nameof(ButtonClick));

        /// <summary>
        /// Gets or sets a command to invoke when button was pressed.
        /// </summary>
        /// <date>30.05.2022.</date>
        public ICommand ButtonClick
        {
            get => GetValue(ButtonClickProperty);
            set => SetValue(ButtonClickProperty, value);
        }

        public static readonly StyledProperty<EPaymentTypes> PaymentTypeProperty =
           AvaloniaProperty.Register<PaymentButton, EPaymentTypes>(nameof(PaymentType));

        /// <summary>
        /// Gets or sets type of payment.
        /// </summary>
        /// <date>30.05.2022.</date>
        public EPaymentTypes PaymentType
        {
            get => GetValue(PaymentTypeProperty);
            set => SetValue(PaymentTypeProperty, value);
        }

        public static readonly StyledProperty<string> ImagePathProperty =
           AvaloniaProperty.Register<PaymentButton, string>(nameof(ImagePath));

        /// <summary>
        /// Gets or sets path to image of button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string ImagePath
        {
            get => GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty =
            AvaloniaProperty.Register<PaymentButton, string>(nameof(LocalizeTextKey));

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
           AvaloniaProperty.Register<PaymentButton, string>(nameof(Text));

        /// <summary>
        /// Gets or sets text of the button.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
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
