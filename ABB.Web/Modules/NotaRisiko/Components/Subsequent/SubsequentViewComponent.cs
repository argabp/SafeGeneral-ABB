using System.Threading.Tasks;
using ABB.Application.NotaResiko.Queries;
using ABB.Web.Modules.NotaRisiko.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaRisiko.Components.Subsequent
{
    public class SubsequentViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
    
        public SubsequentViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(NotaRisikoViewModel model)
        {
            var result = await _mediator.Send(new GetSubsequentQuery() { Id = model.Id });
            
            return View("_Subsequent", result);
        }
    }
}