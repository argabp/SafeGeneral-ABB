using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.LaporanMaiparks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanMaipark.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.LaporanMaipark
{
    public class LaporanMaiparkController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new LaporanMaiparkViewModel();
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> GetLaporanMaiparks([FromBody] LaporanMaiparkViewModel model)
        {
            var command = Mapper.Map<GetLaporanMaiparksQuery>(model);
            var ds = await Mediator.Send(command);
            return Json(JsonConvert.DeserializeObject(ds));
        }
    }
}