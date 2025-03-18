using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.JenisSors.Commands;
using ABB.Application.JenisSors.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.JenisSor.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.JenisSor
{
    public class JenisSorController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetJenisSors([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetJenisSorsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            var groups = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Own Retention", Value = "0" },
                new DropdownOptionDto() { Text = "Treaty", Value = "1" },
                new DropdownOptionDto() { Text = "Fakultatif", Value = "2" }
            };

            foreach (var data in ds)
                data.nm_grp_jns_sor = groups.FirstOrDefault(w => w.Value == data.grp_jns_sor)?.Text;
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveJenisSor([FromBody] JenisSorViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveJenisSorCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Jenis Sor"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteJenisSor(string kd_jns_sor)
        {
            try
            {
                var command = new DeleteJenisSorCommand()
                {
                    kd_jns_sor = kd_jns_sor,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Jenis Sor"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetGroupJenisSor()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Own Retention", Value = "0" },
                new DropdownOptionDto() { Text = "Treaty", Value = "1" },
                new DropdownOptionDto() { Text = "Fakultatif", Value = "2" }
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult AddJenisSorView()
        {
            return PartialView(new JenisSorViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditJenisSorView(string kd_jns_sor)
        {
            var jenisSor = await Mediator.Send(new GetJenisSorQuery()
            {
                kd_jns_sor = kd_jns_sor,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<JenisSorViewModel>(jenisSor));
        }
    }
}