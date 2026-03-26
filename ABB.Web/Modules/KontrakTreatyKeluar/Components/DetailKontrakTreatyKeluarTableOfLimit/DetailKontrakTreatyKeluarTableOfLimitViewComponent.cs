using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.DetailKontrakTreatyKeluarTableOfLimit
{
    public class DetailKontrakTreatyKeluarTableOfLimitViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DetailKontrakTreatyKeluarTableOfLimit");
        }
    }
}