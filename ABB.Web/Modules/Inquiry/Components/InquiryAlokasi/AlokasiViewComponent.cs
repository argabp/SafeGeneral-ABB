using System.Threading.Tasks;
using ABB.Web.Modules.Inquiry.Models;
using Microsoft.AspNetCore.Mvc;
using DetailAlokasiViewModel = ABB.Web.Modules.Akseptasi.Models.DetailAlokasiViewModel;

namespace ABB.Web.Modules.Inquiry.Components.InquiryAlokasi
{
    public class InquiryAlokasiViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(InquiryResikoParameterViewModel model)
        {
            return View("_InquiryAlokasi", new DetailAlokasiViewModel());
        }
    }
}