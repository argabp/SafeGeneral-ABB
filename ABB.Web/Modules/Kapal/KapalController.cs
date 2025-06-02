using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Kapals.Commands;
using ABB.Application.Kapals.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Kapal.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Kapal
{
    public class KapalController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKapals([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKapalsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            var klasifikasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "BKI", Value = "B" },
                new DropdownOptionDto() { Text = "Not Classified", Value = "N" },
                new DropdownOptionDto() { Text = "International", Value = "I" }
            };

            foreach (var data in ds)
                data.nm_klasifikasi = klasifikasi.FirstOrDefault(w => w.Value == data.st_class)?.Text;
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] KapalViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKapalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Kapal"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteKapal(string kd_kapal)
        {
            try
            {
                var command = new DeleteKapalCommand()
                {
                    kd_kapal = kd_kapal,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Kapal"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetKlasifikasi()
        {
            var klasifikasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "BKI", Value = "B" },
                new DropdownOptionDto() { Text = "Not Classified", Value = "N" },
                new DropdownOptionDto() { Text = "International", Value = "I" }
            };

            return Json(klasifikasi);
        }

        public JsonResult GetJenisKapal()
        {
            var klasifikasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tanker", Value = "Tanker" },
                new DropdownOptionDto() { Text = "Bulk", Value = "Bulk" },
                new DropdownOptionDto() { Text = "General Cargo", Value = "General Cargo" },
                new DropdownOptionDto() { Text = "LCT", Value = "LCT" },
                new DropdownOptionDto() { Text = "Barge", Value = "Barge" },
                new DropdownOptionDto() { Text = "Tug", Value = "Tug" },
                new DropdownOptionDto() { Text = "Wooden (Kapal Layar Motor)", Value = "Wooden (Kapal Layar Motor)" },
                new DropdownOptionDto() { Text = "Wooden (Kapal Kayu Motor)", Value = "Wooden (Kapal Kayu Motor)" },
                new DropdownOptionDto() { Text = "Wooden (Perahu Layar Motor", Value = "Wooden (Perahu Layar Motor)" },
                new DropdownOptionDto() { Text = "Container Ship", Value = "Container Ship" },
                new DropdownOptionDto() { Text = "Passenger ferry (roro)", Value = "Passenger ferry (roro)" },
                new DropdownOptionDto() { Text = "TONGKANG", Value = "TONGKANG" },
                new DropdownOptionDto() { Text = "Speed Boat", Value = "Speed Boat" },
                new DropdownOptionDto() { Text = "Crew Boat", Value = "Crew Boat" }
            };

            return Json(klasifikasi);
        }

        public async Task<JsonResult> GetNegara()
        {
            var result = await Mediator.Send(new GetNegaraQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new KapalViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_kapal)
        {
            var kapal = await Mediator.Send(new GetKapalQuery()
            {
                kd_kapal = kd_kapal,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<KapalViewModel>(kapal));
        }
    }
}