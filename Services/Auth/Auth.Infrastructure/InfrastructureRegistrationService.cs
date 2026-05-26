using Auth.Application.Interfaces;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Auth.Infrastructure
{
    public static class InfrastructureRegistrationService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
