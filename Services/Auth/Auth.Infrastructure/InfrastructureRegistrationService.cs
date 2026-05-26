using Auth.Application.Interfaces;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.DatabaseSeeder;
using Auth.Infrastructure.Persistence.Seeder;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Settings;
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

            services.AddDbContext<IAuthDbContext, AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(
             configuration.GetSection("JwtSettings"));

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<RoleSeeder>();
            services.AddScoped<PermissionSeeder>();
            services.AddScoped<UserSeeder>();
            services.AddScoped<DatabaseSeeder>();

            return services;
        }
    }
}
