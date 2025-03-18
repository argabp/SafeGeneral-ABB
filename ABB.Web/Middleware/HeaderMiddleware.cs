using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ABB.Web.Middleware
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public HeaderMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("X-AspNet-Version");
            context.Response.Headers.Remove("Server");
            //context.Response.Headers.Add("Cache-Control", "max-age=86400, must-revalidate");
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Add("Feature-Policy", "camera 'none';  payment 'none'; usb 'none'");
            context.Response.Headers.Add("Content-Security-Policy",
                _config.GetSection("Content-Security-Policy").Value);
            await _next(context);
        }
    }
}