using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Routes.Commands;
using ABB.Application.Routes.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Route
{
    public class RouteController : AuthorizedBaseController
    {
        public RouteController()
        {
        }

        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        public async Task<ActionResult> GetRouteList([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRouteViewQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Generate()
        {
            try
            {
                await Mediator.Send(new GenerateRouteCommand());
                return Json(new { Result = "OK", Message = "Route Generated." });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            return PartialView();
        }
    }
}