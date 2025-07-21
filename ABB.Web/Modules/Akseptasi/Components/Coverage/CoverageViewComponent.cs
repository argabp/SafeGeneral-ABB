using System.Threading.Tasks;
using ABB.Application.Akseptasis.Commands;
using ABB.Web.Modules.Akseptasi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Coverage
{
    public class CoverageViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public CoverageViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.kd_cob) && string.IsNullOrWhiteSpace(model.kd_scob))
            {
                return View("~/Modules/Akseptasi/Views/Empty.cshtml");
            }
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_Coverage", new AkseptasiResikoCoverageViewModel());
        }
    }
}