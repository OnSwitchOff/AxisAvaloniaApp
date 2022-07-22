using AxisAvaloniaApp.Configurations;
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
                using (ZipArchive archive = ZipFile.Open(destination, ZipArchiveMode.Update))
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
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(destination, entry.Name), true);    
                    }
                }
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(ExtractAllFromZip));
                return false;
            }
            return true;
        }

        public bool? ExtractDbFromZip(string zipName, string destination)
        {
            try
            {
                using (ZipArchive archive = ZipFile.Open(zipName, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name == AppConfiguration.DatabaseShortName)
                        {
                            entry.ExtractToFile(Path.Combine(destination, entry.Name), true);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(ExtractAllFromZip));
                return false;
            }
            loggerService.ShowDialog("msgNoDbFileInArchive", icon: UserControls.MessageBoxes.EButtonIcons.Info);
            return null;
        }

        public async Task<bool> ExtractAllFromZipAsync(string zipName, string destination)
        {
            return await Task.Run(() => ExtractAllFromZip(zipName, destination));
        }
    }
}
