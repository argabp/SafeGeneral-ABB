using System.Threading.Tasks;
using ABB.Application.DashboardKlaims.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DashboardKlaim
{
    public class DashboardKlaimController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = await Mediator.Send(new GetDashboardKlaimQuery()
            {
                KodeCabang = Request.Cookies["UserCabang"],
                Cabang = Request.Cookies["UserCabang"],
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return View(model);
        }
    }
}