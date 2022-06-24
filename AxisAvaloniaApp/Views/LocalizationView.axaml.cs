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


        private TaskCompletionSource<MainWindow> taskSource;
        public async Task<MainWindow> MyShowDialog()
        {
            taskSource = new TaskCompletionSource<MainWindow>();
            this.Closed += delegate
            {
                if (DialogResult != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    taskSource.TrySetResult(mainWindow);
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
