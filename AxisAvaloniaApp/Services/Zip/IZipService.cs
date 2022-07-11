using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Zip
{
    public interface IZipService
    {
        bool CompressFileToZip(string fileName, string destination, string entryName);
        bool ExtractAllFromZip(string zipName, string destination);
        Task<bool> CompressFileToZipAsync(string fileName, string destination, string entryName);
        Task<bool> ExtractAllFromZipAsync(string zipName, string destination);
    }
}
