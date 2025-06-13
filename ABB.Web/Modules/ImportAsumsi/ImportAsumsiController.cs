using System;
using System.Threading.Tasks;
using ABB.Application.ImportAsumsi.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ImportAsumsi
{
    public class ImportAsumsiController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsumsi([FromForm] IFormFile file)
        {
            try
            {
                await Mediator.Send(new UploadAsumsiQuery()
                {
                    File = file,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });
                
                return Json("Berhasil mengunggah Asumsi");
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    Error = exception.InnerException == null ? exception.Message : exception.InnerException.Message
                });
            }
        }
    }
}