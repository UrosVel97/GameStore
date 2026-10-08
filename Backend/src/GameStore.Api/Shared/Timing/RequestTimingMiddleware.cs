using System.Diagnostics;
namespace GameStore.Api.Shared.Timing;

public class RequestTimingMiddleware(
    RequestDelegate next,
    ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch? stopwatch = null;

        try
        {
            stopwatch = Stopwatch.StartNew();

            await next(context);

            stopwatch.Stop();

        }
        finally
        {
            var elapsedTimeInMs = stopwatch?.ElapsedMilliseconds;

            logger.LogInformation(
                "{RequestMethod} {RequestPath} completed with status {StatusCode} in {ElapsedTimeInMs} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsedTimeInMs);
        }
    }
}
