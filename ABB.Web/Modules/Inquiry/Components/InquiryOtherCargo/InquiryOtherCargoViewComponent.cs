using System.Threading.Tasks;
using ABB.Application.Inquiries.Queries;
using ABB.Web.Modules.Inquiry.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryOtherCargo
{
    public class InquiryOtherCargoViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InquiryOtherCargoViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            var inquiryCargoViewModel = new InquiryResikoOtherCargoViewModel()
            {
                kd_cb = model.kd_cb.Trim(),
                kd_cob = model.kd_cob.Trim(),
                kd_scob = model.kd_scob.Trim(),
                kd_thn = model.kd_thn.Trim(),
                no_updt = model.no_updt,
                no_pol = model.no_pol
            };
            
            var cargoCommand = _mapper.Map<GetInquiryOtherCargoQuery>(model);
            cargoCommand.DatabaseName = Request.Cookies["DatabaseValue"];
            var cargoResult = await _mediator.Send(cargoCommand);

            if (cargoResult == null)
            {
                inquiryCargoViewModel.grp_kond = "003";

                return View("_InquiryOtherCargo", inquiryCargoViewModel);
            }
        
            _mapper.Map(cargoResult, inquiryCargoViewModel);
            inquiryCargoViewModel.kd_cb = inquiryCargoViewModel.kd_cb.Trim();
            inquiryCargoViewModel.kd_cob = inquiryCargoViewModel.kd_cob.Trim();
            inquiryCargoViewModel.kd_scob = inquiryCargoViewModel.kd_scob.Trim();
            
            return View("_InquiryOtherCargo", inquiryCargoViewModel);
        }
    }
}