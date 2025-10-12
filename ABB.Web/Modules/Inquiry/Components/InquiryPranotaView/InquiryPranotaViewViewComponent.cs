using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryPranotaView
{
    public class InquiryPranotaViewViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryParameterViewModel model)
        {
            return View("_InquiryPranotaView", new InquiryPranotaParameterViewModel()
            {
                kd_cb = model.kd_cb,
                kd_cob = model.kd_cob,
                kd_scob = model.kd_scob,
                kd_thn = model.kd_thn,
                no_pol = model.no_pol,
                no_updt = model.no_updt,
            });
        }
    }
}