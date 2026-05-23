using Auth.Application.Resources;
using System.Reflection;

namespace Auth.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddControllers()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(AuthSharedResource).Assembly.FullName!);
                        return factory.Create("AuthSharedResource", assemblyName.Name!);
                    };
                });

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();
            var supportedCultures = new[] { "en", "ar" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
