using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AxisAvaloniaApp.UserControls.GroupBox
{
    public class GroupBox : TemplatedControl
    {
        public static new readonly StyledProperty<Thickness> BorderThicknessProperty =
            AvaloniaProperty.Register<GroupBox, Thickness>(nameof(BorderThickness), new Thickness(1));

        /// <summary>
        /// Gets or sets thickness of the border.
        /// </summary>
        /// <date>06.06.2022.</date>
        public new Thickness BorderThickness
        {
            get => GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static new readonly StyledProperty<IBrush> BorderBrushProperty =
            AvaloniaProperty.Register<GroupBox, IBrush>(nameof(BorderBrush), Brushes.White);

        /// <summary>
        /// Gets or sets color of the border.
        /// </summary>
        /// <date>06.06.2022.</date>
        public new IBrush BorderBrush
        {
            get => GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public static readonly StyledProperty<IBrush> ParentBackgroundProperty =
            AvaloniaProperty.Register<GroupBox, IBrush>(nameof(ParentBackground), (SolidColorBrush)new BrushConverter().ConvertFrom("#FF1B7E83"));

        /// <summary>
        /// Gets or sets backgroung of the parent control.
        /// </summary>
        /// <remarks>It's needed to hide border under text of the GroupBox header.</remarks>
        /// <date>06.06.2022.</date>
        public IBrush ParentBackground
        {
            get => GetValue(ParentBackgroundProperty);
            set => SetValue(ParentBackgroundProperty, value);
        }

        public static readonly StyledProperty<string> LocalizeTextKeyProperty =
            AvaloniaProperty.Register<GroupBox, string>(nameof(LocalizeTextKey));

        /// <summary>
        /// Gets or sets key to search text of the GroupBox header in the dictionary.
        /// </summary>
        /// <date>06.06.2022.</date>
        public string LocalizeTextKey
        {
            get => GetValue(LocalizeTextKeyProperty);
            set => SetValue(LocalizeTextKeyProperty, value);
        }

        public static readonly StyledProperty<string> HeaderProperty =
            AvaloniaProperty.Register<GroupBox, string>(nameof(Header), string.Empty);

        /// <summary>
        /// Gets or sets text of the GroupBox header.
        /// </summary>
        /// <date>06.06.2022.</date>
        public string Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly StyledProperty<Thickness> HeaderMarginProperty =
           AvaloniaProperty.Register<GroupBox, Thickness>(nameof(HeaderMargin), new Thickness(5, 0, 5, 0));

        /// <summary>
        /// Gets or sets margin of the TextBlock with header.
        /// </summary>
        /// <date>06.06.2022.</date>
        public Thickness HeaderMargin
        {
            get => GetValue(HeaderMarginProperty);
            set => SetValue(HeaderMarginProperty, value);
        }

        public static readonly StyledProperty<object> ContentProperty =
           AvaloniaProperty.Register<GroupBox, object>(nameof(Content));

        /// <summary>
        /// Gets or sets content of the GroupBox.
        /// </summary>
        /// <date>06.06.2022.</date>
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly StyledProperty<Thickness> ContentMarginProperty =
           AvaloniaProperty.Register<GroupBox, Thickness>(nameof(ContentMargin), new Thickness(0));

        /// <summary>
        /// Gets or sets margin of a content of the GroupBox.
        /// </summary>
        /// <date>06.06.2022.</date>
        private Thickness ContentMargin
        {
            get => GetValue(ContentMarginProperty);
            set => SetValue(ContentMarginProperty, value);
        }

        /// <summary>
        /// Updates dependents properties if main property was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">History of changing of property.</param>
        /// <date>06.06.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(Padding):
                    ContentMargin = new Thickness(0, Padding.Top, 0, 0);
                    break;
            }

            base.OnPropertyChanged(change);
        }
    }
}
