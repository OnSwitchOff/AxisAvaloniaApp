using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.NavigationView;

namespace AxisAvaloniaApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly ITranslationService translationService;
        private readonly IExplanationService explanationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.MainWindowViewModel>();
            Sale.PointerPressed += MenuItem_PointerPressed;

            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
        }

        /// <summary>
        /// Remove explanation data if pointer leave from NavigationViewItem. 
        /// </summary>
        /// <param name="sender">NavigationViewItem.</param>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>25.02.2022.</date>
        private void MenuItem_PointerLeave(object? sender, PointerEventArgs e)
        {
            explanationService.ExplanationStr = string.Empty;
        }

        /// <summary>
        /// Show explanation data to user if pointer enter to NavigationViewItem. 
        /// </summary>
        /// <param name="sender">NavigationViewItem.</param>
        /// <param name="e">PointerEventArgs.</param>
        /// <date>25.02.2022.</date>
        private void MenuItem_PointerEnter(object? sender, PointerEventArgs e)
        {
            if (sender is NavigationViewItem viewItem)
            {
                explanationService.ExplanationStr = translationService.GetExplanation(viewItem.GetValue(NavigationExtensions.ExplanationKeyProperty));
            }
        }

        /// <summary>
        /// Set SelectedItem of ListBox if pointer pressed to NavigationViewItem.
        /// </summary>
        /// <param name="sender">NavigationViewItem.</param>
        /// <param name="e">RoutedEventArgs.</param>
        /// <remarks>This is to fix some bug if the SelectedItem from the ListBox is set to null manually.</remarks>
        /// <date>25.02.2022.</date>
        private void MenuItem_PointerPressed(object? sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowViewModel viewModel)
            {
                viewModel.IsPointerPressedActivation = true;
                if (sender is NavigationViewItem item)
                {
                    viewModel.SelectedItem = item;
                }
            }
        }
    }
}
