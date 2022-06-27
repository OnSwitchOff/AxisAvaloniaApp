using AxisAvaloniaApp.UserControls.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Printing
{
    public interface IPrintService
    {
        List<string> GetPrinters();
        bool SendByteArrayToPrinter(string printerName, byte[] bytes);
        bool PrintImageList(string szPrinterName, List<Image> images, bool landscape);
    }

}
