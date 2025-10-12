using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryPranota
{
    public class InquiryPranotaViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryPranotaParameterViewModel model)
        {
            return View("~/Modules/Inquiry/Components/InquiryPranota/_InquiryPranota.cshtml");
        }
    }
}