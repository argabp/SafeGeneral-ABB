using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.ReportViewer
{
    public class ReportViewerViewComponent : ViewComponent
    {
        public ReportViewerViewComponent()
        {
        }

        public IViewComponentResult Invoke(string ReportName, string Parameters)
        {
            return View("_ReportViewer", new ReportViewerModel() { ReportName = ReportName, Parameters = Parameters});
        }
    }
}