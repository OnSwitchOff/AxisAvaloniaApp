using SharpIpp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Printing
{
    public class LinuxPrintService : IPrintService
    {
        public void GetPrinters()
        {
            var client = new SharpIpp.SharpIppClient();
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;
            var r =  client.GetCUPSPrintersAsync(new CUPSGetPrintersRequest(), token);
            //await using var stream = File.Open(@"c:\file.pdf", FileMode.Open);
            //var printerUri = new Uri("ipp://192.168.0.1:631");
            //var request = new PrintJobRequest
            //{
            //    PrinterUri = printerUri,
            //    Document = stream,
            //    JobName = "Test Job",
            //    IppAttributeFidelity = false,
            //    DocumentName = "Document Name",
            //    DocumentFormat = "application/octet-stream",
            //    DocumentNaturalLanguage = "en",
            //    MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
            //    Copies = 1,
            //    Finishings = Finishings.None,
            //    PageRanges = new[] { new Range(1, 1) },
            //    Sides = Sides.OneSided,
            //    NumberUp = 1,
            //    OrientationRequested = Orientation.Portrait,
            //    PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
            //    PrintQuality = PrintQuality.Normal
            //};
            //request.Jo
            //var response = await client.PrintJobAsync(request);
        }
    }
}
