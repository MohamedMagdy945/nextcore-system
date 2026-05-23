using Auth.Application.Resources;
using System.Reflection;

namespace Auth.API.Configurations
{
    public static class LocalizationConfig
    {
        public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "");

            services.AddControllers()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(AuthSharedResource).Assembly.FullName!);

                        return factory.Create(typeof(AuthSharedResource));
                    };
                });

            services.AddHttpContextAccessor();

            return services;
        }

        public static WebApplication UseCustomLocalization(this WebApplication app)
        {
            var supportedCultures = new[] { "en", "ar" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            Messages.Configure(httpContextAccessor);

            return app;
        }
    }
}