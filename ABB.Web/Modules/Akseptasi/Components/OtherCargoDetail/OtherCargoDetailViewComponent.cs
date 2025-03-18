using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.OtherCargoDetail
{
    public class OtherCargoDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_OtherCargoDetail");
        }
    }
}