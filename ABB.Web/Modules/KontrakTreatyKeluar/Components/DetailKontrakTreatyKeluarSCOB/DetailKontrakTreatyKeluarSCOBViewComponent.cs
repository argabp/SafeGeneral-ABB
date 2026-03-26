using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.DetailKontrakTreatyKeluarSCOB
{
    public class DetailKontrakTreatyKeluarSCOBViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DetailKontrakTreatyKeluarSCOB");
        }
    }
}