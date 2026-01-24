using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.LKSs.Commands;
using ABB.Application.LKSs.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LKS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LKS
{
    public class LKSController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetLKSs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLKSsQuery()
            {
                SearchKeyword = searchkeyword, 
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                data.nomor_register = "K." + data.kd_cb.Trim() + "." + data.kd_scob.Trim() 
                                      + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                counter++;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_kl, Int16 no_mts, string st_tipe_pla, Int16 no_pla)
        {
            var command = new GetLKSQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                st_tipe_pla = st_tipe_pla,
                no_pla = no_pla
            };
            
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            
            return View(Mapper.Map<LKSViewModel>(result));
        }
        
        public async Task<IActionResult> View(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_kl, Int16 no_mts, string st_tipe_pla, Int16 no_pla)
        {
            var command = new GetLKSQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                st_tipe_pla = st_tipe_pla,
                no_pla = no_pla
            };
            
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();

            return View(Mapper.Map<LKSViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveLKS([FromBody] LKSViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveLKSCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
             
            return Json(cobs);
        }

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }
        
        public JsonResult GetStatusTipePLA()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tertanggung", Value = "T" },
                new DropdownOptionDto() { Text = "Koasuransi", Value = "K" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodePas()
        {
            var result = await Mediator.Send(new GetKodeTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananPas(string kd_grp_pas)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rk = kd_grp_pas
            });

            return Json(result);
        }
    }
}