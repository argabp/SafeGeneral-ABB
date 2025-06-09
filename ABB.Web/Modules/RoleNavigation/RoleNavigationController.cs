using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.RoleNavigations.Commands;
using ABB.Application.RoleNavigations.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RoleNavigation.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RoleNavigation
{
    public class RoleNavigationController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<ActionResult> GetRoleNavigations([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRoleNavigationsQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<JsonResult> GetNavigations()
        {
            var result = await Mediator.Send(new GetNavigationsQuery());
            return Json(result);
        }

        public IActionResult Add()
        {
            var model = new AddRoleNavigationModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddRoleNavigationModel model)
        {
            try
            {
                var command = new AddRoleNavigationCommand();
                command.RoleId = model.RoleId;
                foreach (var item in model.Navigations)
                {
                    if(item.Navigation != null)
                    {
                        command.Navigations.Add(item.Navigation.NavigationId);

                        if (item.SubNavigation != null)
                            foreach (var subItem in item.SubNavigation)
                                command.Navigations.Add(subItem.NavigationId);
                    }
                }

                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
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

        public async Task<IActionResult> Edit(string id = null)
        {
            var model = new EditRoleNavigationModel() { RoleId = id };
            var roleNavigations = await Mediator.Send(new GetNavigationsDatasourceQuery() { RoleId = id });

            foreach (var roleNavigation in roleNavigations)
            {
                if (roleNavigation.ParentId == 0)
                {
                    model.Navigations.Add(new NavigationItem()
                    {
                        RoleId = roleNavigation.RoleId,
                        Navigation = new NavigationModel()
                        {
                            NavigationId = roleNavigation.NavigationId,
                            Text = roleNavigation.Text
                        }
                    });
                }
            }

            foreach (var roleNavigation in roleNavigations)
                if (roleNavigation.ParentId != 0 && model.Navigations.Any(w => w.Navigation.NavigationId == roleNavigation.ParentId))
                    model.Navigations.FirstOrDefault(w => w.Navigation.NavigationId == roleNavigation.ParentId).SubNavigation.Add(new NavigationModel()
                    {
                        NavigationId = roleNavigation.NavigationId,
                        Text = roleNavigation.Text
                    });

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditRoleNavigationModel model)
        {
            try
            {
                var command = new EditRoleNavigationCommand();
                command.RoleId = model.RoleId;
                foreach (var item in model.Navigations)
                {
                    command.Navigations.Add(item.Navigation.NavigationId);

                    if (item.SubNavigation != null)
                        foreach (var subItem in item.SubNavigation)
                            command.Navigations.Add(subItem.NavigationId);
                }

                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
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

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteRoleNavigationCommand() { Id = id });
            return Json(new { Result = "OK", Message = Constant.DataDisimpan });
        }

        public async Task<JsonResult> GetRoles()
        {
            var result = await Mediator.Send(new GetRolesQuery());
            return Json(result);
        }

        public async Task<ActionResult> GetSubNavigations(int id = 0)
        {
            var ds = await Mediator.Send(new GetSubNavigationQuery() { ParentId = id });
            return Json(ds);
        }
    }
}