using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using System;

namespace AxisAvaloniaApp.UserControls.MessageBoxes
{
    public static class StyleExtensions
    {
        /// <summary>
        /// Sets style to Window in according to EMessageBoxStyles item.
        /// </summary>
        /// <param name="window">Window to set style.</param>
        /// <param name="style">Key to set style.</param>
        /// <date>14.06.2022.</date>
        public static void SetStyle(this Window window, EMessageBoxStyles style)
        {
            var styles = window.Styles;
            switch (style)
            {
                case EMessageBoxStyles.Windows:
                    styles.Add(new StyleInclude(new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Windows.axaml"))
                    {
                        Source = new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Windows.axaml"),
                    });
                    break;
                case EMessageBoxStyles.MacOs:
                    styles.Add(new StyleInclude(new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/MacOs.axaml"))
                    {
                        Source = new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/MacOs.axaml"),
                    });
                    break;
                case EMessageBoxStyles.UbuntuLinux:
                    styles.Add(new StyleInclude(new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Ubuntu.axaml"))
                    {
                        Source = new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Ubuntu.axaml"),
                    });
                    break;
                case EMessageBoxStyles.MintLinux:
                    styles.Add(new StyleInclude(new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Mint.axaml"))
                    {
                        Source = new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Mint.axaml"),
                    });
                    break;
                case EMessageBoxStyles.DarkMode:
                    styles.Add(new StyleInclude(new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Dark.axaml"))
                    {
                        Source = new Uri("avares://AxisAvaloniaApp/Styles/MessageBox/Dark.axaml"),
                    });
                    break;
            }
        }
    }
}
