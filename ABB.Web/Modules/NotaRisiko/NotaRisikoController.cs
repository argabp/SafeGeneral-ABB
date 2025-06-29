using System;
using System.Threading.Tasks;
using ABB.Application.NotaResiko.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaRisiko.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.NotaRisiko
{
    public class NotaRisikoController : AuthorizedBaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new NotaRisikoViewModel();
            
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> GetSourceDatas([FromBody] NotaRisikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetSourceDataQuery>(model);
                var ds = await Mediator.Send(command);
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch
            {
                return Json(new { Error = "Connection Timeout" });
            }
        }
        
        [HttpGet]
        public  IActionResult View(Int64 id, string kodeMetode)
        {
            var notaRisikoModel = new NotaRisikoViewModel() { Id = id };
            return PartialView(kodeMetode == "1" ? "ViewMetode1" : "ViewMetode2", notaRisikoModel);
        }
    }
}