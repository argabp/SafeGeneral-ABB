using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.EntriNotaKlaimTreaties.Commands;
using ABB.Application.EntriNotaKlaimTreaties.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriNotaKlaimTreaty.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EntriNotaKlaimTreaty
{
    public class EntriNotaKlaimTreatyController : AuthorizedBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaKlaimTreatyQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel,
            };
            
            var data = await Mediator.Send(command);
            
            return PartialView(Mapper.Map<EntriNotaKlaimTreatyViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaKlaimTreatyQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel,
            };
            
            var data = await Mediator.Send(command);
            
            return PartialView(Mapper.Map<EntriNotaKlaimTreatyViewModel>(data));
        }
        
        public async Task<ActionResult> GetEntriNotaKlaimTreaties(GridRequest grid)
        {
            var result = await Mediator.Send(new GetEntriNotaKlaimTreatiesQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveEntriNotaKlaimTreaty([FromBody] EntriNotaKlaimTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveEntriNotaKlaimTreatyCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        public async Task<JsonResult> GetCOB()
        {
            var mataUang = await Mediator.Send(new GetCobPSTQuery());
            
            return Json(mataUang);
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var mataUang = await Mediator.Send(new GetMataUangPSTQuery());
            
            return Json(mataUang);
        }
        
        public async Task<JsonResult> GetKodePersh()
        {
            var statusTipeDla = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };
            
            return Json(statusTipeDla);
        }
        
        public async Task<JsonResult> GetKodeRekananPersh(string kd_grp_pas, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp_pas,
                kd_cb = kd_cb
            });

            return Json(result);
        }
    }
}