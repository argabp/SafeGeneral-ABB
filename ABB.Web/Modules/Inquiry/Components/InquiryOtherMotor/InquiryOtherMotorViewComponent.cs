using System.Threading.Tasks;
using ABB.Application.Inquiries.Queries;
using ABB.Web.Modules.Inquiry.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryOtherMotor
{
    public class InquiryOtherMotorViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public InquiryOtherMotorViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            var inquiryMotorViewModel = new InquiryResikoOtherMotorViewModel()
            {
                kd_cb = model.kd_cb.Trim(),
                kd_cob = model.kd_cob.Trim(),
                kd_scob = model.kd_scob.Trim(),
                kd_thn = model.kd_thn.Trim(),
                no_updt = model.no_updt,
                no_pol = model.no_pol
            };
            
            var motorCommand = _mapper.Map<GetInquiryOtherMotorQuery>(model);
            motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
            var motorResult = await _mediator.Send(motorCommand);

            if (motorResult == null)
            {
                inquiryMotorViewModel.grp_jns_kend = "001";
                inquiryMotorViewModel.kd_guna = "000";
                inquiryMotorViewModel.nilai_casco = 0;
                inquiryMotorViewModel.nilai_tjh = 0;
                inquiryMotorViewModel.nilai_tjp = 0;
                inquiryMotorViewModel.nilai_pap = 0;
                inquiryMotorViewModel.nilai_pad = 0;
                inquiryMotorViewModel.pst_rate_prm = 0;
                inquiryMotorViewModel.stn_rate_prm = 1;
                inquiryMotorViewModel.pst_rate_hh = 0;
                inquiryMotorViewModel.stn_rate_hh = 1;
                inquiryMotorViewModel.nilai_rsk_sendiri = 0;
                inquiryMotorViewModel.nilai_prm_casco = 0;
                inquiryMotorViewModel.nilai_prm_tjh = 0;
                inquiryMotorViewModel.nilai_prm_tjp = 0;
                inquiryMotorViewModel.nilai_prm_pap = 0;
                inquiryMotorViewModel.nilai_prm_pad = 0;
                inquiryMotorViewModel.nilai_prm_hh = 0;
                inquiryMotorViewModel.nilai_pap_med = 0;
                inquiryMotorViewModel.nilai_pad_med = 0;
                inquiryMotorViewModel.nilai_prm_pap_med = 0;
                inquiryMotorViewModel.nilai_prm_pad_med = 0;
                inquiryMotorViewModel.nilai_prm_aog = 0;
                inquiryMotorViewModel.pst_rate_aog = 0;
                inquiryMotorViewModel.stn_rate_prm = 1;
                inquiryMotorViewModel.pst_rate_banjir = 0;
                inquiryMotorViewModel.stn_rate_banjir = 1;
                inquiryMotorViewModel.nilai_prm_banjir = 0;
                inquiryMotorViewModel.validitas = "A";
                inquiryMotorViewModel.pst_rate_trs = 0;
                inquiryMotorViewModel.stn_rate_trs = 1;
                inquiryMotorViewModel.nilai_prm_trs = 0;
                inquiryMotorViewModel.pst_rate_tjh = 0;
                inquiryMotorViewModel.stn_rate_tjh = 1;
                inquiryMotorViewModel.pst_rate_tjp = 0;
                inquiryMotorViewModel.stn_rate_tjp = 1;
                inquiryMotorViewModel.pst_rate_pap = 0;
                inquiryMotorViewModel.stn_rate_pap = 1;
                inquiryMotorViewModel.pst_rate_pad = 0;
                inquiryMotorViewModel.stn_rate_pad = 1;
                inquiryMotorViewModel.kd_endt = "I";

                return View("_InquiryOtherMotor", inquiryMotorViewModel);
            }
        
            _mapper.Map(motorResult, inquiryMotorViewModel);
            inquiryMotorViewModel.kd_cb = inquiryMotorViewModel.kd_cb.Trim();
            inquiryMotorViewModel.kd_cob = inquiryMotorViewModel.kd_cob.Trim();
            inquiryMotorViewModel.kd_scob = inquiryMotorViewModel.kd_scob.Trim();
            
            return View("_InquiryOtherMotor", inquiryMotorViewModel);
        }
    }
}