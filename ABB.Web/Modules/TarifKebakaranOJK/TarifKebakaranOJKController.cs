using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Okupasis.Commands;
using ABB.Application.Okupasis.Queries;
using ABB.Application.TarifKebakaranOJKs.Commands;
using ABB.Application.TarifKebakaranOJKs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Okupasi.Models;
using ABB.Web.Modules.TarifKebakaranOJK.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.TarifKebakaranOJK
{
    public class TarifKebakaranOJKController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetTarifKebakaranOJKs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetOkupasiQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailTarifKebakaranOJKs([DataSourceRequest] DataSourceRequest request, string kd_okup)
        {
            if (string.IsNullOrWhiteSpace(kd_okup))
                return Ok();

            var ds = await Mediator.Send(new GetDetailTarifKebakaranOJKsQuery()
            {
                kd_okup = kd_okup,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
            {
                switch (data.kd_kls_konstr)
                {
                    case "1":
                        data.text_kls_konstr = "KELAS I";
                        break;
                    case "2":
                        data.text_kls_konstr = "KELAS II";
                        break;
                    case "3":
                        data.text_kls_konstr = "KELAS III";
                        break;
                }

                data.text_stn_rate_premi = data.stn_rate_prm == 1 ? "%" : "%o";
                    
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailTarifKebakaranOJK([FromBody] DetailTarifKebakaranOJKViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTarifKebakaranOJKCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailTarifKebakaranOJK([FromBody] DeleteDetailTarifKebakaranOJKViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailTarifKebakaranOJKCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetKodeKelasKontruksi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "KELAS I", Value = "1" },
                new DropdownOptionDto() { Text = "KELAS II", Value = "2" },
                new DropdownOptionDto() { Text = "KELAS III", Value = "3" }
            };

            return Json(result);
        }
        
        public JsonResult GetSatuanRatePremi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "%", Value = "1" },
                new DropdownOptionDto() { Text = "%o", Value = "10" }
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult AddDetailTarifKebakaranOJKView(string kd_okup)
        {
            return PartialView(new DetailTarifKebakaranOJKViewModel() { kd_okup = kd_okup});
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailTarifKebakaranOJKView(string kd_okup, string kd_kls_konstr)
        {
            var detailLokasiResiko = await Mediator.Send(new GetDetailTarifKebakaranOJKQuery()
            {
                kd_kls_konstr = kd_kls_konstr,
                kd_okup = kd_okup,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<DetailTarifKebakaranOJKViewModel>(detailLokasiResiko));
        }
    }
}