using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Common.Logging;

public static class LoggingConfiguration
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .CreateBootstrapLogger();
    }

    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, logger) =>
        {
            var env = context.HostingEnvironment;

            logger
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .Enrich.WithExceptionDetails()

                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);

            if (env.IsDevelopment())
            {
                logger.MinimumLevel.Debug();
            }

            logger.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} [{CorrelationId}]{NewLine}{Exception}"
            );

            var elasticUri = context.Configuration["ElasticConfiguration:Uri"];

            if (elasticUri == null) return;


            logger.WriteTo.Elasticsearch(new[] { new Uri(elasticUri) }, opts =>
            {
                var cleanAppName = env.ApplicationName?.ToLower().Replace(".", "-") ?? "app";
                var cleanEnvName = env.EnvironmentName?.ToLower() ?? "production";

                opts.DataStream = new DataStreamName("logs", cleanAppName, cleanEnvName);

                opts.BootstrapMethod = BootstrapMethod.Failure;

                opts.ConfigureChannel = channelOpts =>
                {
                    channelOpts.BufferOptions = new BufferOptions
                    {
                        ExportMaxConcurrency = 4
                    };
                };
            }, transport =>
            {

            });

        });

        return builder;
    }
}