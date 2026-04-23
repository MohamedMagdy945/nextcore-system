
using Asp.Versioning;
using Basket.Application.Common;
using Basket.Application.GerpcService;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Host.UseSerilog(Logging.ConfigureLogger);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Basket API",
                        Description = "An ASP.NET Core Web API for managing basket v1 micro-services in commerce application",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Mohamed Magdy",
                            Email = "mohamedmagdy000022@gmail.com",
                        }
                    });
                options.SwaggerDoc("v2",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Version = "v2",
                        Title = "Basket API",
                        Description = "An ASP.NET Core Web API for managing basket v2 micro-services in commerce application",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Mohamed Magdy",
                            Email = "mohamedmagdy000022@gmail.com",
                        }
                    });
                options.DocInclusionPredicate((version, apiDescription) =>
                    {
                        if (!apiDescription.TryGetMethodInfo(out var methodInfo))
                        {
                            return false;
                        }
                        var versions = methodInfo.DeclaringType?
                                        .GetCustomAttributes(true)
                                        .OfType<ApiVersionAttribute>()
                                        .SelectMany(attr => attr.Versions);
                        return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;
                    }

                );
            });



            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
            });

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyMarker).Assembly));


            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v' VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            ;

            //redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            var discountUrl = builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl");

            builder.Services.AddScoped<DiscountGrpcSerivce>();

            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(cfg =>
            {
                cfg.Address = new Uri(discountUrl!);
            });


            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ct, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });

            builder.Services.AddMassTransitHostedService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket.API v2");
                });
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
