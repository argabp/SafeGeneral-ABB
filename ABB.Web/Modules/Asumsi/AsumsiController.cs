using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Asumsis.Commands;
using ABB.Application.Asumsis.Queries;
using ABB.Web.Modules.Asumsi.Models;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Asumsi
{
    public class AsumsiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetAsumsi([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAsumsiQuery() 
            { 
                SearchKeyword = searchkeyword
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetAsumsiPeriode([DataSourceRequest] DataSourceRequest request, string kodeAsumsi)
        {
            if (string.IsNullOrWhiteSpace(kodeAsumsi))
                return Ok();
            
            var ds = await Mediator.Send(new GetAsumsiPeriodeQuery()
            {
                KodeAsumsi = kodeAsumsi
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetAsumsiDetail([DataSourceRequest] DataSourceRequest request, string kodeAsumsi, string kodeProduk, DateTime periodeProses)
        {
            if (string.IsNullOrWhiteSpace(kodeAsumsi))
                return Ok();
            
            var ds = await Mediator.Send(new GetAsumsiDetailQuery() 
            { 
                KodeAsumsi = kodeAsumsi,
                KodeProduk = kodeProduk,
                PeriodeProses = periodeProses
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAsumsi([FromBody] AsumsiViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddAsumsiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Asumsi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditAsumsi([FromBody] AsumsiViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditAsumsiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Asumsi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAsumsi([FromBody] AsumsiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteAsumsiCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Asumsi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAsumsiPeriode([FromBody] AsumsiPeriodeViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddAsumsiPeriodeCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Asumsi Periode"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAsumsiPeriode([FromBody] AsumsiPeriodeViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteAsumsiPeriodeCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Asumsi Periode"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAsumsiDetail([FromBody] AsumsiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddAsumsiDetailCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Asumsi Detail"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditAsumsiDetail([FromBody] AsumsiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditAsumsiDetailCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Asumsi Detail"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAsumsiDetail([FromBody] AsumsiDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteAsumsiDetailCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Asumsi Detail"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}