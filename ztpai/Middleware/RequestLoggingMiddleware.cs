using ztpai.Services;

namespace ztpai.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate requestDelegate)
    {
        public async Task InvokeAsync(HttpContext context, ILoggingService loggingService)
        {
            loggingService.Log($"{context.Request.Method} - {context.Request.Path}");

            await requestDelegate(context);

            loggingService.Log($"{context.Response.StatusCode}");
        }
    }
}
