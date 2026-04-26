using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Alokasis.Commands;
using ABB.Application.Alokasis.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Akseptasi.Models;
using ABB.Web.Modules.Alokasi.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Alokasi
{
    public class AlokasiController : AuthorizedBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetSORs(GridRequest grid)
        {
            var result = await Mediator.Send(new GetSORsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetAlokasis(GridRequest grid, string kd_cb, string kd_cob, 
            string kd_scob, string kd_thn, Int16 no_updt, string no_pol)
        {
            var result = await Mediator.Send(new GetAlokasisQuery()
            {
                Grid = grid, 
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_pol = no_pol
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetDetailAlokasis(GridRequest grid, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, Int16 no_updt, Int16 no_rsk, string kd_endt, string no_pol)
        {
            var result = await Mediator.Send(new GetDetailAlokasisQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_pol = no_pol
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAlokasi([FromBody] AlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAlokasiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditDetailAlokasi", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailAlokasi([FromBody] DetailAlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailAlokasiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditDetailAlokasi", model);
        }
        
        [HttpGet]
        public IActionResult AddAlokasi()
        {
            var viewModel = new AlokasiViewModel();
            
            return PartialView(viewModel);
        }
        
        [HttpGet]
        public IActionResult AddDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, Int16 no_rsk)
        {
            var viewModel = new DetailAlokasiViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_rsk = no_rsk;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.no_updt_reas = 0;
            viewModel.kd_grp_sb_bis = "5";
            viewModel.no_pol = "00000";
            
            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, short no_updt, string no_pol)
        {
            var sor = new SORViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_pol = no_pol,
                IsViewOnly = false
            };
            
            return PartialView(sor);
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            var sor = await Mediator.Send(new GetDetailAlokasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                kd_grp_sor = kd_grp_sor,
                kd_jns_sor = kd_jns_sor,
                kd_rk_sor = kd_rk_sor,
                no_pol = no_pol,
                no_updt_reas = no_updt_reas,
                kd_grp_sb_bis = kd_grp_sb_bis
            });
            
            return PartialView(Mapper.Map<DetailAlokasiViewModel>(sor));
        }

        [HttpGet]
        public async Task<IActionResult> EditAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_updt_reas)
        {
            var resiko = await Mediator.Send(new GetAlokasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_updt_reas = no_updt_reas,
            });
            
            return PartialView(Mapper.Map<AlokasiViewModel>(resiko));
        }

        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, short no_updt, string no_pol)
        {
            var sor = new SORViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_pol = no_pol,
                IsViewOnly = true
            };
            
            return PartialView(sor);
        }

        [HttpGet]
        public async Task<IActionResult> ViewAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_updt_reas)
        {
            var resiko = await Mediator.Send(new GetAlokasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_pol = no_pol,
                no_updt_reas = no_updt_reas,
            });
            
            return PartialView(Mapper.Map<AlokasiViewModel>(resiko));
        }

        [HttpGet]
        public async Task<IActionResult> ViewDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            var sor = await Mediator.Send(new GetDetailAlokasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                kd_grp_sor = kd_grp_sor,
                kd_jns_sor = kd_jns_sor,
                kd_rk_sor = kd_rk_sor,
                no_pol = no_pol,
                no_updt_reas = no_updt_reas,
                kd_grp_sb_bis = kd_grp_sb_bis
            });
            
            return PartialView(Mapper.Map<DetailAlokasiViewModel>(sor));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_updt_reas)
        {
            try
            {
                var command = new DeleteAlokasiCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    no_pol = no_pol,
                    no_updt_reas = no_updt_reas,
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            try
            {
                var command = new DeleteDetailAlokasiCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    kd_grp_sor = kd_grp_sor,
                    kd_jns_sor = kd_jns_sor,
                    kd_rk_sor = kd_rk_sor,
                    no_pol = no_pol,
                    no_updt_reas = no_updt_reas,
                    kd_grp_sb_bis = kd_grp_sb_bis
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> ProsesAlokasi([FromBody] ProsesAlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesAlokasiCommand>(model);
                var result =  await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetGroupAndRekananSor(string kd_jns_sor, string kd_cob, string kd_cb,
            decimal thn_uw, decimal nilai_ttl_ptg, decimal nilai_prm)
        {
            try
            {
                var command = new GetGroupAndRekananSorQuery()
                {
                    kd_jns_sor = kd_jns_sor,
                    kd_cob = kd_cob,
                    kd_cb = kd_cb,
                    thn_uw = thn_uw,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetJenisSor()
        {
            try
            {
                var command = new GetJenisSorQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                
                var result = await Mediator.Send(command);
                
                return Json(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRekananSor(string? jns_lookup)
        {
            try
            {
                if (jns_lookup == null)
                    return Ok();
                
                var command = new GetRekananSorPSTQuery()
                {
                    jns_lookup = jns_lookup
                };
                
                var result = await Mediator.Send(command);
                
                return Json(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetShareAndPremiReas(decimal nilai_ttl_ptg_reas, string kd_jns_sor,
            decimal nilai_ttl_ptg, decimal nilai_prm, decimal net_prm)
        {
            try
            {
                var command = new GetShareAndPremiReasQuery()
                {
                    nilai_ttl_ptg_reas = nilai_ttl_ptg_reas,
                    kd_jns_sor = kd_jns_sor,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm,
                    net_prm = net_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTTLAndPremiReas(decimal pst_share, decimal nilai_prm_reas,
            string kd_jns_sor, decimal nilai_prm, decimal net_prm, decimal nilai_ttl_ptg)
        {
            try
            {
                var command = new GetTTLAndPremiReasQuery()
                {
                    pst_share = pst_share,
                    nilai_prm_reas = nilai_prm_reas,
                    kd_jns_sor = kd_jns_sor,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm,
                    net_prm = net_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetKmsReas(decimal pst_kms_reas, decimal nilai_prm_reas,
            decimal nilai_adj_reas)
        {
            try
            {
                var command = new GetKmsReasQuery()
                {
                    pst_kms_reas = pst_kms_reas,
                    nilai_prm_reas = nilai_prm_reas,
                    nilai_adj_reas = nilai_adj_reas
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAdjReas(decimal pst_share, decimal pst_adj_reas,
            byte stn_adj_reas, decimal pst_kms, decimal nilai_prm_reas, decimal nilai_prm,
            decimal pst_rate_prm, byte stn_rate_prm, string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk)
        {
            try
            {
                var command = new GetAdjReasQuery()
                {
                    pst_share = pst_share,
                    pst_adj_reas = pst_adj_reas,
                    stn_adj_reas = stn_adj_reas,
                    pst_kms = pst_kms,
                    nilai_prm_reas = nilai_prm_reas,
                    nilai_prm = nilai_prm,
                    pst_rate_prm = pst_rate_prm,
                    stn_rate_prm = stn_rate_prm,
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_pol = no_pol,
                    no_updt = no_updt,
                    no_rsk = no_rsk
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var mataUang = await Mediator.Send(new GetMataUangPSTQuery());
            
            return Json(mataUang);
        }
        
        public JsonResult GetRatePremi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "%", Value = "1" },
                new DropdownOptionDto() { Text = "%o", Value = "10" }
            };

            return Json(result);
        }
    }
}