using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.PranotaKoas
{
    public class PranotaKoasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiPranotaParameterViewModel model)
        {
            return View("~/Modules/Akseptasi/Views/Empty.cshtml");
            
            return View("_PranotaKoas", new AkseptasiPranotaKoasViewModel());
        }
    }
}