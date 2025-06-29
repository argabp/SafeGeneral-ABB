using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.PeriodeProses.Commands;
using ABB.Application.PeriodeProses.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PeriodeProses.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PeriodeProses
{
    public class PeriodeProsesController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<ActionResult> GetPeriodeProses([DataSourceRequest] DataSourceRequest request)
        {
            var ds = await Mediator.Send(new GetPeriodeProsesQuery());
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPeriodeProses([FromBody] PeriodeProsesViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddPeriodeProsesCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Periode Proses"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditPeriodeProses([FromBody] PeriodeProsesViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditPeriodeProsesCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Periode Proses"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeletePeriodeProses([FromBody] PeriodeProsesViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeletePeriodeProsesCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Periode Proses"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}