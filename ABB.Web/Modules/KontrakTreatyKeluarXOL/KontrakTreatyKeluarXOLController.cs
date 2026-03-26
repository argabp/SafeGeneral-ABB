using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.KontrakTreatyKeluarXOLs.Commands;
using ABB.Application.KontrakTreatyKeluarXOLs.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KontrakTreatyKeluarXOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluarXOL
{
    public class KontrakTreatyKeluarXOLController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKontrakTreatyKeluarXOLs(GridRequest grid)
        {
            var result = await Mediator.Send(new GetKontrakTreatyKeluarXOLsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarXOLs(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_npps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarXOLsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_npps = kd_tty_npps
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new KontrakTreatyKeluarXOLViewModel()
            {
                pst_adj_onrpi = 0,
                nilai_bts_or = 0,
                nilai_bts_tty = 0,
                nilai_kurs = 0,
                pst_reinst = 0
            });
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_jns_sor, string kd_tty_npps)
        {
            var command = new GetKontrakTreatyKeluarXOLQuery()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_npps = kd_tty_npps
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            
            return View(Mapper.Map<KontrakTreatyKeluarXOLViewModel>(result));
        }
        
        public IActionResult AddDetail()
        {
            return View(new DetailKontrakTreatyKeluarXOLViewModel()
            {
                pst_com = 0,
                pst_share = 0
            });
        }
        
        public IActionResult EditDetail(string kd_grp_pas, string kd_rk_pas, decimal pst_com, 
            decimal pst_share, string? kd_grp_sb_bis, string? kd_rk_sb_bis)
        {
            var data = new DetailKontrakTreatyKeluarXOLViewModel()
            {
                kd_grp_pas = kd_grp_pas,
                kd_rk_pas = kd_rk_pas,
                pst_com = pst_com,
                pst_share = pst_share,
                kd_grp_sb_bis = kd_grp_sb_bis,
                kd_rk_sb_bis = kd_rk_sb_bis
            };
            
            return View(data);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKontrakTreatyKeluarXOL([FromBody] KontrakTreatyKeluarXOLViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKontrakTreatyKeluarXOLCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_npps) ? "Add" : "Edit" , model);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteKontrakTreatyKeluarXOL(string kd_cb, string kd_jns_sor, string kd_tty_npps)
        {
            try
            {
                var command = new DeleteKontrakTreatyKeluarXOLCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_npps = kd_tty_npps
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
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
        
        public JsonResult GetKodePas()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetKodeRekanan(string kd_grp, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp,
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetJenisSor()
        {
            var result = await Mediator.Send(new GetJenisSorPSTQuery());

            return Json(result);
        }

        public JsonResult GetSumberBisnis()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Broker", Value = "2" },
                new DropdownOptionDto() { Text = "-", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetNmTtyNpps(string kd_cob, string nm_jns_sor,
            string npps_layer, decimal thn_tty_npps)
        {
            var result = await Mediator.Send(new GetNmTtyNppsQuery()
            {
                kd_cob = kd_cob,
                nm_jns_sor = nm_jns_sor,
                npps_layer = npps_layer,
                thn_tty_npps = thn_tty_npps
            });

            return Json(result);
        }
    }
}