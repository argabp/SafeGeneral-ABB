using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.NotaFakultatifKeluars.Commands;
using ABB.Application.NotaFakultatifKeluars.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaFakultatifKeluar.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaFakultatifKeluar
{
    public class NotaFakultatifKeluarController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetNotaFakultatifKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetNotaFakultatifKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetDetailNotaFakultatifKeluars(GridRequest grid, string kd_cb,
            string jns_tr, string jns_nt_msk, string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, 
            string no_nt_kel)
        {
            var result = await Mediator.Send(new GetDetailNotaFakultatifKeluarsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel
            });
            
            return Json(result);
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaFakultatifKeluarQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            
            return View(Mapper.Map<NotaFakultatifKeluarViewModel>(result));
        }
        
        public async Task<IActionResult> View(string kd_cb, string jns_tr, string jns_nt_msk, 
            string kd_thn, string kd_bln, string no_nt_msk, string jns_nt_kel, string no_nt_kel)
        {
            var command = new GetNotaFakultatifKeluarQuery()
            {
                kd_cb = kd_cb,
                jns_tr = jns_tr,
                jns_nt_msk = jns_nt_msk,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                no_nt_msk = no_nt_msk,
                jns_nt_kel = jns_nt_kel,
                no_nt_kel = no_nt_kel
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            
            return View(Mapper.Map<NotaFakultatifKeluarViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNotaFakultatifKeluar([FromBody] NotaFakultatifKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaFakultatifKeluarCommand>(model);
                
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

            return PartialView("Edit" , model);
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
        
        public async Task<JsonResult> GetKodeRekanan(string kd_grp, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp,
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangPSTQuery());

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

        public JsonResult GetKodeTertuju()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" },
                new DropdownOptionDto() { Text = "Broker", Value = "2" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public JsonResult GetKodeEndorsement()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Insert", Value = "I" },
                new DropdownOptionDto() { Text = "Delete", Value = "D" }
            };

            return Json(result);
        }
    }
}