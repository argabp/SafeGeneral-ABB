    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ABB.Application.Common;
    using ABB.Application.Common.Dtos;
    using ABB.Application.Zonas.Commands;
    using ABB.Application.Zonas.Queries;
    using ABB.Web.Modules.Base;
    using ABB.Web.Modules.Zona.Models;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;

    namespace ABB.Web.Modules.Zona
{
    public class ZonaController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetZonas([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetZonaQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailZonas([DataSourceRequest] DataSourceRequest request, string kd_zona)
        {
            if (string.IsNullOrWhiteSpace(kd_zona))
                return Ok();

            var ds = await Mediator.Send(new GetDetailZonaQuery()
            {
                kd_zona = kd_zona,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
            {
                data.text_stn_rate_premi = data.stn_rate_prm == 1 ? "%" : "%o";

                data.nm_okup = data.kd_okup.Trim() == "2976" ? "Dwelling House" : "Non Dwelling House";

            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddZona([FromBody] SaveZonaViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddZonaCommand>(model);
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
        public async Task<IActionResult> EditZona([FromBody] SaveZonaViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditZonaCommand>(model);
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
        public async Task<IActionResult> DeleteZona([FromBody] DeleteZonaViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteZonaCommand>(model);
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
        public async Task<IActionResult> SaveDetailZona([FromBody] DetailZonaViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailZonaCommand>(model);
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
        public async Task<IActionResult> DeleteDetailZona([FromBody] DeleteDetailZonaViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailZonaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetKelasKonstruksi()
        {
            var result = new List<DropdownOptionDto>();
            
            var kelasKonstruksi = await Mediator.Send(new GetKelasKonstruksiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in kelasKonstruksi)
                result.Add(new DropdownOptionDto()
                {
                    Text = data.nm_kls_konstr,
                    Value = data.kd_kls_konstr
                });

            return Json(result);
        }
        
        public JsonResult GetOkupasi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Non Dwelling House", Value = "0" },
                new DropdownOptionDto() { Text = "Dwelling House", Value = "2976" }
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
        public IActionResult AddDetailZonaView(string kd_zona)
        {
            return PartialView(new DetailZonaViewModel() { kd_zona = kd_zona});
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailZonaView(string kd_zona, string kd_kls_konstr)
        {
            var detailLokasiResiko = await Mediator.Send(new GetSingleDetailZonaQuery()
            {
                kd_kls_konstr = kd_kls_konstr,
                kd_zona = kd_zona,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<DetailZonaViewModel>(detailLokasiResiko));
        }
    }
}