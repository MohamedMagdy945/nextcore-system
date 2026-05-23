using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Auth.API.Controllers;

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
                Title = "NextCoreApp Auth Service API",
                Version = "v1",
                Description =
                  "Authentication and Authorization service for NextCoreApp. " +
                  "Provides JWT authentication, user registration, login, role management, " +
                  "refresh tokens, and account security operations.",

                Contact = new OpenApiContact
                {
                    Name = "Mohamed magdy said",
                    Email = "mohamedmagdy000022@gmail.com",
                },
            });

            // JWT
            const string securityScheme = "Bearer";

            options.AddSecurityDefinition(securityScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = securityScheme
                        }
                    },
                    Array.Empty<string>()
                }
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

            options.SwaggerEndpoint("/swagger/v1/swagger.json", "AppCore API V1");
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