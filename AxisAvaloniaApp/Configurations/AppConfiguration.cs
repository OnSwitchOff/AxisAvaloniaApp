using DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace AxisAvaloniaApp.Configurations
{
    public static class AppConfiguration
    {
        private const string DatabaseName = "AxisUno.db";
        private static bool? isDatabaseExist;
        private static readonly System.Collections.Generic.List<string> supportedImageFormats;


        /// <summary>
        /// Initialize databaseLocation field
        /// </summary>
        /// <exception cref="Exception">Throws Exception if operation system is not identified.</exception>
        static AppConfiguration()
        {
            supportedImageFormats = new System.Collections.Generic.List<string>() { ".png", ".jpg", ".jpeg", ".bmp", };
        }

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
        public static bool IsDatabaseExist
        {
            get
            {
                if (isDatabaseExist == null)
                {
                    string initial = DatabaseLocation;
                }

                return (bool)isDatabaseExist;
            }
            private set
            {
                if (isDatabaseExist == null)
                {
                    isDatabaseExist = value;
                }
            }

        }

        /// <summary>
        /// Gets or sets path to logo.
        /// </summary>
        /// <date>13.06.2022.</date>
        public static string LogoPath
        {
            get
            {
                foreach (string imageFormat in supportedImageFormats)
                {
                    if (File.Exists(Path.Combine(DatabaseLocation, "logo" + imageFormat)))
                    {
                        return Path.Combine(DatabaseLocation, "logo" + imageFormat);
                    }
                }

                return Path.Combine(DatabaseLocation, "logo.png");
            }
            set
            {
                string extention = GetFileFormat(value);
                if (supportedImageFormats.Contains(extention))
                {
                    try
                    {
                        if (File.Exists(LogoPath))
                        {
                            File.Delete(LogoPath);
                        }

                        File.Copy(value, Path.Combine(DatabaseLocation, "logo" + extention), true);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets path to logo.
        /// </summary>
        /// <date>13.06.2022.</date>
        public static string DocumentHeaderPath
        {
            get
            {
                foreach (string imageFormat in supportedImageFormats)
                {
                    if (File.Exists(Path.Combine(DatabaseLocation, "DocumentHeader" + imageFormat)))
                    {
                        return Path.Combine(DatabaseLocation, "DocumentHeader" + imageFormat);
                    }
                }

                return Path.Combine(DatabaseLocation, "DocumentHeader.png");
            }
            set
            {
                string extention = GetFileFormat(value);
                if (supportedImageFormats.Contains(extention))
                {
                    try
                    {
                        if (File.Exists(DocumentHeaderPath))
                        {
                            File.Delete(DocumentHeaderPath);
                        }

                        File.Copy(value, Path.Combine(DatabaseLocation, "DocumentHeader" + extention), true);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
        }


        /// <summary>
        /// Gets or sets path to logo.
        /// </summary>
        /// <date>13.06.2022.</date>
        public static string DocumentFooterPath
        {
            get
            {
                foreach (string imageFormat in supportedImageFormats)
                {
                    if (File.Exists(Path.Combine(DatabaseLocation, "DocumentFooter" + imageFormat)))
                    {
                        return Path.Combine(DatabaseLocation, "DocumentFooter" + imageFormat);
                    }
                }

                return Path.Combine(DatabaseLocation, "DocumentFooter.png");
            }
            set
            {
                string extention = GetFileFormat(value);
                if (supportedImageFormats.Contains(extention))
                {
                    try
                    {
                        if (File.Exists(DocumentFooterPath))
                        {
                            File.Delete(DocumentFooterPath);
                        }

                        File.Copy(value, Path.Combine(DatabaseLocation, "DocumentFooter" + extention), true);
                    }
                    catch (System.Exception ex)
                    {

                    }
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
        public static string DatabaseLocation
        {
            get
            {
                string databaseLocation;
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    databaseLocation = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "ProgramData");
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                {
                    //databaseLocation = Path.Combine("/var", "lib");
                    databaseLocation = Path.Combine("/home", Environment.UserName);
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    databaseLocation = Path.Combine("/Users", Environment.UserName, "Library", "Application Support");

                }
                else
                {
                    throw new Exception("Unidentified operating system!");
                }

                databaseLocation = Path.Combine(databaseLocation, "Axis", "Uno");

                if (!Directory.Exists(databaseLocation))
                {
                    IsDatabaseExist = false;
                    Directory.CreateDirectory(databaseLocation);
                }

                IsDatabaseExist = true;

                return databaseLocation;
            }
        }

        /// <summary>
        /// Path to ZipArchives
        /// </summary>
        public static string BackupFolderPath
        { 
            get
            {
                string path = Path.Combine(DatabaseLocation, "Backup");
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
                return  path;
            }                
        }

        /// <summary>
        /// Path to Database
        /// </summary>
        public static string DatabaseFullName { get => Path.Combine(DatabaseLocation, DatabaseName); }
        public static string DatabaseShortName { get =>  DatabaseName; }

        /// <summary>
        /// Gets format of file.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>returns format of file.</returns>
        /// <date>22.07.2022.</date>
        private static string GetFileFormat(string filePath)
        {
            return filePath.Substring(filePath.LastIndexOf("."), filePath.Length - filePath.LastIndexOf("."));
        }
    }
}
