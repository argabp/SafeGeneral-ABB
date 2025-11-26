using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryOther
{
    public class InquiryOtherViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InquiryOtherViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            return View("~/Modules/Inquiry/Views/Empty.cshtml");
        }
    }
}