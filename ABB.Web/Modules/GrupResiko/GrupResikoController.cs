using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.GrupResikos.Commands;
using ABB.Application.GrupResikos.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.GrupResiko.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.GrupResiko
{
    public class GrupResikoController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetGrupResikos([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetGrupResikoQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailGrupResikos([DataSourceRequest] DataSourceRequest request, string kd_grp_rsk)
        {
            if (string.IsNullOrWhiteSpace(kd_grp_rsk))
                return Ok();
            
            var ds = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                kd_grp_rsk = kd_grp_rsk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddGrupResiko([FromBody] SaveGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddGrupResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditGrupResiko([FromBody] SaveGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditGrupResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteGrupResiko([FromBody] DeleteGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteGrupResikoCommand>(model);
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
        public async Task<IActionResult> AddDetailGrupResiko([FromBody] SaveDetailGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddDetailGrupResikoCommand>(model);
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
        public async Task<IActionResult> EditDetailGrupResiko([FromBody] SaveDetailGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditDetailGrupResikoCommand>(model);
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
        public async Task<IActionResult> DeleteDetailGrupResiko([FromBody] DeleteDetailGrupResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailGrupResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}