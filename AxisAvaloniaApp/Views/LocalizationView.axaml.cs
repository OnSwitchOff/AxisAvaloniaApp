using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.ViewModels;

namespace AxisAvaloniaApp.Views
{
    public partial class LocalizationView : AbstractResultableWindow
    {
        private bool? dialogResult;
        public override bool? DialogResult
        {
            get => dialogResult;
            set
            {
                dialogResult = value;
                if (value != null)
                {
                    this.Close();
                }
            }
        }

        public LocalizationView()
        {
            dialogResult = null;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new LocalizationViewModel();
        }



        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}
