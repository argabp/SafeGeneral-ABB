using System.Collections.Generic;
using ABB.Web.Modules.RoleRoute.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RoleRoute.Components.ControllerActionList
{
    public class ControllerActionListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<RouteModel> model)
        {
            return View("_ControllerActionList", model);
        }
    }
}