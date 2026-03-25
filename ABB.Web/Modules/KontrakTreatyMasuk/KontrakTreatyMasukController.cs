using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.KontrakTreatyMasuks.Commands;
using ABB.Application.KontrakTreatyMasuks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KontrakTreatyMasuk.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyMasuk
{
    public class KontrakTreatyMasukController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKontrakTreatyMasuks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetKontrakTreatyMasuksQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new KontrakTreatyMasukViewModel());
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_jns_sor, string kd_tty_msk)
        {
            var command = new GetKontrakTreatyMasukQuery()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_msk = kd_tty_msk
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            result.kd_grp_sb_bis = result.kd_grp_sb_bis?.Trim();
            result.kd_rk_sb_bis = result.kd_rk_sb_bis?.Trim();
            
            return View(Mapper.Map<KontrakTreatyMasukViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKontrakTreatyMasuk([FromBody] KontrakTreatyMasukViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKontrakTreatyMasukCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteKontrakTreatyMasuk(string kd_cb, string kd_jns_sor, string kd_tty_msk)
        {
            try
            {
                var command = new DeleteKontrakTreatyMasukCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_msk = kd_tty_msk
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

        public JsonResult GetTipeTreatyMasuk()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Sesi", Value = "S" },
                new DropdownOptionDto() { Text = "Retrosesi", Value = "R" }
            };

            return Json(dropdownOptionDtos);
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
    }
}