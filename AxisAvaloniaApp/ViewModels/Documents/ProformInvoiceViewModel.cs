using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class ProformInvoiceViewModel : DocumentViewModel
    {
        protected override EDocumentTypes documentType => EDocumentTypes.ProformInvoice;
    }
}
