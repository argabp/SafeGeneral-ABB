using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Okupasis.Commands;
using ABB.Application.Okupasis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Okupasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Okupasi
{
    public class OkupasiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetOkupasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetOkupasiQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailOkupasis([DataSourceRequest] DataSourceRequest request, string kd_okup)
        {
            if (string.IsNullOrWhiteSpace(kd_okup))
                return Ok();

            var ds = await Mediator.Send(new GetDetailOkupasiQuery()
            {
                kd_okup = kd_okup,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
            {
                data.Id = data.kd_okup.Trim() + data.kd_kls_konstr.Trim();
                
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
        public async Task<IActionResult> AddOkupasi([FromBody] SaveOkupasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddOkupasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Okupasi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditOkupasi([FromBody] SaveOkupasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditOkupasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Okupasi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteOkupasi([FromBody] DeleteOkupasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteOkupasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Okupasi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailOkupasi([FromBody] DetailOkupasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailOkupasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Detail Okupasi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailOkupasi([FromBody] DeleteDetailOkupasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailOkupasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Okupasi"});
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
        public IActionResult AddDetailOkupasiView(string kd_okup)
        {
            return PartialView(new DetailOkupasiViewModel() { kd_okup = kd_okup});
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailOkupasiView(string kd_okup, string kd_kls_konstr)
        {
            var detailLokasiResiko = await Mediator.Send(new GetSingleDetailOkupasiQuery()
            {
                kd_kls_konstr = kd_kls_konstr,
                kd_okup = kd_okup,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<DetailOkupasiViewModel>(detailLokasiResiko));
        }
    }
}