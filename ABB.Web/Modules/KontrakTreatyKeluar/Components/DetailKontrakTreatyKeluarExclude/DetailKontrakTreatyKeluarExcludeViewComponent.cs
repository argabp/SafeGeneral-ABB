using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.DetailKontrakTreatyKeluarExclude
{
    public class DetailKontrakTreatyKeluarExcludeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DetailKontrakTreatyKeluarExclude");
        }
    }
}