using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.LaporanBPPDANs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanBPPDAN.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.LaporanBPPDAN
{
    public class LaporanBPPDANController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new LaporanBPPDANViewModel();
            
            return View(model);
        }

        public JsonResult GetJenisLaporan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Detil", Value = "D" },
                new DropdownOptionDto() { Text = "Rekap", Value = "R" }
            };

            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GetLaporanBPPDANs([FromBody] LaporanBPPDANViewModel model)
        {
            var command = Mapper.Map<GetLaporanBPPDANsQuery>(model);
            var ds = await Mediator.Send(command);
            return Json(JsonConvert.DeserializeObject(ds));
        }
    }
}