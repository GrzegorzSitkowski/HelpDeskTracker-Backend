namespace HelpDeskTracker.WebApi.Middlewares
{
    public static class ExceptionResultMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionResultMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionResultMiddleware>();
        }
    }
}
