using Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Api.Extensions
{
    public static class ExeptionExtensionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}