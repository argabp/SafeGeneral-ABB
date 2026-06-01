using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.InquiryPremiFakultatifMasuk.Components.InquiryFakultatifOtherCargoDetail
{
    public class InquiryFakultatifOtherCargoDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            return View("_InquiryFakultatifOtherCargoDetail");
        }
    }
}