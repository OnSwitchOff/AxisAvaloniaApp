using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.ViewModels;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Views
{
    public partial class SplashScreenView : Window
    {

        private bool? dialogResult;
        public bool? DialogResult
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

        public SplashScreenView()
        {
            dialogResult = null;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SplashScreenViewModel dc = new SplashScreenViewModel(false);
            DataContext = dc;
            //dc.PropertyChanged += Dc_PropertyChanged;
        }


        public SplashScreenView(bool isFirstStart)
        {
            dialogResult = null;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SplashScreenViewModel dc = new SplashScreenViewModel(isFirstStart);
            DataContext = dc;
            dc.PropertyChanged += Dc_PropertyChanged;
        }

        private void Dc_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Progress" && ((SplashScreenViewModel)DataContext).Progress == 100)
            {
                this.DialogResult = true;
            }
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
