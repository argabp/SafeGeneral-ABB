using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Inquiry.Components.InquiryPranotaKoas
{
    public class InquiryPranotaKoasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryPranotaParameterViewModel model)
        {
            return View("~/Modules/Inquiry/Components/InquiryPranotaKoas/_InquiryPranotaKoas.cshtml", new InquiryPranotaKoasViewModel());
        }
    }
}