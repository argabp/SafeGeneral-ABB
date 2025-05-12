using System.Threading.Tasks;
using ABB.Application.DashboardUnderwriting.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DashboardUnderwriting
{
    public class DashboardUnderwritingController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            var model = await Mediator.Send(new GetDashboardUnderwritingQuery()
            {
                KodeCabang = Request.Cookies["UserCabang"],
                Cabang = Request.Cookies["UserCabang"],
            });
            
            return View(model);
        }
    }
}