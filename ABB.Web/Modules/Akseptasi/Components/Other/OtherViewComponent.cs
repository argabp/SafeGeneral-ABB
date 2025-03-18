using System;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Other
{
    public class OtherViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OtherViewComponent(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            
            switch (model.kd_cob.Trim())
            {
                case "B":
                    var akseptasiBondingViewModel = new AkseptasiResikoOtherBondingViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var bondingCommand = _mapper.Map<GetAkseptasiOtherBondingQuery>(model);
                    bondingCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var bondingResult = await _mediator.Send(bondingCommand);

                    if (bondingResult == null)
                    {
                        akseptasiBondingViewModel.grp_obl = "004";
                        akseptasiBondingViewModel.grp_kontr = "005";
                        akseptasiBondingViewModel.kd_rumus = "F";
                        akseptasiBondingViewModel.grp_jns_pekerjaan = "012";
                        akseptasiBondingViewModel.kd_grp_obl = "O";
                        akseptasiBondingViewModel.kd_grp_surety = "5";
                    
                        return View("_OtherBonding", akseptasiBondingViewModel);
                    }
                
                    _mapper.Map(bondingResult, akseptasiBondingViewModel);
                    akseptasiBondingViewModel.kd_cb = akseptasiBondingViewModel.kd_cb.Trim();
                    akseptasiBondingViewModel.kd_cob = akseptasiBondingViewModel.kd_cob.Trim();
                    akseptasiBondingViewModel.kd_scob = akseptasiBondingViewModel.kd_scob.Trim();

                    return View("_OtherBonding", akseptasiBondingViewModel);
                case "C":
                    return View("_OtherCargo", model);
                case "M":
                    return View("_OtherMotor", model);
                case "F":
                    var akseptasiFireViewModel = new AkseptasiResikoOtherFireViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var fireCommand = _mapper.Map<GetAkseptasiOtherFireQuery>(model);
                    fireCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var fireResult = await _mediator.Send(fireCommand);

                    if (fireResult == null)
                    {
                        akseptasiFireViewModel.kd_penerangan = "1";
                        akseptasiFireViewModel.kategori_gd = "E";

                        return View("_OtherFire", akseptasiFireViewModel);
                    }
                
                    _mapper.Map(fireResult, akseptasiFireViewModel);
                    akseptasiFireViewModel.kd_cb = akseptasiFireViewModel.kd_cb.Trim();
                    akseptasiFireViewModel.kd_cob = akseptasiFireViewModel.kd_cob.Trim();
                    akseptasiFireViewModel.kd_scob = akseptasiFireViewModel.kd_scob.Trim();
                    
                    return View("_OtherFire", akseptasiFireViewModel);
                case "H":
                    var akseptasiHullViewModel = new AkseptasiResikoOtherHullViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var hullCommand = _mapper.Map<GetAkseptasiOtherFireQuery>(model);
                    hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var hullResult = await _mediator.Send(hullCommand);

                    if (hullResult == null)
                    {
                        return View("_OtherHull", akseptasiHullViewModel);
                    }
                
                    _mapper.Map(hullResult, akseptasiHullViewModel);
                    akseptasiHullViewModel.kd_cb = akseptasiHullViewModel.kd_cb.Trim();
                    akseptasiHullViewModel.kd_cob = akseptasiHullViewModel.kd_cob.Trim();
                    akseptasiHullViewModel.kd_scob = akseptasiHullViewModel.kd_scob.Trim();
                    akseptasiHullViewModel.merk_kapal = akseptasiHullViewModel.merk_kapal?.Trim();
                    akseptasiHullViewModel.kd_kapal = akseptasiHullViewModel.kd_kapal.Trim();
                    
                    return View("_OtherHull", akseptasiHullViewModel);
                case "P":
                    var akseptasiPaViewModel = new AkseptasiResikoOtherPAViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var paCommand = _mapper.Map<GetAkseptasiOtherPAQuery>(model);
                    paCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var paResult = await _mediator.Send(paCommand);

                    if (paResult == null)
                    {
                        akseptasiPaViewModel.thn_akh = "1";
                        akseptasiPaViewModel.nilai_harga_ptg = 0;
                        akseptasiPaViewModel.pst_rate_std = 0;
                        akseptasiPaViewModel.pst_rate_bjr = 0;
                        akseptasiPaViewModel.pst_rate_tl = 0;
                        akseptasiPaViewModel.pst_rate_gb = 0;
                        akseptasiPaViewModel.nilai_prm_std = 0;
                        akseptasiPaViewModel.nilai_prm_bjr = 0;
                        akseptasiPaViewModel.nilai_prm_tl = 0;
                        akseptasiPaViewModel.nilai_bia_adm = 0;
                        akseptasiPaViewModel.nilai_prm_btn = 0;
                        akseptasiPaViewModel.flag_std = "2";
                        akseptasiPaViewModel.flag_bjr = "2";
                        akseptasiPaViewModel.flag_tl = "1";
                        akseptasiPaViewModel.flag_gb = "1";
                        akseptasiPaViewModel.pst_rate_phk = 0;
                        akseptasiPaViewModel.nilai_prm_phk = 0;
                        akseptasiPaViewModel.nilai_bia_mat = 0;
                        akseptasiPaViewModel.nilai_ptg_std = 0;
                        akseptasiPaViewModel.nilai_ptg_bjr = 0;
                        akseptasiPaViewModel.nilai_ptg_tl = 0;
                        akseptasiPaViewModel.nilai_ptg_gb = 0;
                        akseptasiPaViewModel.nilai_ptg_hh = 0;
                        akseptasiPaViewModel.stn_rate_std = 10;
                        akseptasiPaViewModel.stn_rate_bjr = 10;
                        akseptasiPaViewModel.stn_rate_gb = 10;
                        akseptasiPaViewModel.stn_rate_tl = 10;
                        akseptasiPaViewModel.stn_rate_phk = 0;
                        akseptasiPaViewModel.kd_grp_asj = "5";
                    
                        return View("_OtherPA", akseptasiPaViewModel);
                    }
                
                    _mapper.Map(paResult, akseptasiPaViewModel);
                    akseptasiPaViewModel.kd_cb = akseptasiPaViewModel.kd_cb.Trim();
                    akseptasiPaViewModel.kd_cob = akseptasiPaViewModel.kd_cob.Trim();
                    akseptasiPaViewModel.kd_scob = akseptasiPaViewModel.kd_scob.Trim();

                    return View("_OtherPA", akseptasiPaViewModel);
                default:
                    return View("_DefaultOther");
            }
        }
    }
}