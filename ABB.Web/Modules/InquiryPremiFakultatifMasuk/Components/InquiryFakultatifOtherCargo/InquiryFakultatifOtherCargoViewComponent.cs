using System.Threading.Tasks;
using ABB.Application.Inquiries.Queries;
using ABB.Web.Modules.Inquiry.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.InquiryPremiFakultatifMasuk.Components.InquiryFakultatifOtherCargo
{
    public class InquiryFakultatifOtherCargoViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InquiryFakultatifOtherCargoViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            var akseptasiCargoViewModel = new InquiryResikoOtherCargoViewModel()
            {
                kd_cb = model.kd_cb.Trim(),
                kd_cob = model.kd_cob.Trim(),
                kd_scob = model.kd_scob.Trim(),
                kd_thn = model.kd_thn.Trim(),
                no_updt = model.no_updt,
                no_pol = model.no_pol
            };
            
            var cargoCommand = _mapper.Map<GetInquiryOtherCargoQuery>(model);
            cargoCommand.DatabaseName = "abb_kp00";
            var cargoResult = await _mediator.Send(cargoCommand);

            if (cargoResult == null)
            {
                akseptasiCargoViewModel.grp_kond = "003";

                return View("_InquiryFakultatifOtherCargo", akseptasiCargoViewModel);
            }
        
            _mapper.Map(cargoResult, akseptasiCargoViewModel);
            akseptasiCargoViewModel.kd_cb = akseptasiCargoViewModel.kd_cb.Trim();
            akseptasiCargoViewModel.kd_cob = akseptasiCargoViewModel.kd_cob.Trim();
            akseptasiCargoViewModel.kd_scob = akseptasiCargoViewModel.kd_scob.Trim();
            
            return View("_InquiryFakultatifOtherCargo", akseptasiCargoViewModel);
        }
    }
}