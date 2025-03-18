using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Coverage
{
    public class CoverageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_Coverage", new AkseptasiResikoCoverageViewModel());
        }
    }
}