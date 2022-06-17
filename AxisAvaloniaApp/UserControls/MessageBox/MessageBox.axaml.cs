using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.ComponentModel;

namespace AxisAvaloniaApp.UserControls.MessageBox
{
    public partial class MessageBox : Window, ICloseable
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        public MessageBox(EMessageBoxStyles style)
        {
            this.SetStyle(style);

            InitializeComponent();
        }

        public static readonly StyledProperty<EButtonResults> ResultProperty =
            AvaloniaProperty.Register<MessageBox, EButtonResults>(nameof(Result), EButtonResults.None);

        /// <summary>
        /// Gets or sets choice of user.
        /// </summary>
        /// <date>14.06.2022.</date>
        public EButtonResults Result
        {
            get => GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }

        public static readonly StyledProperty<bool> ControlBoxProperty =
            AvaloniaProperty.Register<MessageBox, bool>(nameof(ControlBox), true);

        /// <summary>
        /// Gets or sets value indicating whether buttons to minimize, maximize and close are visible.
        /// </summary>
        /// <date>14.06.2022.</date>
        public bool ControlBox
        {
            get => GetValue(ControlBoxProperty);
            set => SetValue(ControlBoxProperty, value);
        }

        public static readonly StyledProperty<bool> IsReuseWindowProperty =
            AvaloniaProperty.Register<MessageBox, bool>(nameof(IsReuseWindow), false);

        /// <summary>
        /// Gets or sets value indicating whether the window can be used again (it is hidden when closed if IsReuseWindow equals "true").
        /// </summary>
        /// <date>14.06.2022.</date>
        public bool IsReuseWindow
        {
            get => GetValue(IsReuseWindowProperty);
            set => SetValue(IsReuseWindowProperty, value);
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(ControlBox):
                    if (!ControlBox)
                    {
                        ExtendClientAreaToDecorationsHint = true;
                        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
                        ExtendClientAreaTitleBarHeightHint = 0;
                    }
                    break;
            }

            base.OnPropertyChanged(change);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsReuseWindow)
            {
                e.Cancel = true;
                Hide();
            }

            base.OnClosing(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
