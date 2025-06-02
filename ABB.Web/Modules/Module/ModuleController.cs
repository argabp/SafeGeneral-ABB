using System;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Modules.Commends;
using ABB.Application.Modules.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Module.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Module
{
    public class ModuleController : AuthorizedBaseController
    {
        public ModuleController()
        {
            
        }
        
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<JsonResult> GetModules([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetModulesQuery() { SearchKeyword = searchkeyword });
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Add()
        {
            var model = new ModuleViewModel();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody] ModuleViewModel model)
        {
            try
            {
                await Mediator.Send(Mapper.Map<AddModuleCommand>(model));
                return Json(new { Result = "OK", Message = "Successfully Add Module" });
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

        public async Task<IActionResult> Edit(int id)
        {
            var query = await Mediator.Send(new GetModuleQuery() { ModuleId = id });
            var model = Mapper.Map<ModuleViewModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] ModuleViewModel model)
        {
            try
            {
                await Mediator.Send(Mapper.Map<EditModuleCommand>(model));
                return Json(new { Result = "OK", Message = "Successfully Edit Module" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteModuleCommand() { Id = id });

                return Json(new { Result = true, Message = "Successfully Delete Module" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}