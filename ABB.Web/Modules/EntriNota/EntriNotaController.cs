using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.EntriNotas.Commands;
using ABB.Application.EntriNotas.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.EntriNota.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.EntriNota
{
    public class EntriNotaController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        public ActionResult NotaCancel()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaQuery()
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
            
            return PartialView(Mapper.Map<NotaViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaQuery()
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
            
            return PartialView(Mapper.Map<NotaViewModel>(data));
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetEntriNotaQuery()
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
            
            return PartialView(Mapper.Map<NotaViewModel>(data));
        }
        
        public async Task<ActionResult> GetEntriNotas([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetEntriNotasQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var statusPolis = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Leader (Sebagai Leader Koasuransi)", Value = "L" },
                new DropdownOptionDto() { Text = "Member (Sebagai Member Koasuransi)", Value = "M" },
                new DropdownOptionDto() { Text = "Transaksi Direct", Value = "O" },
                new DropdownOptionDto() { Text = "Inward Fakultatif", Value = "C" }
            };

            foreach (var data in ds)
            {
                data.st_pas = statusPolis.FirstOrDefault(w => w.Value == data.st_pas)?.Text ?? string.Empty;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> GetEntriNotaCancels([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string kd_cob, string kd_scob, string kd_thn, string no_pol, Int16 no_updt)
        {
            var ds = await Mediator.Send(new GetEntriNotaCancelsQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var statusPolis = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Leader (Sebagai Leader Koasuransi)", Value = "L" },
                new DropdownOptionDto() { Text = "Member (Sebagai Member Koasuransi)", Value = "M" },
                new DropdownOptionDto() { Text = "Transaksi Direct", Value = "O" },
                new DropdownOptionDto() { Text = "Inward Fakultatif", Value = "C" }
            };

            foreach (var data in ds)
            {
                data.st_pas = statusPolis.FirstOrDefault(w => w.Value == data.st_pas)?.Text ?? string.Empty;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailNotas([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string jns_tr, string jns_nt_msk, string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, 
            string no_nt_kel)
        {
            if (string.IsNullOrWhiteSpace(kd_cb) || string.IsNullOrWhiteSpace(jns_tr) || string.IsNullOrWhiteSpace(jns_nt_msk) ||
                string.IsNullOrWhiteSpace(kd_thn) || string.IsNullOrWhiteSpace(kd_bln) || string.IsNullOrWhiteSpace(no_nt_msk) ||
                string.IsNullOrWhiteSpace(jns_nt_kel) || string.IsNullOrWhiteSpace(no_nt_kel))
                return Ok();
            
            var ds = await Mediator.Send(new GetDetailNotasQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntriNota([FromBody] NotaDto model)
        {
            try
            {
                var command = Mapper.Map<SaveEntriNotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntriNotaCancel([FromBody] NotaDto model)
        {
            try
            {
                var command = Mapper.Map<SaveEntriNotaCancelCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeTertuju()
        {
            var result = await Mediator.Send(new GetKodeTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananTertuju(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_pol, Int16 no_updt, string kd_grp_ttj)
        {
            var result = await Mediator.Send(new GetRekananTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_grp_ttj = kd_grp_ttj,
            });

            return Json(result);
        }
        
        public JsonResult GetCaraBayar()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Single Premium", Value = "1" },
                new DropdownOptionDto() { Text = "Bulanan", Value = "2" },
                new DropdownOptionDto() { Text = "Triwulanan", Value = "3" },
                new DropdownOptionDto() { Text = "Semesteran", Value = "4" },
                new DropdownOptionDto() { Text = "Tahunan", Value = "5" },
                new DropdownOptionDto() { Text = "Lainnya", Value = "6" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GenerateEntriNotaData(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var result = await Mediator.Send(new GenerateEntriNotaDataQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GenerateNilaiAng(decimal pst_ang, decimal nilai_nt, decimal nilai_ppn, decimal nilai_pph)
        {
            var result = await Mediator.Send(new GenerateNilaiAngQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                pst_ang = pst_ang,
                nilai_nt = nilai_nt,
                nilai_ppn = nilai_ppn,
                nilai_pph = nilai_pph
            });

            return Json(result);
        }

        public async Task<JsonResult> ValidateSaveDetailNota(string no_pol, decimal nilai_nt, decimal nilai_ang)
        {
            var result = await Mediator.Send(new ValidateSaveDetailNotaCommand()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                no_pol = no_pol,
                nilai_nt = nilai_nt,
                nilai_ang = nilai_ang
            });

            return Json(result);
        }
    }
}