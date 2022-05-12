using Autofac;
using AxisAvaloniaApp.Services.ThemeSelector;
using AxisAvaloniaApp.ViewModels;

namespace AxisAvaloniaApp.AutofacModules
{
    public class ApplicationServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().InstancePerDependency();
            builder.RegisterType<SaleViewModel>().InstancePerDependency();
            builder.RegisterType<DocumentViewModel>().InstancePerDependency();
            builder.RegisterType<CashRegisterViewModel>().InstancePerDependency();
            builder.RegisterType<ExchangeViewModel>().InstancePerDependency();
            builder.RegisterType<ReportsViewModel>().InstancePerDependency();
            builder.RegisterType<SettingsViewModel>().InstancePerDependency();

            builder.RegisterType<ThemeSelectorService>().As<IThemeSelectorService>().InstancePerDependency();

            base.Load(builder);
        }
    }
}
