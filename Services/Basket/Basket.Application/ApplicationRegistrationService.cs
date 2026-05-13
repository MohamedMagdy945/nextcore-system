using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Basket.Application
{
    public static class ApplicationRegistrationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}