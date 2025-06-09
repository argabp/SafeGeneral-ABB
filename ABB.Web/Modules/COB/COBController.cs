using System;
using System.Threading.Tasks;
using ABB.Application.COBs.Commands;
using ABB.Application.COBs.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.COB.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.COB
{
    public class COBController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<JsonResult> GetCOBs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetCOBsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Add()
        {
            var model = new COBViewModel();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] COBViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddCOBCommand>(model);
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

        public async Task<IActionResult> Edit(string kd_cob)
        {
            var query = await Mediator.Send(new GetCOBQuery()
            {
                kd_cob = kd_cob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var model = Mapper.Map<COBViewModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] COBViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditCOBCommand>(model);
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
        public async Task<IActionResult> Delete(string kd_cob)
        {
            try
            {
                await Mediator.Send(new DeleteCOBCommand()
                {
                    kd_cob = kd_cob,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });

                return Json(new { Result = true, Message = Constant.DataDisimpan });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}