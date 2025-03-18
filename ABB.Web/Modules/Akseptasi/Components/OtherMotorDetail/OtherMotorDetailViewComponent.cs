using System.Threading.Tasks;
using ABB.Application.Akseptasis.Queries;
using ABB.Web.Modules.Akseptasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akseptasi.Components.OtherMotorDetail
{
    public class OtherMotorDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AkseptasiResikoParameterViewModel model)
        {
            
            // return View("_Coverage", akseptasiViewModel);
            return View("_OtherMotorDetail");
        }
    }
}