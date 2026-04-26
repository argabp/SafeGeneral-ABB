using System.Threading.Tasks;
using ABB.Web.Modules.Alokasi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Alokasi.Components.Alokasi
{
    public class AlokasiViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(SORViewModel model)
        {
            return View("_Alokasi", new AlokasiViewModel());
        }
    }
}