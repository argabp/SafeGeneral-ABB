using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ABB.Web.Extensions
{
    public static class HttpContextExtension
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static string GetControllerName(this HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
        }

        public static string GetActionName(this HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ActionName;
        }

        public static string GetIpAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
    }
}