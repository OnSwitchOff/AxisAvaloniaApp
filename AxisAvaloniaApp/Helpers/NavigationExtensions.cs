using Avalonia;
using Microinvest.CommonLibrary.Enums;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace AxisAvaloniaApp.Helpers
{
    public class NavigationExtensions
    {
        /// <summary>
        /// Registers type of document.
        /// </summary>
        /// <date>25.05.2022.</date>
        public static readonly AttachedProperty<EDocumentTypes> DocumentTypeProperty = 
            AvaloniaProperty.RegisterAttached<NavigationExtensions, Interactive, EDocumentTypes>(
            "DocumentType", default(EDocumentTypes), false, BindingMode.OneTime);

        /// <summary>
        /// Sets type of document.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <param name="value">Type of document.</param>
        /// <date>25.05.2022.</date>
        public static void SetDocumentType(AvaloniaObject element, EDocumentTypes value)
        {
            element.SetValue(DocumentTypeProperty, value);
        }

        /// <summary>
        /// Gets type of document.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <returns>Type of document.</returns>
        /// <date>25.05.2022.</date>
        public static EDocumentTypes GetDocumentType(AvaloniaObject element)
        {
            return element.GetValue(DocumentTypeProperty);
        }

        /// <summary>
        /// Registers path to ViewModel.
        /// </summary>
        /// <date>25.05.2022.</date>
        public static readonly AttachedProperty<string> NavigateToProperty =
            AvaloniaProperty.RegisterAttached<NavigationExtensions, Interactive, string>(
            "NavigateTo", string.Empty, false, BindingMode.OneTime);

        /// <summary>
        /// Sets path to ViewModel.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <param name="value">Path tof ViewModel.</param>
        /// <date>25.05.2022.</date>
        public static void SetNavigateTo(AvaloniaObject element, string value)
        {
            element.SetValue(NavigateToProperty, value);
        }

        /// <summary>
        /// Gets path to ViewModel.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <returns>Path to ViewModel.</returns>
        /// <date>25.05.2022.</date>
        public static string GetNavigateTo(AvaloniaObject element)
        {
            return element.GetValue(NavigateToProperty);
        }

        /// <summary>
        /// Registers key to get explanation of the control.
        /// </summary>
        /// <date>25.05.2022.</date>
        public static readonly AttachedProperty<string> ExplanationKeyProperty =
            AvaloniaProperty.RegisterAttached<NavigationExtensions, Interactive, string>(
            "DocumentType", string.Empty, false, BindingMode.OneTime);

        /// <summary>
        /// Sets key to get explanation of the control.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <param name="value">Key to get explanation of the control.</param>
        /// <date>25.05.2022.</date>
        public static void SetExplanationKey(AvaloniaObject element, string value)
        {
            element.SetValue(ExplanationKeyProperty, value);
        }

        /// <summary>
        /// Gets key to get explanation of the control.
        /// </summary>
        /// <param name="element">The control that uses the property.</param>
        /// <returns>Key to get explanation of the control.</returns>
        /// <date>25.05.2022.</date>
        public static string GetExplanationKey(AvaloniaObject element)
        {
            return element.GetValue(ExplanationKeyProperty);
        }
    }
}
