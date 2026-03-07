using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.NotaKlaimReasuransis.Commands;
using ABB.Application.NotaKlaimReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaKlaimReasuransi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaKlaimReasuransi
{
    public class NotaKlaimReasuransiController : AuthorizedBaseController
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
            var command = new GetNotaKlaimReasuransiQuery()
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
            
            return PartialView(Mapper.Map<NotaKlaimReasuransiViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaKlaimReasuransiQuery()
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
            
            return PartialView(Mapper.Map<NotaKlaimReasuransiViewModel>(data));
        }
        
        public async Task<ActionResult> GetNotaKlaimReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetNotaKlaimReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNotaKlaimReasuransi([FromBody] NotaKlaimReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaKlaimReasuransiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var mataUang = await Mediator.Send(new GetMataUangPSTQuery());
            
            return Json(mataUang);
        }
        
        public JsonResult GetTipeMutasi()
        {
            var tipeMutasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PLA", Value = "P" },
                new DropdownOptionDto() { Text = "DLA", Value = "D" },
                new DropdownOptionDto() { Text = "Beban", Value = "B" },
                new DropdownOptionDto() { Text = "Recovery", Value = "R" }
            };
            
            return Json(tipeMutasi);
        }
        
        public JsonResult GetStatusTipeDLA()
        {
            var statusTipeDla = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tertanggung", Value = "T" },
                new DropdownOptionDto() { Text = "Koasuransi", Value = "K" },
                new DropdownOptionDto() { Text = "Reasuransi", Value = "R" }
            };
            
            return Json(statusTipeDla);
        }
        
        public JsonResult GetKodeSor()
        {
            var kodeSor = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Own Retention", Value = "0" },
                new DropdownOptionDto() { Text = "Treaty", Value = "1" },
                new DropdownOptionDto() { Text = "Fakultatif", Value = "2" },
                new DropdownOptionDto() { Text = "Excess of Loss", Value = "N" },
                new DropdownOptionDto() { Text = "P.As/Reas", Value = "5" },
                new DropdownOptionDto() { Text = "Treaty", Value = "T" }
            };
            
            return Json(kodeSor);
        }
        
        public async Task<JsonResult> GetKodeRekananSor(string kd_grp_sor)
        {
            var result = await Mediator.Send(new GetRekananSorPSTQuery()
            {
                jns_lookup = kd_grp_sor
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeJenisSor()
        {
            var result = await Mediator.Send(new GetJenisSorPSTQuery());

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeTertuju()
        {
            var result = await Mediator.Send(new GetKodeTertujuPSTQuery());

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananTertuju(string kd_grp_ttj, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp_ttj,
                kd_cb = kd_cb
            });

            return Json(result);
        }

        public async Task<JsonResult> GenerateNotaKlaimReasuransiData(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var result = await Mediator.Send(new GenerateNotaKlaimReasuransiDataQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk
            });

            return Json(result);
        }
    }
}