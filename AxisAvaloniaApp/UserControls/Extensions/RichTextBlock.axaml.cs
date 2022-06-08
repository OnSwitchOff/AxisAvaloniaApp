using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class RichTextBlock : AxisTextBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBlock"/> class.
        /// </summary>
        public RichTextBlock()
        {
            InitializeComponent();

            TextWrapping = Avalonia.Media.TextWrapping.WrapWithOverflow;
        }

        public static readonly StyledProperty<string> OverflowTextProperty =
            AvaloniaProperty.Register<AxisTextBlock, string>(nameof(OverflowText), string.Empty);

        /// <summary>
        /// Gets or sets text that overflows.
        /// </summary>
        /// <date>08.06.2022.</date>
        public string OverflowText
        {
            get => GetValue(OverflowTextProperty);
            set => SetValue(OverflowTextProperty, value);
        }

        /// <summary>
        /// Subscribes to PropertyChanged event of parent when RichTextBlock is attached to visual tree;
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>08.06.2022.</date>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (this.Parent != null)
            {
                this.Parent.PropertyChanged += Parent_PropertyChanged;
            }
        }

        /// <summary>
        /// Sets "Overflow" if size of parent is changing.
        /// </summary>
        /// <typeparam name="sender">Control that was changed.</typeparam>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>08.06.2022.</date>
        private void Parent_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(Control.Bounds):
                    int runsLenght = 0;

                    foreach (var line in this.TextLayout.TextLines)
                    {
                        runsLenght += line.TextRange.Length;
                    }

                    OverflowText = this.Text.Substring(runsLenght);
                    break;
            }
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        /// <date>08.06.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
