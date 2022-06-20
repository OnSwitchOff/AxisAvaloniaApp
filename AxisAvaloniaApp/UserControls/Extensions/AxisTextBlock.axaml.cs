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
        Type IStyleable.StyleKey => typeof(TextBlock);
        private ITranslationService translationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisTextBlock"/> class.
        /// </summary>
        public AxisTextBlock()
        {
            InitializeComponent();

            this.Background = Avalonia.Media.Brushes.Transparent;

            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.translationService.LanguageChanged += Localize;
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty = 
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Gets or sets key to search label text in the dictionary.
        /// </summary>
        /// <date>12.05.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set => SetValue(LocalizeTextKeyProperty, value);
        }

        /// <summary>
        /// Subscribes to LanguageChanged event when the AxisTextBlock is added to a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            //this.translationService.LanguageChanged += Localize;
            base.OnAttachedToVisualTree(e);
        }

        /// <summary>
        /// Unsubscribes for LanguageChanged event when the AxisTextBlock is removed from a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            //this.translationService.LanguageChanged -= Localize;
            base.OnDetachedFromVisualTree(e);
        }

        /// <summary>
        /// Invoke "Localize" method if LocalizeTextKey was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>25.05.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {            
            base.OnPropertyChanged(change);

            switch (change.Property.Name)
            {
                case nameof(LocalizeTextKey):
                    Localize();
                    break;
            }
        }

        /// <summary>
        /// Invoke "Localize" method when AxisTextBlock is initialized.
        /// </summary>
        /// <date>12.05.2022.</date>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Localize();
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        /// <date>12.05.2022.</date>
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
                if (this.LocalizeTextKey.StartsWith("COM"))
                {
                    this.Text = this.LocalizeTextKey;
                }
                else
                {
                    this.Text = translationService.Localize(this.LocalizeTextKey).TrimStart('\n');
                }
            }
        }
    }
}
