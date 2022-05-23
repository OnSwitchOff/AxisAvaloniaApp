namespace AxisAvaloniaApp.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Get IControl to show to user.
        /// </summary>
        /// <param name="viewModel">ViewModel for the next control.</param>
        /// <param name="control">Current control to cache data.</param>
        /// <returns>IControl.</returns>
        /// <date>20.05.2022.</date>
        Avalonia.Controls.IControl NavigateTo(
            string viewModel,
            Avalonia.Controls.IControl control);
    }
}
