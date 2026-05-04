using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.NotaPremiXOLKeluars.Commands;
using ABB.Application.NotaPremiXOLKeluars.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaPremiXOLKeluar.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaPremiXOLKeluar
{
    public class NotaPremiXOLKeluarController : AuthorizedBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string jns_sb_nt, string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaPremiXOLKeluarQuery()
            {
                jns_sb_nt = jns_sb_nt,
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
            
            return PartialView(Mapper.Map<NotaPremiXOLKeluarViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> View(string jns_sb_nt, string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaPremiXOLKeluarQuery()
            {
                jns_sb_nt = jns_sb_nt,
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
            
            return PartialView(Mapper.Map<NotaPremiXOLKeluarViewModel>(data));
        }
        
        public async Task<ActionResult> GetNotaPremiXOLKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetNotaPremiXOLKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNotaPremiXOLKeluar([FromBody] NotaPremiXOLKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaPremiXOLKeluarCommand>(model);
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
        
        public JsonResult GetJenisNota()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Masuk", Value = "N" },
                new DropdownOptionDto() { Text = "Keluar", Value = "0" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeTertuju()
        {
            var result = await Mediator.Send(new GetKodeTertujuPSTQuery());

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananTertuju(string kd_cb, string kd_grp_rk)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp_rk,
                kd_cb = kd_cb
            });

            return Json(result);
        }

        public JsonResult GetSumberBisnis()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Broker", Value = "2" },
                new DropdownOptionDto() { Text = "P.As/Reas", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }
    }
}