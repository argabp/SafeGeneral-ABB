using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.KategoriJenisKendaraans.Queries;
using ABB.Application.PlatNomorKendaraans.Commands;
using ABB.Application.PlatNomorKendaraans.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PlatNomorKendaraan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PlatNomorKendaraan
{
    public class PlatNomorKendaraanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetPlatNomorKendaraans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetPlatNomorKendaraansQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var wilayah = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Wilayah I", Value = "1" },
                new DropdownOptionDto() { Text = "Wilayah II", Value = "2" },
                new DropdownOptionDto() { Text = "Wilayah III", Value = "3" }
            };

            foreach (var data in ds)
                data.nm_ref = wilayah.FirstOrDefault(w => w.Value == data.kd_ref1?.Trim())?.Text;
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] PlatNomorKendaraanViewModel model)
        {
            try
            {
                var command = Mapper.Map<SavePlatNomorKendaraanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Plat Nomor Kendaraan"});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_grp_rsk, string kd_rsk)
        {
            try
            {
                var command = new DeletePlatNomorKendaraanCommand()
                {
                    kd_grp_rsk = kd_grp_rsk, kd_rsk = kd_rsk,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Plat Nomor Kendaraan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetGrupResiko()
        {
            var result = await Mediator.Send(new GetGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetWilayah()
        {
            var wilayah = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Wilayah I", Value = "1" },
                new DropdownOptionDto() { Text = "Wilayah II", Value = "2" },
                new DropdownOptionDto() { Text = "Wilayah III", Value = "3" }
            };

            return Json(wilayah);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new PlatNomorKendaraanViewModel()
            {
                kd_grp_rsk = "106"
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_grp_rsk, string kd_rsk)
        {
            var platNomorKendaraan = await Mediator.Send(new GetPlatNomorKendaraanQuery()
            {
                kd_grp_rsk = kd_grp_rsk,
                kd_rsk = kd_rsk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            platNomorKendaraan.kd_grp_rsk = platNomorKendaraan.kd_grp_rsk.Trim();
            platNomorKendaraan.kd_ref1 = platNomorKendaraan.kd_ref1?.Trim() ?? string.Empty;

            return PartialView(Mapper.Map<PlatNomorKendaraanViewModel>(platNomorKendaraan));
        }
    }
}