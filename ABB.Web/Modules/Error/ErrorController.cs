using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Error
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/403")]
        public IActionResult Error403()
        {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            return View();
        }
    }
}