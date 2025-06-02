using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.ModuleNavigations.Commands;
using ABB.Application.ModuleNavigations.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ModuleNavigation.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ModuleNavigation
{
    public class ModuleNavigationController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetModuleNavigations([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetModuleNavigationsQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> GetModules()
        {
            var ds = await Mediator.Send(new GetModulesQuery());
            return Json(ds);
        }
        
        public async Task<ActionResult> GetNavigationDropdown()
        {
            var ds = await Mediator.Send(new GetNavigationsQuery());
            return Json(ds);
        }
        
        public async Task<ActionResult> GetNavigationDropdownByModuleId(int moduleId)
        {
            var ds = await Mediator.Send(new GetNavigationByModuleQuery() { ModuleId = moduleId });
            return Json(ds);
        }
        
        public IActionResult Add()
        {
            var model = new ModuleNavigationViewModel();
            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ModuleNavigationViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveModuleNavigationCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Module Menu Saved." });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<IActionResult> Edit(int moduleId)
        {
            var query = await Mediator.Send(new GetModuleNavigationQuery() { ModuleId = moduleId});
            var model = Mapper.Map<ModuleNavigationViewModel>(query);
            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int moduleId)
        {
            try
            {
                await Mediator.Send(new DeleteModuleNavigationCommand() { ModuleId = moduleId });
                return Json(new { Result = "OK", Message = "Menu Deleted." });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "Error", Message = $"Fail deleting role module, error {ex.Message}" });
            }
        }
    }
}