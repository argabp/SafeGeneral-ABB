using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Middleware
{
    public class SessionGuardMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionGuardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (Path.HasExtension(path))
            {
                await _next(context);
                return;
            }
            
            // ---- Allow AllowAnonymous endpoints ----
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var allowAnonymous = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>();
                if (allowAnonymous != null)
                {
                    await _next(context);
                    return;
                }
            }

            // ---- AUTH CHECKS ----
            var hasSession = context.Session.GetString("SessionId") != null;
            var hasDbCookie = context.Request.Cookies["DatabaseValue"] != null;

            if (!hasSession || !hasDbCookie)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}