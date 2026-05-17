using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Ordering.API.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(
            this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Order API v1",
                    Version = "v1",
                    Description = "An ASP.NET Core Web API for managing Order v1 micro-services in commerce application",
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Order API v2",
                    Version = "v2",
                    Description = "An ASP.NET Core Web API for managing Order v2 micro-services in commerce application",
                });



                // XML Docs
                var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);

                // Grouping
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.GroupName?.Equals(docName, StringComparison.OrdinalIgnoreCase) ?? true)
                        return false;

                    return true;
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerDocumentation(
           this WebApplication app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;

                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Order API V2");
                options.DisplayRequestDuration();
                options.EnablePersistAuthorization();
                options.DocExpansion(DocExpansion.None);
                options.DefaultModelsExpandDepth(-1);
                options.EnableFilter();
                options.EnableDeepLinking();
            });

            return app;
        }
    }
}
