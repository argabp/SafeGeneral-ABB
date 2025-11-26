using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryObyek
{
    public class InquiryObyekViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            
            return View("~/Modules/Inquiry/Views/Empty.cshtml");
        }
    }
}