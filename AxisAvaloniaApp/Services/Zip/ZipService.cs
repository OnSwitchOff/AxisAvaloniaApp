using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Zip
{
    public  class ZipService : IZipService
    {
        protected readonly ILoggerService loggerService;

        public ZipService()
        {
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
        }

        public bool CompressFileToZip(string fileName, string destination, string entryName)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(destination, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(fileName, entryName);
                }
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(CompressFileToZip));
                return false;
            }
            return true;          
        }

        public async Task<bool> CompressFileToZipAsync(string fileName, string destination, string entryName)
        {
            return await Task.Run(() => CompressFileToZip(fileName, destination, entryName));
        }

        public bool ExtractAllFromZip(string zipName, string destination)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(destination);
                }
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(ExtractAllFromZip));
                return false;
            }
            return true;
        }

        public async Task<bool> ExtractAllFromZipAsync(string zipName, string destination)
        {
            return await Task.Run(() => ExtractAllFromZip(zipName, destination));
        }
    }
}
