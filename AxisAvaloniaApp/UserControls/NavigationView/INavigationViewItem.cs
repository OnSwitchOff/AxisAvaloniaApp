namespace AxisAvaloniaApp.UserControls.NavigationView
{
    public interface INavigationViewItem
    {
        /// <summary>
        /// Path to icon.
        /// </summary>
        /// <date>19.05.2022.</date>
        public string IconPath { get; set; }

        /// <summary>
        /// Text that shown by user.
        /// </summary>
        /// <date>19.05.2022.</date>
        public string Text { get; set; }

        /// <summary>
        /// Key to get text in according to using language. 
        /// </summary>
        /// <date>19.05.2022.</date>
        public string LocalizeKey { get; set; }

        /// <summary>
        /// Data to be shown to user.
        /// </summary>
        /// <date>17.05.2022.</date>
        public Avalonia.Controls.IControl Content { get; set; }

        /// <summary>
        /// Flag indicating whether the NavigationViewItem is selected by user.
        /// </summary>
        /// <date>19.05.2022.</date>
        public bool IsSelected { get; set; }
    }
}
