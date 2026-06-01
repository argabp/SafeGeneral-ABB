using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.InquiryPremiFakultatifMasuk.Components.InquiryFakultatifPranotaKoas
{
    public class InquiryFakultatifPranotaKoasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryPranotaParameterViewModel model)
        {
            return View("~/Modules/Shared/Empty.cshtml");
        }
    }
}