using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryOtherMotorDetail
{
    public class InquiryOtherMotorDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_InquiryOtherMotorDetail");
        }
    }
}