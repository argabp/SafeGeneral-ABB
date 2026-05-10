using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesPremiFakultatifMasuk.Components.FakultatifResiko
{
    public class FakultatifResikoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            return View("_FakultatifResiko", new AkseptasiResikoViewModel());
        }
    }
}