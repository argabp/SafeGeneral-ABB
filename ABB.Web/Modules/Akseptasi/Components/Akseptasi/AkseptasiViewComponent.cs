using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Akseptasi
{
    public class AkseptasiViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AkseptasiViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiParameterViewModel model)
        {
            var akseptasiViewModel = new AkseptasiViewModel();

            if (model == null) return View("_Akseptasi", akseptasiViewModel);
            
            var command = _mapper.Map<GetAkseptasiQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            if (result == null)
            {
                akseptasiViewModel.no_aks = "00000";
                akseptasiViewModel.no_updt = 0;
                akseptasiViewModel.no_renew = 0;
                akseptasiViewModel.thn_uw = 0;
                akseptasiViewModel.no_endt = "0";
                akseptasiViewModel.kd_grp_ttg = "9";
                akseptasiViewModel.kd_grp_bank = "2";
                akseptasiViewModel.st_pas = "O";
                akseptasiViewModel.kd_grp_pas = "5";
                akseptasiViewModel.kd_grp_bank = "6";
                akseptasiViewModel.pst_share_bgu = 100;
                akseptasiViewModel.faktor_prd = 100;
                akseptasiViewModel.flag_konv = "N";
                akseptasiViewModel.flag_reas = "N";
                akseptasiViewModel.kd_grp_mkt = "M";
                akseptasiViewModel.st_cover = "X";
                akseptasiViewModel.wpc = 0;
                akseptasiViewModel.st_aks = "1";
                akseptasiViewModel.flag_dis_fleet = "N";
                akseptasiViewModel.kd_thn = DateTime.Now.ToString("yy");
                akseptasiViewModel.kd_cb = model.kd_cb.Trim();
                akseptasiViewModel.kd_cob = string.Empty;
                akseptasiViewModel.kd_scob = string.Empty;
                akseptasiViewModel.tgl_mul_ptg = DateTime.Now;
                akseptasiViewModel.tgl_akh_ptg = DateTime.Now.AddYears(1);
                akseptasiViewModel.jk_wkt_ptg = Convert.ToInt16((akseptasiViewModel.tgl_akh_ptg - akseptasiViewModel.tgl_mul_ptg).TotalDays);
                akseptasiViewModel.tgl_ttd = DateTime.Now;
                akseptasiViewModel.tgl_closing = DateTime.Now;
                akseptasiViewModel.tgl_input = DateTime.Now;
                
                return View("_Akseptasi", akseptasiViewModel);
            }
                
            _mapper.Map(result, akseptasiViewModel);
            akseptasiViewModel.kd_cb = akseptasiViewModel.kd_cb.Trim();
            akseptasiViewModel.kd_cob = akseptasiViewModel.kd_cob.Trim();
            akseptasiViewModel.kd_scob = akseptasiViewModel.kd_scob.Trim();
            akseptasiViewModel.IsEdit = true;

            return View("_Akseptasi", akseptasiViewModel);
        }
    }
}