using MAApi.Middlewares;

namespace MAApi.Extensions
{
    public static class WebApiResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebApiResponseMiddleware>();
        }
    }
}
