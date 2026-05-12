using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Coverage
{
    public class CoverageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.kd_cob) && string.IsNullOrWhiteSpace(model.kd_scob))
            {
                return View("~/Modules/Shared/Empty.cshtml");
            }
            
            return View("_Coverage", new AkseptasiResikoCoverageViewModel());
        }
    }
}