using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.DetailKontrakTreatyKeluarCoverage
{
    public class DetailKontrakTreatyKeluarCoverageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DetailKontrakTreatyKeluarCoverage");
        }
    }
}