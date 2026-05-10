using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesPremiFakultatifMasuk.Components.FakultatifCoverage
{
    public class CoverageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.kd_cob) && string.IsNullOrWhiteSpace(model.kd_scob))
            {
                return View("Empty");
            }
            
            return View("_FakultatifCoverage", new AkseptasiResikoCoverageViewModel());
        }
    }
}