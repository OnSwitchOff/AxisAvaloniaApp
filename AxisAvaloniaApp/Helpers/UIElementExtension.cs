using AxisAvaloniaApp.Services.Translation;

namespace UnoWinUINet5.Helpers
{
    /// <summary>
    /// New properties of an UIElement.
    /// </summary>
    public class UIElementExtension
    {
        ///// <summary>
        ///// Describes HelpMessage property.
        ///// </summary>
        ///// <date>23.03.2022.</date>
        //public static readonly DependencyProperty HelpMessageProperty =
        //    DependencyProperty.RegisterAttached("HelpMessage", typeof(string), typeof(UIElementExtension), new PropertyMetadata(null));

        ///// <summary>
        ///// Describes LocalizedText property.
        ///// </summary>
        ///// <date>13.04.2022.</date>
        //public static readonly DependencyProperty LocalizedTextProperty =
        //    DependencyProperty.RegisterAttached("LocalizedText", typeof(string), typeof(UIElementExtension), new PropertyMetadata(null));

        //private static readonly ITranslationService TranslationService = Services.Translation.TranslationService.CreateInstance();

        ///// <summary>
        ///// Gets HelpMessage property.
        ///// </summary>
        ///// <param name="obj">UIElement.</param>
        ///// <returns>Returns value of HelpMessage property.</returns>
        ///// <date>23.03.2022.</date>
        //public static string GetHelpMessage(UIElement obj)
        //{
        //    return (string)obj.GetValue(HelpMessageProperty);
        //}

        ///// <summary>
        ///// Sets HelpMessage property.
        ///// </summary>
        ///// <param name="obj">UIElement.</param>
        ///// <param name="value">New value of HelpMessage property.</param>
        ///// <date>23.03.2022.</date>
        //public static void SetHelpMessage(UIElement obj, string value)
        //{
        //    obj.SetValue(HelpMessageProperty, value);

        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        obj.PointerEntered += Obj_PointerEntered;
        //        obj.PointerExited += Obj_PointerExited;
        //    }
        //}

        ///// <summary>
        ///// Gets LocalizedText property.
        ///// </summary>
        ///// <param name="obj">UIElement.</param>
        ///// <returns>Returns value of LocalizedText property.</returns>
        ///// <date>13.04.2022.</date>
        //public static string GetLocalizedText(UIElement obj)
        //{
        //    return (string)obj.GetValue(LocalizedTextProperty);
        //}

        ///// <summary>
        ///// Sets LocalizedText property.
        ///// </summary>
        ///// <param name="obj">UIElement.</param>
        ///// <param name="value">New value of LocalizedText property.</param>
        ///// <date>13.04.2022.</date>
        //public static void SetLocalizedText(UIElement obj, string value)
        //{
        //    obj.SetValue(LocalizedTextProperty, value);
        //    string localizedValue = TranslationService.Localize(value);

        //    switch (obj)
        //    {
        //        case TextBlock textBlock:
        //            textBlock.Text = localizedValue;
        //            break;
        //        case NavigationViewItemBase viewItem:
        //            viewItem.Content = localizedValue;
        //            break;
        //        case ToolTip toolTip:
        //            toolTip.Content = localizedValue;
        //            break;
        //        case Button button:
        //            button.Content = localizedValue;
        //            break;
        //        case Expander expander:
        //            expander.Header = localizedValue;
        //            break;
        //        case UserControls.PaymentButton paymentButton:
        //            paymentButton.Text = localizedValue;
        //            break;
        //    }
        //}

        ///// <summary>
        ///// Clear explanation string if pointer was exited from UIElement.
        ///// </summary>
        ///// <param name="sender">UIElement</param>
        ///// <param name="e">Event args.</param>
        ///// <date>23.03.2022.</date>
        //private static void Obj_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    if (App.MainWindow.Content is MainView)
        //    {
        //        ((MainView)App.MainWindow.Content).ViewModel.HelpMessage = string.Empty;
        //    }
        //}

        ///// <summary>
        ///// Show explanation string if pointer was entered to UIElement.
        ///// </summary>
        ///// <param name="sender">UIElement</param>
        ///// <param name="e">Event args.</param>
        ///// <date>23.03.2022.</date>
        //private static void Obj_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    if (App.MainWindow.Content is MainView)
        //    {
        //        ((MainView)App.MainWindow.Content).ViewModel.HelpMessage = TranslationService.GetExplanation((string)((UIElement)sender).GetValue(HelpMessageProperty));
        //    }
        //}
    }
}
