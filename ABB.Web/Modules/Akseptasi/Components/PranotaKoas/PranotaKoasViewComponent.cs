using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.PranotaKoas
{
    public class PranotaKoasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiPranotaParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_PranotaKoas", new AkseptasiPranotaKoasViewModel());
        }
    }
}