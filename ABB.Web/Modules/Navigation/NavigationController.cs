using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Navigations.Commands;
using ABB.Application.Navigations.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Navigation.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Navigation
{
    public class NavigationController : AuthorizedBaseController
    {
        public NavigationController()
        {
        }

        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        public async Task<ActionResult> GetNavigationList([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetNavigationViewQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        public async Task<ActionResult> GetRouteDropdown()
        {
            var ds = await Mediator.Send(new GetRouteDropdownQuery());
            return Json(ds);
        }
        public async Task<ActionResult> GetSubNavigationDropdown(int Id)
        {
            var ds = await Mediator.Send(new GetSubNavigationDropdownQuery() { NavigationId = Id});
            return Json(ds);
        }
        public async Task<ActionResult> GetSubNavigationDropdownByNavId(int Id)
        {
            var ds = await Mediator.Send(new GetSubNavigationDropdownByNavIdQuery() { NavigationId = Id });
            return Json(ds);
        }
        public IActionResult Add()
        {
            AddNavigationModel model = new AddNavigationModel { IsActive = true };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddNavigationModel model)
        {
            try
            {
                var command = new AddNavigationWithSubnavigationCommand()
                {
                    Text = model.Text,
                    Icon = model.Icon,
                    IsActive = model.IsActive,
                    RouteId = model.RouteId ?? 0,
                    SubNavigationId = model.SubNavigations.Select(x => x.Id).Distinct().ToList()
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Menu Added." });
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
        
        public async Task<IActionResult> Edit(int Id)
        {
            var query = await Mediator.Send(new GetEditNavigationQuery() { Id = Id});
            var model = Mapper.Map<EditNavigationModel>(query);
            return PartialView(model);

        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditNavigationModel model)
        {
            try
            {
                var command = new EditNavigationWithSubnavigationCommand()
                {
                    NavigationId = model.NavigationId,
                    Text = model.Text,
                    Icon = model.Icon,
                    IsActive = model.IsActive,
                    RouteId = model.RouteId ?? 0,
                    SubNavigationId = model.SubNavigations.Select(x => x.Id).Distinct().ToList()
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Menu Updated." });
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
        public async Task<IActionResult> Delete(int NavigationId)
        {
            try
            {
                await Mediator.Send(new DeleteNavigationCommand() { NavigationId = NavigationId });
                return Json(new { Result = "OK", Message = "Menu Deleted." });
            }
            catch (ValidationException ex)
            {
                return Json(new { Result = "Error", Message = $"Fail deleting menu, error {ex.Message}" });
            }
        }

    }
}