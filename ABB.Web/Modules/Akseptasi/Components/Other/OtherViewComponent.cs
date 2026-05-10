using System.Threading.Tasks;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.Other
{
    public class OtherViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            return View("Empty");
        }
    }
}