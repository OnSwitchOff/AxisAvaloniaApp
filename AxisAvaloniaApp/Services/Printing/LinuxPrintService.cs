using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Printing
{
    public class LinuxPrintService : IPrintService
    {
        Encoding defaultEnc = new UTF8Encoding();
        public List<string> GetPrinters()
        {
            List<string> result = new List<string>();
            try
            {
                System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo();
                process.UseShellExecute = false;
                process.FileName = "lpstat";
                process.Arguments = "-e";
                process.RedirectStandardOutput = true;

                System.Diagnostics.Process cmd = System.Diagnostics.Process.Start(process);

                string? line;

                while ((line = cmd.StandardOutput.ReadLine()) != null)
                {
                    result.Add(line);
                }

                cmd.WaitForExit();

            }
            catch (Exception e)
            {
                return result;
            }
            return result;
        }

        public bool SendByteArrayToPrinter(string szPrinterName, byte[] bytes)
        {
            try
            {
                var myProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = "lpr",
                        Arguments = $"-T receipt -P \"{szPrinterName}\" -l ",
                        UseShellExecute = false,
                        RedirectStandardInput = true
                    }
                };

                myProcess.Start();

                var myStreamWriter = myProcess.StandardInput;

                myStreamWriter.Write(defaultEnc.GetString(bytes));
                myStreamWriter.Close();
                myProcess.WaitForExit();
                myProcess.Close();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


        List<Image> Images = new List<Image>();
        // The Click event is raised when the user clicks the Print button.
        public bool PrintImageList(string szPrinterName, List<Image> images, bool landscape = false)
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
            Image newImage = Images[0];

            // Create coordinates for upper-left corner of image.
            float x = 0.0F;
            float y = 0.0F;

            // Create rectangle for source image.
            RectangleF srcRect = new RectangleF(0.0F, 0.0F, newImage.Width, newImage.Height);
            GraphicsUnit units = GraphicsUnit.Pixel;

            // Draw image to screen.
            ev.Graphics.DrawImage(newImage, x, y, srcRect, units);
            Images.Remove(Images[0]);
            ev.HasMorePages = Images.Count > 0;
        }
    }
}
