using System.Net;
using System.Text.Json;

namespace SkogsInsikt.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Ett oväntat fel inträffade vid {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                status = 500,
                message = "Ett oväntat fel inträffade. Försök igen senare."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
