using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.InquiryPremiFakultatifMasuk.Components.InquiryFakultatifOtherMotorDetail
{
    public class InquiryFakultatifOtherMotorDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            return View("_InquiryFakultatifOtherMotorDetail");
        }
    }
}