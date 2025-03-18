using System.Threading.Tasks;
using ABB.Application.Modules.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Home
{
    public class HomeController : AuthorizedBaseController
    {
        public HomeController()
        {
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var result = await Mediator.Send(new GetModuleByUserQuery());

            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SetModule([FromQuery] int moduleId)
        {
            Response.Cookies.Delete("Module");
            Response.Cookies.Append("Module", moduleId.ToString());

            return Ok();
        }
    }
}