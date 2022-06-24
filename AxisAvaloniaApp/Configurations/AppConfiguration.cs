using DataBase;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AxisAvaloniaApp.Configurations
{
    public static class AppConfiguration
    {
        private const string DatabaseName = "AxisUno.db";

        /// <summary>
        /// Gets options to configure a database.
        /// </summary>
        /// <returns>Path to connect to database.</returns>
        /// <date>09.09.2022.</date>
        public static DbContextOptions<DatabaseContext> GetDatabaseOptions()
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            builder.UseSqlite(ConnectionString);

            return builder.Options;
        }

        /// <summary>
        /// Gets value indicating whether database exists. 
        /// </summary>
        public static bool IsDatabaseExist => Directory.Exists(Path.Combine(Path.GetPathRoot(System.Environment.SystemDirectory), "ProgramData", "Axis", "Uno"));

        /// <summary>
        /// Gets or sets path to logo.
        /// </summary>
        /// <date>13.06.2022.</date>
        public static string LogoPath
        {
            get
            {
                string logoPath = Path.Combine(DatabaseLocation, "logo.png");
                if (File.Exists(Path.Combine(DatabaseLocation, "logo.jpg")))
                {
                    logoPath = Path.Combine(DatabaseLocation, "logo.jpg");
                }
                else if (File.Exists(Path.Combine(DatabaseLocation, "logo.bmp")))
                {
                    logoPath = Path.Combine(DatabaseLocation, "logo.bmp");
                }
                else if (File.Exists(Path.Combine(DatabaseLocation, "logo.ico")))
                {
                    logoPath = Path.Combine(DatabaseLocation, "logo.ico");
                }

                return logoPath;
            }
            set
            {
                string extention = value.Substring(value.LastIndexOf("."), value.Length - value.LastIndexOf("."));
                if (extention.Equals(".png") || extention.Equals(".jpg") || extention.Equals(".bmp") || extention.Equals(".ico"))
                {
                    if (File.Exists(LogoPath))
                    {
                        File.Delete(LogoPath);
                    }

                    File.Copy(value, Path.Combine(DatabaseLocation, "logo" + extention), true);
                }
            }
        }

        /// <summary>
        /// Gets or sets path to log file.
        /// </summary>
        /// <date>14.06.2022.</date>
        public static string LogFilePath => Path.Combine(DatabaseLocation, "error.log");

        /// <summary>
        /// Gets path to connect to database.
        /// </summary>
        /// <returns>Path to connect to database.</returns>
        /// <date>09.09.2022.</date>
        private static string ConnectionString
        {
            get
            {
                var fullPath = Path.Combine(DatabaseLocation, DatabaseName);

                return new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder()
                {
                    DataSource = fullPath,
                }
                .ToString();
            }
        }

        /// <summary>
        /// Gets path to folder with database.
        /// </summary>
        /// <returns>Path to folder with database.</returns>
        /// <date>09.09.2022.</date>
        private static string DatabaseLocation
        {
            get
            {
                string dataBasePath = string.Empty;
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    dataBasePath = Path.Combine(
                        Path.GetPathRoot(System.Environment.SystemDirectory),
                        "ProgramData",
                        "Axis",
                        "Uno");

                    if (!Directory.Exists(dataBasePath))
                    {
                        Directory.CreateDirectory(dataBasePath);
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
}
