using Microsoft.AspNetCore.Diagnostics;

namespace ztpai.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            var error = new { 
                error = "Unexpected server error occured", 
                message = exception.Message 
            };
            
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}
