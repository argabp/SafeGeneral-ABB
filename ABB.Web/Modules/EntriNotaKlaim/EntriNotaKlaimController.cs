using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.EntriNotaKlaims.Commands;
using ABB.Application.EntriNotaKlaims.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriNotaKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EntriNotaKlaim
{
    public class EntriNotaKlaimController : AuthorizedBaseController
    {
        private static List<RekananDto> _rekanans;
        private static List<DropdownOptionDto> _mataUang;
        private static List<DropdownOptionDto> _tipeMutasi;
        private static List<DropdownOptionDto> _statusTipeDla;
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            

            _rekanans = await Mediator.Send(new GetRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            
            _mataUang = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            _tipeMutasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PLA", Value = "P" },
                new DropdownOptionDto() { Text = "DLA", Value = "D" },
                new DropdownOptionDto() { Text = "Beban", Value = "B" },
                new DropdownOptionDto() { Text = "Recovery", Value = "R" }
            };
            
            _statusTipeDla = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tertanggung", Value = "T" },
                new DropdownOptionDto() { Text = "Koasuransi", Value = "K" },
                new DropdownOptionDto() { Text = "Reasuransi", Value = "R" }
            };
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaKlaimQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
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
            
            return PartialView(Mapper.Map<NotaKlaimViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaKlaimQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
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
            
            return PartialView(Mapper.Map<NotaKlaimViewModel>(data));
        }
        
        public async Task<ActionResult> GetEntriNotaKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetEntriNotaKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                counter++;
                data.nomor_register = data.kd_cb.Trim() + "." + data.kd_cob.Trim() +
                                      data.kd_scob.Trim() + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                data.nm_tipe_mts = _tipeMutasi.FirstOrDefault(w => w.Value.Trim() == data.tipe_mts.Trim())?.Text ?? string.Empty;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveEntriNotaKlaim([FromBody] NotaKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveEntriNotaKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public JsonResult GetMataUang()
        {
            return Json(_mataUang);
        }
        
        public JsonResult GetTipeMutasi()
        {
            return Json(_tipeMutasi);
        }
        
        public JsonResult GetStatusTipeDLA()
        {
            return Json(_statusTipeDla);
        }
        
        public async Task<JsonResult> GetKodeTertuju()
        {
            var result = await Mediator.Send(new GetKodeTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetKodeRekananTertuju(string kd_grp_ttj)
        {
            return Json(_rekanans.Where(w => w.kd_grp_rk == kd_grp_ttj)
                .Select(rekanan => new DropdownOptionDto() { Text = rekanan.nm_rk, Value = rekanan.kd_rk })
                .ToList());
        }

        public async Task<JsonResult> GenerateEntriNotaKlaimData(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var result = await Mediator.Send(new GenerateEntriNotaKlaimDataQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk
            });

            return Json(result);
        }
    }
}