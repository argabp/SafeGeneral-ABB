using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using ABB.Web.Modules.Inquiry.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryCoverage
{
    public class InquiryCoverageViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public InquiryCoverageViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.kd_cob) && string.IsNullOrWhiteSpace(model.kd_scob))
            {
                return View("~/Modules/Inquiry/Views/Empty.cshtml");
            }
            
            return View("_InquiryCoverage", new InquiryResikoCoverageViewModel());
        }
    }
}