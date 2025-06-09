using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.RoleRoutes.Commands;
using ABB.Application.RoleRoutes.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RoleRoute.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RoleRoute
{
    public class RoleRouteController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<IActionResult> GetRoleRoutes([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRoleRoutesQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        public async Task<IActionResult> GetRoutes(string roleId, string searchKeyword)
        {
            var routes = await Mediator.Send(new GetRoleRouteQuery() { RoleId = roleId, SearchKeyword = searchKeyword });
            var model = new List<RouteModel>();
            Mapper.Map(routes, model);
            return ViewComponent("ControllerActionList", new { model = model });
        }
        public async Task<IActionResult> Edit(string id, string name)
        {
            var routes = await Mediator.Send(new GetRoleRouteQuery() { RoleId = id });

            var model = new EditRoleRouteModel()
            {
                RoleId = id,
                RoleName = name,
                Routes = new List<RouteModel>()
            };
            Mapper.Map(routes, model.Routes);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditRoleRouteModel model)
        {
            try
            {
                var command = new EditRoleRouteCommand();
                command.RoleId = model.RoleId;
                foreach (var item in model.Routes.Where(w => w.Active))
                {
                    command.Routes.Add(item.RouteId);
                }

                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }
    }
}