using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar.Components.DetailKontrakTreatyKeluarKoasuransi
{
    public class DetailKontrakTreatyKeluarKoasuransiViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DetailKontrakTreatyKeluarKoasuransi");
        }
    }
}