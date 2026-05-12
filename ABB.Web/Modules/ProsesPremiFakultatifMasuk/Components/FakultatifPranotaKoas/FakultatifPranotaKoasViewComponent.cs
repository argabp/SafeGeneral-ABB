using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesPremiFakultatifMasuk.Components.FakultatifPranotaKoas
{
    public class FakultatifPranotaKoasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiPranotaParameterViewModel model)
        {
            return View("~/Modules/Shared/Empty.cshtml");
        }
    }
}