using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using ABB.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ABB.Web.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILog _log;

        public AuthorizationFilter(IHttpContextAccessor httpContextAccessor, ILog log)
        {
            _httpContextAccessor = httpContextAccessor;
            _log = log;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var controller = (string)context.RouteData.Values["Controller"];
                var action = (string)context.RouteData.Values["Action"];
                var controllerDashAction = $"{controller}-{action}";
                if (context.HasAllowAnonymous()) return;
                if (context.HttpContext.User.Identity.Name != null)
                {
                    if (SkippedController(controller, action)) return;
                    var mediator = context.HttpContext.RequestServices.GetService<ISender>();
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var allowed = mediator.Send(new CheckAuthorizationCommand()
                        { UserID = userId, Controller = controller, Action = action }).Result;
                    if (!allowed)
                    {
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, MethodBase.GetCurrentMethod());
                context.Result = new RedirectToActionResult("500", "Error", null);
            }
        }

        private bool SkippedController(string controller, string action)
        {
            //dictionary<skipped controller, exception action>
            var skippedController = new Dictionary<string, string[]>();
            skippedController.Add("Home", new string[] { });
            skippedController.Add("Account", new string[] { });

            foreach (var ctrl in skippedController)
            {
                if (ctrl.Key == controller && ctrl.Value.Length == 0) return true;
                else if (ctrl.Key == controller && ctrl.Value.Length > 0)
                {
                    foreach (var ctrlAction in ctrl.Value)
                    {
                        if (action == ctrlAction) return false;
                        else return true;
                    }
                }
            }

            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeUserAttribute : TypeFilterAttribute
    {
        public AuthorizeUserAttribute() : base(typeof(AuthorizationFilter))
        {
        }
    }
}