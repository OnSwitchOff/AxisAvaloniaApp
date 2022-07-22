using DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

namespace AxisAvaloniaApp.Configurations
{
    public static class AppConfiguration
    {
        private const string DatabaseName = "AxisUno.db";
        //private static string databaseLocation;
        public static bool? isDatabaseExist;

        /// <summary>
        /// Initialize databaseLocation field
        /// </summary>
        /// <exception cref="Exception">Throws Exception if operation system is not identified.</exception>
        static AppConfiguration()
        {
            //if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            //{
            //    databaseLocation = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "ProgramData");
            //}
            //else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            //{
            //    //databaseLocation = Path.Combine("/var", "lib");
            //    databaseLocation = Path.Combine("/home", Environment.UserName);
            //}
            //else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            //{
            //    databaseLocation = Path.Combine("/Users", Environment.UserName, "Library", "Application Support");

            //}
            //else
            //{
            //    throw new Exception("Unidentified operating system!");
            //}

            //databaseLocation = Path.Combine(databaseLocation, "Axis", "Uno");
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
        //=> Directory.Exists(databaseLocation);

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
                else if (File.Exists(Path.Combine(DatabaseLocation, "logo.jpeg")))
                {
                    logoPath = Path.Combine(DatabaseLocation, "logo.jpeg");
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
                if (extention.Equals(".png") || extention.Equals(".jpg") || extention.Equals(".jpeg") || extention.Equals(".bmp") || extention.Equals(".ico"))
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

                //string dataBasePath = string.Empty;
                //if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                //{
                //    dataBasePath = Path.Combine(
                //        Path.GetPathRoot(System.Environment.SystemDirectory),
                //        "ProgramData",
                //        "Axis",
                //        "Uno");

                //    if (!Directory.Exists(dataBasePath))
                //    {
                //        Directory.CreateDirectory(dataBasePath);
                //    }
                //}
                //else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                //{
                //    dataBasePath = Path.Combine("/var", "lib", "Axis", "Uno");
                //    string cmd = "chmod -R 777 " + Path.Combine("/var", "lib");
                //    try
                //    {
                //        using (Process proc = Process.Start("/bin/bash", $"-c \"{cmd}\""))
                //        {
                //            proc.WaitForExit();
                //            //return proc.ExitCode == 0;

                //            if (!Directory.Exists(databaseLocation))
                //            {
                //                Directory.CreateDirectory(databaseLocation);
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                //{
                //    dataBasePath = Path.Combine("Library", "Application Support", "Axis", "Uno");

                //}
                //else
                //{
                //    throw new System.Exception("Unidentified operating system!");
                //}

                return databaseLocation;
            }
        }
    }
}
