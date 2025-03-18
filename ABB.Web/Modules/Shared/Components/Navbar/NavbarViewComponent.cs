using System.Threading.Tasks;
using ABB.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.Navbar
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public NavbarViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _mediator.Send(new GetNavbarQuery());
            return View("_Navbar", model);
        }
    }
}