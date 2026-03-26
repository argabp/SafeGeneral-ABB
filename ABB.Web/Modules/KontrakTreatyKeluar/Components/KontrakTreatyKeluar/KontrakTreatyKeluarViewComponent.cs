using System;
using System.Threading.Tasks;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Web.Modules.KontrakTreatyKeluar.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.KontrakTreatyKeluar
{
    public class KontrakTreatyKeluarViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public KontrakTreatyKeluarViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(KontrakTreatyKeluarParameterViewModel model)
        {
            var kontrakTreatyKeluarViewModel = new KontrakTreatyKeluarViewModel();

            if (model == null) return View("_KontrakTreatyKeluar", kontrakTreatyKeluarViewModel);
            
            var command = _mapper.Map<GetKontrakTreatyKeluarQuery>(model);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                kontrakTreatyKeluarViewModel.frek_lap = "B";
                kontrakTreatyKeluarViewModel.faktor_sor = "N";
                kontrakTreatyKeluarViewModel.nilai_bts_cash_call = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_cash_call_idr = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_or = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_or_idr = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_tty = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_tty_idr = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_cash_pla = 0;
                kontrakTreatyKeluarViewModel.nilai_bts_cash_pla_idr = 0;
                kontrakTreatyKeluarViewModel.pst_kms_reas = 0;
                kontrakTreatyKeluarViewModel.pst_share_reas = 0;
                kontrakTreatyKeluarViewModel.pst_hf = 0;
                kontrakTreatyKeluarViewModel.pst_profit_comm = 0;
                kontrakTreatyKeluarViewModel.tgl_akh_ptg = DateTime.Now;
                kontrakTreatyKeluarViewModel.tgl_mul_ptg = DateTime.Now;
                
                return View("_KontrakTreatyKeluar", kontrakTreatyKeluarViewModel);
            }
                
            _mapper.Map(result, kontrakTreatyKeluarViewModel);
            kontrakTreatyKeluarViewModel.kd_cb = kontrakTreatyKeluarViewModel.kd_cb.Trim();
            kontrakTreatyKeluarViewModel.kd_cob = kontrakTreatyKeluarViewModel.kd_cob.Trim();
            kontrakTreatyKeluarViewModel.kd_jns_sor = kontrakTreatyKeluarViewModel.kd_jns_sor.Trim();
            kontrakTreatyKeluarViewModel.faktor_sor = kontrakTreatyKeluarViewModel.faktor_sor?.Trim();
            kontrakTreatyKeluarViewModel.frek_lap = kontrakTreatyKeluarViewModel.frek_lap.Trim();
            kontrakTreatyKeluarViewModel.IsEdit = true;

            return View("_KontrakTreatyKeluar", kontrakTreatyKeluarViewModel);
        }
    }
}