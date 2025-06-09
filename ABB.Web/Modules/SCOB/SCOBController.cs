using System;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.SCOBs.Commands;
using ABB.Application.SCOBs.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.SCOB.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.SCOB
{
    public class SCOBController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<JsonResult> GetSCOBs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetSCOBsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Add()
        {
            var model = new SCOBViewModel();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] SCOBViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddSCOBCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                return PartialView(model);
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cob, string kd_scob)
        {
            var query = await Mediator.Send(new GetSCOBQuery()
            {
                kd_cob = kd_cob, kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var model = Mapper.Map<SCOBViewModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] SCOBViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditSCOBCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string kd_cob, string kd_scob)
        {
            try
            {
                await Mediator.Send(new DeleteSCOBCommand()
                {
                    kd_cob = kd_cob, kd_scob = kd_scob,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });

                return Json(new { Result = true, Message = Constant.DataDisimpan });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<ActionResult> GetCOBs()
        {
            var ds = await Mediator.Send(new GetCOBsQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(ds);
        }
    }
}