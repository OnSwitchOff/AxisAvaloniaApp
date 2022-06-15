using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }
}
