using System;
using System.Text;
using System.Threading.Tasks;
using ABB.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ABB.Web.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var message = $"Controller:{context.GetControllerName()} | Action:{context.GetActionName()}";
                _logger.LogError(ex, message);

                await ErrorResponse(context);
            }
        }

        private async Task ErrorResponse(HttpContext context)
        {
            var isFromAjax = context.Request.IsAjaxRequest();
            if (isFromAjax)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                var json = JsonConvert.SerializeObject(new
                {
                    Result = "ERROR",
                    Message = @"<div style='margin:20px'>
                                        <h6> <i class='fas fa-exclamation-triangle text-danger'></i> Oops! Something went wrong.</h6>
                                    <p>
                                        We will work on fixing that right away
                                    </p>
                                     </div>"
                });
                await context.Response.WriteAsync(json, Encoding.UTF8);
            }
            else
            {
                context.Response.Redirect("/Error/500");
            }
        }
    }
}