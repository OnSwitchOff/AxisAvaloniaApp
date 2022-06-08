using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.Views
{
    public partial class WelcomeView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeView"/> class.
        /// </summary>
        public WelcomeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
