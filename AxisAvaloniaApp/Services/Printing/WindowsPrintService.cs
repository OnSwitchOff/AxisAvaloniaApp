using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Printing
{
    public class WindowsPrintService : IPrintService
    {
        Encoding defaultEnc = new UTF8Encoding();
        public List<string> GetPrinters()
        {
            List<string> result = new List<string>();
            try
            {
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    result.Add(printer);
                }
            }
            catch (Exception e)
            {
                return result;
            }
            return result;
        }

        public bool SendByteArrayToPrinter(string printerName, byte[] bytes)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "print";
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardInput = true;

                Process p = new Process();
                p.StartInfo = info;
                p.Start();

                var myStreamWriter = p.StandardInput;

                myStreamWriter.Write(defaultEnc.GetString(bytes));
                myStreamWriter.Close();
                p.WaitForExit();
                p.Close();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
           
        }

        List<Image> Images = new List<Image>();
        // The Click event is raised when the user clicks the Print button.
        public bool PrintImageList(string szPrinterName, List<Image> images)
        {
            try
            {
                Images = images;
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = szPrinterName;
                //pd.PrinterSettings.PrintFileName
                pd.PrintPage += Pd_PrintPage;
                pd.Print();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        // The PrintPage event is raised for each page to be printed.
        private void Pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            // Create image.
            //Image newImage = Image.FromFile("SampImag.jpg");
            Image newImage = resizeImage(Images[0], new Size(ev.PageSettings.PaperSize.Width, ev.PageSettings.PaperSize.Height));

            // Create coordinates for upper-left corner of image.
            float x = 0.0F;
            float y = 0.0F;

            // Create rectangle for source image.
            RectangleF srcRect = new RectangleF(0.0F, 0.0F, newImage.Width, newImage.Height);
            GraphicsUnit units = GraphicsUnit.Point;

            // Draw image to screen.
            ev.Graphics.DrawImage(newImage, x, y, srcRect, units);
            Images.Remove(Images[0]);
            ev.HasMorePages = Images.Count > 0;
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)size.Width / (float)sourceWidth);

            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);

            int destHeight = (int)(sourceHeight * nPercent);

            System.Drawing.Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
    }
}
