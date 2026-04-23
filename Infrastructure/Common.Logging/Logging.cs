using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfigureation) =>
            {
                var environment = context.HostingEnvironment;
                loggerConfigureation.MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", environment.ApplicationName)
                    .Enrich.WithProperty("EnvironmentName", environment.EnvironmentName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                    .WriteTo.Console();

                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfigureation.MinimumLevel.Override("Catalog", LogEventLevel.Debug)
                        .MinimumLevel.Override("Basket", LogEventLevel.Debug)
                        .MinimumLevel.Override("Discount", LogEventLevel.Debug)
                        .MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                }

                var elasticSearch = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                if (!string.IsNullOrEmpty(elasticSearch))
                {
                    loggerConfigureation.WriteTo.Elasticsearch(
                        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(
                            new Uri(elasticSearch))
                        {
                            AutoRegisterTemplate = true,
                            AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                            IndexFormat = "NewEcommerce-logs-{0:yyy.MM.dd}",
                            MinimumLogEventLevel = LogEventLevel.Debug,
                        }

                        );
                }

            };
    }
}
