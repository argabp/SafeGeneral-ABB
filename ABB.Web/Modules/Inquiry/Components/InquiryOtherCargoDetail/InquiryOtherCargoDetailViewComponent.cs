using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryOtherCargoDetail
{
    public class InquiryOtherCargoDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_InquiryOtherCargoDetail");
        }
    }
}