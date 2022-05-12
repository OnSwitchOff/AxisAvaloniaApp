using Autofac;
using FluentValidation;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using AxisAvaloniaApp.Helpers;

namespace AxisAvaloniaApp.AutofacModules
{
    public sealed class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatR(MediatorAssemblies.ApplicationLayer);

            var openHandlerTypes = new[]
            {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>),
        };

            foreach (var openHandlerType in openHandlerTypes)
            {
                builder.RegisterAssemblyTypes(MediatorAssemblies.ApplicationLayer)
                    .AsClosedTypesOf(openHandlerType)
                    .AsImplementedInterfaces();
            }
        }
    }
}
