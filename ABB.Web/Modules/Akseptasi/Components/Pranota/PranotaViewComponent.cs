using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Pranota
{
    public class PranotaViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PranotaViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiPranotaParameterViewModel model)
        {
            var akseptasiPranotaViewModel = new AkseptasiPranotaViewModel();

            if (model == null) return View("_Pranota", akseptasiPranotaViewModel);
            
            var command = _mapper.Map<GetAkseptasiPranotaQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await _mediator.Send(command);

            if (result == null)
            {
                akseptasiPranotaViewModel.nilai_prm = 0;
                akseptasiPranotaViewModel.pst_dis = 0;
                akseptasiPranotaViewModel.nilai_dis = 0;
                akseptasiPranotaViewModel.pst_dis_fea = 0;
                akseptasiPranotaViewModel.nilai_dis_fea = 0;
                akseptasiPranotaViewModel.pst_dis_fleet = 0;
                akseptasiPranotaViewModel.nilai_dis_fleet = 0;
                akseptasiPranotaViewModel.nilai_insentif = 0;
                akseptasiPranotaViewModel.nilai_bia_pol = 0;
                akseptasiPranotaViewModel.nilai_bia_mat = 0;
                akseptasiPranotaViewModel.pst_kms = 0;
                akseptasiPranotaViewModel.nilai_kms = 0;
                akseptasiPranotaViewModel.pst_hf = 0;
                akseptasiPranotaViewModel.nilai_hf = 0;
                akseptasiPranotaViewModel.pst_kms_reas = 0;
                akseptasiPranotaViewModel.nilai_kms_reas = 0;
                akseptasiPranotaViewModel.nilai_bia_supl = 0;
                akseptasiPranotaViewModel.nilai_bia_pu = 0;
                akseptasiPranotaViewModel.nilai_bia_pbtl = 0;
                akseptasiPranotaViewModel.nilai_bia_form = 0;
                akseptasiPranotaViewModel.nilai_kl = 0;
                akseptasiPranotaViewModel.pst_pjk = 0;
                akseptasiPranotaViewModel.nilai_pjk = 0;
                akseptasiPranotaViewModel.nilai_ttl_kms = 0;
                akseptasiPranotaViewModel.nilai_ttl_bia = 0;
                akseptasiPranotaViewModel.nilai_ttl_ptg = 0;
                akseptasiPranotaViewModel.nilai_ttl_ptg = 0;
                akseptasiPranotaViewModel.kd_cb = model.kd_cb.Trim();
                akseptasiPranotaViewModel.kd_cob = model.kd_cob.Trim();
                akseptasiPranotaViewModel.kd_scob = model.kd_scob.Trim();
                akseptasiPranotaViewModel.kd_thn = model.kd_thn.Trim();
                akseptasiPranotaViewModel.no_updt = model.no_updt;
                akseptasiPranotaViewModel.no_aks = model.no_aks;
                
                return View("_Pranota", akseptasiPranotaViewModel);
            }
                
            _mapper.Map(result, akseptasiPranotaViewModel);
            akseptasiPranotaViewModel.kd_cb = akseptasiPranotaViewModel.kd_cb.Trim();
            akseptasiPranotaViewModel.kd_cob = akseptasiPranotaViewModel.kd_cob.Trim();
            akseptasiPranotaViewModel.kd_scob = akseptasiPranotaViewModel.kd_scob.Trim();
            akseptasiPranotaViewModel.IsEditPranota = true;

            return View("_Pranota", akseptasiPranotaViewModel);
        }
    }
}