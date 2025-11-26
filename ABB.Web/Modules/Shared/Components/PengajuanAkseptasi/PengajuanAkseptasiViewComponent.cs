using System;
using System.Threading.Tasks;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Web.Modules.PengajuanAkseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.PengajuanAkseptasi
{
    public class PengajuanAkseptasiViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PengajuanAkseptasiViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(PengajuanAkseptasiModel model)
        {
            var command = _mapper.Map<GetPengajuanAkseptasiQuery>(model);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return View("_PengajuanAkseptasi", new PengajuanAkseptasiViewModel()
                {
                    tgl_akh_ptg = DateTime.Now,
                    tgl_mul_ptg = DateTime.Now,
                    tgl_pengajuan = DateTime.Now,
                    kd_grp_mkt = "M",
                    kd_grp_ttg = "9" ,
                    pst_pas1 = 0,
                    pst_pas2 = 0,
                    pst_pas3 = 0,
                    pst_pas4 = 0,
                    pst_pas5 = 0,
                    pst_dis = 0,
                    pst_kms = 0,
                    flag_approved = false,
                    kd_cb = model.kd_cb
                });
            }

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_tol = result.kd_tol.Trim();

            return View("_PengajuanAkseptasi", _mapper.Map<PengajuanAkseptasiViewModel>(result));
        }
    }
}