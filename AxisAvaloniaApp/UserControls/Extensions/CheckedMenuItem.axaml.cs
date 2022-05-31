using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using System;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class CheckedMenuItem : MenuItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(MenuItem);
        private readonly ITranslationService translationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckedMenuItem"/> class.
        /// </summary>
        public CheckedMenuItem()
        {
            InitializeComponent();

            IsChecked = false;
            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.translationService.LanguageChanged += Localize;
        }

        public static readonly StyledProperty<bool> IsCheckedProperty =
            AvaloniaProperty.Register<CheckedMenuItem, bool>(nameof(IsChecked));

        /// <summary>
        /// Gets or sets value indicating whether CheckBox is checked.
        /// </summary>
        /// <date>27.05.2022.</date>
        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);

        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty =
            AvaloniaProperty.Register<CheckedMenuItem, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Gets or sets key to search MenuItem header in the dictionary.
        /// </summary>
        /// <date>27.05.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set => SetValue(LocalizeTextKeyProperty, value);
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
        /// Invoke "Localize" method when CheckedMenuItem is initialized.
        /// </summary>
        /// <date>27.05.2022.</date>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Localize();
        }

        /// <summary>
        /// Change IsChecked property if CheckedMenuItem is pressed.
        /// </summary>
        /// <param name="e">PointerPressedEventArgs</param>
        /// <date>27.05.2022.</date>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            IsChecked = !IsChecked;
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
        /// <date>27.05.2022.</date>
        private void Localize()
        {
            if (!string.IsNullOrEmpty(this.LocalizeTextKey))
            {
                this.Header = translationService.Localize(this.LocalizeTextKey);
            }
        }
    }
}
