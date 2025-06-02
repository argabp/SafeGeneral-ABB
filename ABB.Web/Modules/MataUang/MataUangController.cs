using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.MataUangs.Commands;
using ABB.Application.MataUangs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.MataUang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.MataUang
{
    public class MataUangController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetMataUangs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetMataUangQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailMataUangs([DataSourceRequest] DataSourceRequest request, string kd_mtu)
        {
            if (string.IsNullOrWhiteSpace(kd_mtu))
                return Ok();
            
            var ds = await Mediator.Send(new GetDetailMataUangQuery()
            {
                kd_mtu = kd_mtu,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddMataUang([FromBody] SaveMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddMataUangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Mata Uang"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditMataUang([FromBody] SaveMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditMataUangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Mata Uang"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMataUang([FromBody] DeleteMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteMataUangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Mata Uang"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddDetailMataUang([FromBody] SaveDetailMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddDetailMataUangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Detail Mata Uang"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetailMataUang([FromBody] SaveDetailMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditDetailMataUangCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Detail Mata Uang"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailMataUang([FromBody] DeleteDetailMataUangViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailMataUangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Mata Uang"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}