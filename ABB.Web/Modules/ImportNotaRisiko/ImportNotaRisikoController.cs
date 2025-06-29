using System;
using System.Threading.Tasks;
using ABB.Application.ImportNotaRisiko.Queries;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ImportNotaRisiko
{
    public class ImportNotaRisikoController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadNotaRisiko([FromForm] IFormFile file)
        {
            try
            {
                await Mediator.Send(new ImportNotaRisikoQuery()
                {
                    File = file
                });
                
                return Json("Berhasil mengunggah Nota Risiko");
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