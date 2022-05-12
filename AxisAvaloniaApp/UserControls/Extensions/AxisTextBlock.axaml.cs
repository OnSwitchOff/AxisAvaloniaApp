using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using System;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class AxisTextBlock : TextBlock, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ComboBox);
        private ITranslationService translationService;

        public AxisTextBlock()
        {
            InitializeComponent();

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.translationService.LanguageChanged += Localize;
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty = 
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Key to search label text in the dictionary
        /// </summary>
        /// <date>12.05.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set => SetValue(LocalizeTextKeyProperty, value);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Localize();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Localize()
        {
            if (!string.IsNullOrEmpty(this.LocalizeTextKey))
            {
                this.Text = translationService.Localize(this.LocalizeTextKey);
            }
        }
    }
}
