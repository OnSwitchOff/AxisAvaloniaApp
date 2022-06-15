using SharpIpp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }
}
