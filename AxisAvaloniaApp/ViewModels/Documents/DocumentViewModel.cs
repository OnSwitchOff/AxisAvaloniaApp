using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class DocumentViewModel : ViewModelBase
    {
        protected abstract EDocumentTypes documentType { get; }
    }
}
