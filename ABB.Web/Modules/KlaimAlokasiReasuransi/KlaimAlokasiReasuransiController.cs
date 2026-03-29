using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.KlaimAlokasiReasuransis.Commands;
using ABB.Application.KlaimAlokasiReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KlaimAlokasiReasuransi.Models;
using Microsoft.AspNetCore.Mvc;
using GetRekananSorQuery = ABB.Application.KlaimAlokasiReasuransis.Queries.GetRekananSorQuery;

namespace ABB.Web.Modules.KlaimAlokasiReasuransi
{
    public class KlaimAlokasiReasuransiController : AuthorizedBaseController
    {
        private readonly string DatabaseName = "abb_pst";
        private readonly List<DropdownOptionDto> _tipeMutasi = new List<DropdownOptionDto>()
        {
            new DropdownOptionDto() { Text = "PLA", Value = "P" },
            new DropdownOptionDto() { Text = "DLA", Value = "D" },
            new DropdownOptionDto() { Text = "Beban", Value = "B" },
            new DropdownOptionDto() { Text = "Recovery", Value = "R" }
        };

        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetMutasiKlaims(GridRequest grid)
        {
            var result = await Mediator.Send(new GetMutasiKlaimsQuery()
            {
                Grid = grid
            });

            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> GetKlaimAlokasiReasuransis(GridRequest grid,
            string kd_cb,
            string kd_cob,
            string kd_scob,
            string kd_thn,
            string no_kl,
            short no_mts)
        {
            var result = await Mediator.Send(new GetKlaimAlokasiReasuransisQuery()
            {
                kd_cb = kd_cb,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                Grid = grid,
            });

            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> GetKlaimAlokasiReasuransiXLs(GridRequest grid,
            string kd_cb,
            string kd_cob,
            string kd_scob,
            string kd_thn,
            string no_kl,
            short no_mts)
        {
            var result = await Mediator.Send(new GetKlaimAlokasiReasuransiXLsQuery()
            {
                kd_cb = kd_cb,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                Grid = grid,
            });

            return Json(result);
        }
         
        public async Task<IActionResult> ClosingKlaimAlokasiReasuransi([FromBody] MutasiKlaimModel model)
        {
            try
            {
                var command = Mapper.Map<ClosingKlaimAlokasiReasuransiCommand>(model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        public async Task<IActionResult> AlokasiReasuransi([FromBody] MutasiKlaimModel model)
        {
            try
            {
                var command = Mapper.Map<AlokasiReasCommand>(model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangPSTQuery());

            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
            
            return Json(cobs);
        }

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBPSTQuery()
            {
                kd_cob = kd_cob
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetMataUang(string kd_cb)
        {
            var result = await Mediator.Send(new GetMataUangByKodeCabangQuery()
            {
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public JsonResult GetTipeMutasi()
        {
            return Json(_tipeMutasi);
        }
        
        public JsonResult GetValiditas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Klaim Normal", Value = "A" },
                new DropdownOptionDto() { Text = "Ex Gratia", Value = "B" },
                new DropdownOptionDto() { Text = "Salvage", Value = "C" },
                new DropdownOptionDto() { Text = "Subrogari", Value = "D" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetUsers()
        {
            var users = await Mediator.Send(new GetUsersQuery());
            
            return Json(users);
        }
        
        public async Task<IActionResult> View(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var mutasiKlaim = await Mediator.Send(new GetMutasiKlaimQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts
            });

            mutasiKlaim.kd_cb = mutasiKlaim.kd_cb.Trim();
            mutasiKlaim.kd_cob = mutasiKlaim.kd_cob.Trim();
            mutasiKlaim.kd_scob = mutasiKlaim.kd_scob.Trim();
            mutasiKlaim.validitas = mutasiKlaim.validitas.Trim();
            
            return PartialView(Mapper.Map<MutasiKlaimViewModel>(mutasiKlaim));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteSOL([FromBody] DeleteKlaimAlokasiReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKlaimAlokasiReasuransiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Klaim Alokasi Reasuransi"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteSOLXOL([FromBody] DeleteKlaimAlokasiReasuransiXLViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteKlaimAlokasiReasuransiXLCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Klaim Alokasi Reasuransi XL"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<IActionResult> AddSOL(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts, decimal nilai_ttl_kl)
        {
            var klaimAlokasiReasuransiViewModel = new KlaimAlokasiReasuransiViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                nilai_ttl_kl = nilai_ttl_kl
            };
            
            return PartialView(klaimAlokasiReasuransiViewModel);
        }

        public async Task<IActionResult> EditSOL(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            string kd_jns_sor, string kd_grp_sor, string kd_rk_sor)
        {
            var klaimAlokasiReasuransi = await Mediator.Send(new GetKlaimAlokasiReasuransiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                kd_jns_sor = kd_jns_sor,
                kd_grp_sor = kd_grp_sor,
                kd_rk_sor = kd_rk_sor
            });

            klaimAlokasiReasuransi.kd_cb = klaimAlokasiReasuransi.kd_cb.Trim();
            klaimAlokasiReasuransi.kd_cob = klaimAlokasiReasuransi.kd_cob.Trim();
            klaimAlokasiReasuransi.kd_scob = klaimAlokasiReasuransi.kd_scob.Trim();
            klaimAlokasiReasuransi.kd_jns_sor = klaimAlokasiReasuransi.kd_jns_sor.Trim();
            klaimAlokasiReasuransi.kd_grp_sor = klaimAlokasiReasuransi.kd_grp_sor.Trim();
            klaimAlokasiReasuransi.kd_rk_sor = klaimAlokasiReasuransi.kd_rk_sor.Trim();
            
            return PartialView(Mapper.Map<KlaimAlokasiReasuransiViewModel>(klaimAlokasiReasuransi));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveSOL([FromBody] KlaimAlokasiReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKlaimAlokasiReasuransiCommand>(model);
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        public async Task<IActionResult> AddSOLXOL(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var klaimAlokasiReasuransiXLViewModel = new KlaimAlokasiReasuransiXLViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                kd_jns_sor = "XOL"
            };
            
            return PartialView(klaimAlokasiReasuransiXLViewModel);
        }

        public async Task<IActionResult> EditSOLXOL(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            string kd_jns_sor, string kd_kontr)
        {
            var klaimAlokasiReasuransi = await Mediator.Send(new GetKlaimAlokasiReasuransiXLQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                kd_jns_sor = kd_jns_sor,
                kd_kontr = kd_kontr
            });

            klaimAlokasiReasuransi.kd_cb = klaimAlokasiReasuransi.kd_cb.Trim();
            klaimAlokasiReasuransi.kd_cob = klaimAlokasiReasuransi.kd_cob.Trim();
            klaimAlokasiReasuransi.kd_scob = klaimAlokasiReasuransi.kd_scob.Trim();
            klaimAlokasiReasuransi.kd_jns_sor = klaimAlokasiReasuransi.kd_jns_sor.Trim();
            klaimAlokasiReasuransi.kd_kontr = klaimAlokasiReasuransi.kd_kontr.Trim();
            
            return PartialView(Mapper.Map<KlaimAlokasiReasuransiXLViewModel>(klaimAlokasiReasuransi));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveSOLXOL([FromBody] KlaimAlokasiReasuransiXLViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKlaimAlokasiReasuransiXLCommand>(model);
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    
        [HttpGet]
        public async Task<ActionResult> GetJenisSor()
        {
            try
            {
                var command = new GetJenisSorPSTQuery();
                
                var result = await Mediator.Send(command);
                
                return Json(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRekananSor(string? jns_lookup, 
            string kd_cb, string kd_jns_sor, string kd_cob)
        {
            try
            {
                if (jns_lookup == null)
                    return Ok();
                
                var command = new GetRekananSorQuery()
                {
                    jns_lookup = jns_lookup,
                    kd_cb = kd_cb,
                    kd_jns_sor = kd_jns_sor,
                    kd_cob = kd_cob
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
        public async Task<ActionResult> GetKontrakSOR(string kd_cb, string kd_cob)
        {
            try
            {
                var command = new GetKontrakSORsQuery()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob
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
        public async Task<ActionResult> GetGroupAndRekananSor(string kd_jns_sor)
        {
            try
            {
                var command = new GetGroupAndRekananSorQuery()
                {
                    kd_jns_sor = kd_jns_sor
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
        public async Task<ActionResult> GenerateNilaiKlaim(decimal nilai_ttl_kl, decimal pst_share)
        {
            try
            {
                var command = new GenerateNilaiKlaimQuery()
                {
                    nilai_ttl_kl = nilai_ttl_kl,
                    pst_share = pst_share
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}