using DataBase;
using Microsoft.EntityFrameworkCore;

namespace AxisAvaloniaApp.Configurations
{
    public static class DatabaseConfiguration
    {
        private const string DatabaseName = "AxisUno.db";

        /// <summary>
        /// Gets options to configure a database.
        /// </summary>
        /// <returns>Path to connect to database.</returns>
        /// <date>09.09.2022.</date>
        public static DbContextOptions<DatabaseContext> GetOptions()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            builder.UseSqlite(GetConnectionString());

            return builder.Options;
        }

        /// <summary>
        /// Gets path to connect to database.
        /// </summary>
        /// <returns>Path to connect to database.</returns>
        /// <date>09.09.2022.</date>
        private static string GetConnectionString()
        {
            var fullPath = System.IO.Path.Combine(GetDatabaseLocation(), DatabaseName);

            return new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder()
            {
                DataSource = fullPath,
            }
            .ToString();
        }

        /// <summary>
        /// Gets path to folder with database.
        /// </summary>
        /// <returns>Path to folder with database.</returns>
        /// <date>09.09.2022.</date>
        private static string GetDatabaseLocation()
        {
            string dataBasePath = string.Empty;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                dataBasePath = System.IO.Path.Combine(
                    System.IO.Path.GetPathRoot(System.Environment.SystemDirectory),
                    "ProgramData",
                    "Axis",
                    "Uno");

                if (!System.IO.Directory.Exists(dataBasePath))
                {
                    System.IO.Directory.CreateDirectory(dataBasePath);
                }
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                //dataBasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                //dataBasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            }
            else
            {
                throw new System.Exception("Unidentified operating system!");
            }

            return dataBasePath;
        }
    }
}
