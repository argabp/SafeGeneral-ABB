using System;
using System.Threading.Tasks;
using ABB.Application.NotaResiko.Queries;
using ABB.Web.Modules.NotaRisiko.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaRisiko.Components.SubsequentPAA
{
    public class SubsequentPAAViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
    
        public SubsequentPAAViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(NotaRisikoViewModel model)
        {
            var result = await _mediator.Send(new GetSubsequentPAAQuery() { Id = model.Id });

            return View("_SubsequentPAA", result);
        }
    }
}