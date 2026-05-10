using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesPremiFakultatifMasuk.Components.FakultatifResikoView
{
    public class FakultatifResikoViewViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiParameterViewModel model)
        {
            return View("_FakultatifResikoView", new AkseptasiResikoParameterViewModel()
            {
                kd_cb = model.kd_cb,
                kd_cob = model.kd_cob,
                kd_scob = model.kd_scob,
                kd_thn = model.kd_thn,
                no_aks = model.no_aks,
                no_updt = model.no_updt,
            });
        }
    }
}