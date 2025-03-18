using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.OtherMotor
{
    public class OtherMotorViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OtherMotorViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            var akseptasiMotorViewModel = new AkseptasiResikoOtherMotorViewModel()
            {
                kd_cb = model.kd_cb.Trim(),
                kd_cob = model.kd_cob.Trim(),
                kd_scob = model.kd_scob.Trim(),
                kd_thn = model.kd_thn.Trim(),
                no_updt = model.no_updt,
                no_aks = model.no_aks
            };
            
            var motorCommand = _mapper.Map<GetAkseptasiOtherMotorQuery>(model);
            motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
            var motorResult = await _mediator.Send(motorCommand);

            if (motorResult == null)
            {
                akseptasiMotorViewModel.grp_jns_kend = "0011";
                akseptasiMotorViewModel.kd_guna = "000";
                akseptasiMotorViewModel.nilai_casco = 0;
                akseptasiMotorViewModel.nilai_tjh = 0;
                akseptasiMotorViewModel.nilai_tjp = 0;
                akseptasiMotorViewModel.nilai_pap = 0;
                akseptasiMotorViewModel.nilai_pad = 0;
                akseptasiMotorViewModel.pst_rate_prm = 0;
                akseptasiMotorViewModel.stn_rate_prm = 1;
                akseptasiMotorViewModel.pst_rate_hh = 0;
                akseptasiMotorViewModel.stn_rate_hh = 1;
                akseptasiMotorViewModel.nilai_rsk_sendiri = 0;
                akseptasiMotorViewModel.nilai_prm_casco = 0;
                akseptasiMotorViewModel.nilai_prm_tjh = 0;
                akseptasiMotorViewModel.nilai_prm_tjp = 0;
                akseptasiMotorViewModel.nilai_prm_pap = 0;
                akseptasiMotorViewModel.nilai_prm_pad = 0;
                akseptasiMotorViewModel.nilai_prm_hh = 0;
                akseptasiMotorViewModel.nilai_pap_med = 0;
                akseptasiMotorViewModel.nilai_pad_med = 0;
                akseptasiMotorViewModel.nilai_prm_pap_med = 0;
                akseptasiMotorViewModel.nilai_prm_pad_med = 0;
                akseptasiMotorViewModel.nilai_prm_aog = 0;
                akseptasiMotorViewModel.pst_rate_aog = 0;
                akseptasiMotorViewModel.stn_rate_prm = 1;
                akseptasiMotorViewModel.pst_rate_banjir = 0;
                akseptasiMotorViewModel.stn_rate_banjir = 1;
                akseptasiMotorViewModel.nilai_prm_banjir = 0;
                akseptasiMotorViewModel.validitas = "A";
                akseptasiMotorViewModel.pst_rate_trs = 0;
                akseptasiMotorViewModel.stn_rate_trs = 1;
                akseptasiMotorViewModel.nilai_prm_trs = 0;
                akseptasiMotorViewModel.pst_rate_tjh = 0;
                akseptasiMotorViewModel.stn_rate_tjh = 1;
                akseptasiMotorViewModel.pst_rate_tjp = 0;
                akseptasiMotorViewModel.stn_rate_tjp = 1;
                akseptasiMotorViewModel.pst_rate_pap = 0;
                akseptasiMotorViewModel.stn_rate_pap = 1;
                akseptasiMotorViewModel.pst_rate_pad = 0;
                akseptasiMotorViewModel.stn_rate_pad = 1;

                return View("_OtherMotor", akseptasiMotorViewModel);
            }
        
            _mapper.Map(motorResult, akseptasiMotorViewModel);
            akseptasiMotorViewModel.kd_cb = akseptasiMotorViewModel.kd_cb.Trim();
            akseptasiMotorViewModel.kd_cob = akseptasiMotorViewModel.kd_cob.Trim();
            akseptasiMotorViewModel.kd_scob = akseptasiMotorViewModel.kd_scob.Trim();
            
            return View("_OtherMotor", akseptasiMotorViewModel);
        }
    }
}