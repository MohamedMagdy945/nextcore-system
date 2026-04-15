using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviors;
using System.Reflection;

namespace Ordering.Application.Extensions
{
    public static class ServiceRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }

    }
}
