using System.Diagnostics;

namespace Auth.API.Middlewares
{
    public sealed class CorrelationIdMiddleware(RequestDelegate next)
    {
        private const string HeaderName = "X-Correlation-Id";

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId =
                context.Request.Headers[HeaderName].FirstOrDefault()
                ?? Guid.NewGuid().ToString("N");

            context.TraceIdentifier = correlationId;

            Activity.Current?.SetTag(
                "correlation.id",
                correlationId);

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(HeaderName))
                {
                    context.Response.Headers[HeaderName] = correlationId;
                }

                return Task.CompletedTask;
            });

            using (Serilog.Context.LogContext.PushProperty(
                       "CorrelationId",
                       correlationId))
            {
                await next(context);
            }
        }
    }
}
