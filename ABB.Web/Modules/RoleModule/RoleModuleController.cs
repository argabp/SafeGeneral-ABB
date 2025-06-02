using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.RoleModules.Commands;
using ABB.Application.RoleModules.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RoleModule.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RoleModule
{
    public class RoleModuleController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetRoleModules([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRoleModulesQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<ActionResult> GetRoles()
        {
            var ds = await Mediator.Send(new GetRolesQuery());
            return Json(ds);
        }
        
        public async Task<ActionResult> GetModuleDropdown()
        {
            var ds = await Mediator.Send(new GetModulesQuery());
            return Json(ds);
        }
        
        public async Task<ActionResult> GetModuleDropdownByRoleId(string roleId)
        {
            var ds = await Mediator.Send(new GetModulesByRoleQuery() { RoleId = roleId });
            return Json(ds);
        }
        
        public IActionResult Add()
        {
            var model = new RoleModuleViewModel();
            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RoleModuleViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveRoleModuleCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Role Module Saved." });
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

        public async Task<IActionResult> Edit(string roleId)
        {
            var query = await Mediator.Send(new GetRoleModuleQuery() { RoleId = roleId});
            var model = Mapper.Map<RoleModuleViewModel>(query);
            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            try
            {
                await Mediator.Send(new DeleteRoleModuleCommand() { RoleId = roleId });
                return Json(new { Result = "OK", Message = "Menu Deleted." });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "Error", Message = $"Fail deleting role module, error {ex.Message}" });
            }
        }
    }
}