using System.Threading.Tasks;
using ABB.Application.Navigations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ABB.Web.Modules.Shared.Components.Navigation
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IActionContextAccessor _http;

        public NavigationViewComponent(IMediator mediator, IActionContextAccessor http)
        {
            _mediator = mediator;
            _http = http;
        }

        public async Task<IViewComponentResult> InvokeAsync(string LoggedInUsername)
        {
            var moduleId = HttpContext.Request.Cookies["Module"];
            var navigation = await _mediator.Send(new GetLoggedInNavigationQuery() { Username = LoggedInUsername, ModuleId = moduleId});
            return View("_Navigation", navigation);
        }
    }
}