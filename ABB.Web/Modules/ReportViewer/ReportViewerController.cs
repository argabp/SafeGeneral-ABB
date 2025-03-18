using ABB.Web.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ReportViewer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.ReportViewer
{
    public class ReportViewerController : AuthorizedBaseController
    {
        public IActionResult Sertifikat()
        {
            return View();
        }
        
        public IActionResult LoadSertifikat(SertifikatModel sertifikatModel)
        {
            sertifikatModel.Url = $"{Request.Host.Value}{Request.Path}?${sertifikatModel.Url}";

            var model = JsonConvert.SerializeObject(sertifikatModel);

            return PartialView("ViewReport", model);
        }

        public IActionResult ViewReport(string reportName)
        {
            return View("_ViewReport", new ReportViewModel()
            {
                ReportName = reportName + ".html",
                FolderName = HttpContext.Session.GetString("SessionId")
            });
        }
    }
}