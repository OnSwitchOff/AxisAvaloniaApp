using Autofac;
using Serilog;
using Serilog.Formatting.Compact;

namespace AxisAvaloniaApp.AutofacModules
{
    public sealed class LoggingModule : Module
    {
        private const string LogFileName = "logs.json";

        protected override void Load(ContainerBuilder builder)
        {
            var logger = CreateLogger();

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
        }

        private static ILogger CreateLogger()
        {
            var file = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), LogFileName);
            if (!System.IO.File.Exists(file))
            {
                System.IO.File.Create(file);
            }

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), file)
                .CreateLogger();
        }
    }
}
