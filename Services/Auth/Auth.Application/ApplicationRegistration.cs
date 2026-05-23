using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

