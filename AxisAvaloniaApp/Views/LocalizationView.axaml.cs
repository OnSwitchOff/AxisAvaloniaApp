using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.ViewModels;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Views
{
    public partial class LocalizationView : Window
    {
        private bool? dialogResult;
        public  bool? DialogResult
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

        //private MainWindow mainWindow = null;

        public LocalizationView()
        {
            dialogResult = null;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new LocalizationViewModel();
            //this.Closed += LocalizationView_Closed;
        }

        private void LocalizationView_Closed(object? sender, System.EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private TaskCompletionSource<SplashScreenView> taskSource;
        public async Task<SplashScreenView> MyShowDialog()
        {
            taskSource = new TaskCompletionSource<SplashScreenView>();
            this.Closed += delegate
            {
                if (DialogResult != null)
                {
                    SplashScreenView splashScreenView = new SplashScreenView(true);
                    splashScreenView.Show();
                    taskSource.TrySetResult(splashScreenView);
                }
                else
                {
                    System.Environment.Exit(0);
                }                         
            };
            this.Show();
            return await taskSource.Task;
        }
    }
}
