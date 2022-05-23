using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microinvest.CommonLibrary.Enums;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace AxisAvaloniaApp.Helpers
{
    public class NavigationExtensions
    {
        public static readonly AttachedProperty<EDocumentTypes> DocumentTypeProperty = 
            AvaloniaProperty.RegisterAttached<NavigationExtensions, Interactive, EDocumentTypes>(
            "DocumentType", default(EDocumentTypes), false, BindingMode.OneTime);

        public static void SetDocumentType(AvaloniaObject element, EDocumentTypes value)
        {
            element.SetValue(DocumentTypeProperty, value);
        }

        public static EDocumentTypes GetDocumentType(AvaloniaObject element)
        {
            return element.GetValue(DocumentTypeProperty);
        }

        public static readonly AttachedProperty<string> NavigateToProperty =
            AvaloniaProperty.RegisterAttached<NavigationExtensions, Interactive, string>(
            "NavigateTo", string.Empty, false, BindingMode.OneTime);

        public static void SetNavigateTo(AvaloniaObject element, string value)
        {
            element.SetValue(NavigateToProperty, value);
        }

        public static string GetNavigateTo(AvaloniaObject element)
        {
            return element.GetValue(NavigateToProperty);
        }
    }
}
