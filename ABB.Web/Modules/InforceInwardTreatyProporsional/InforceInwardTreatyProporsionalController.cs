using System;
using System.Threading.Tasks;
using ABB.Application.InforceInwardTreatyProporsional.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.InforceInwardTreatyProporsional.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.InforceInwardTreatyProporsional
{
    public class InforceInwardTreatyProporsionalController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new InforceInwardTreatyProporsionalModel();
            
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetTreatyProporsional([FromBody] InforceInwardTreatyProporsionalModel model)
        {
            try
            {
                var ds = await Mediator.Send(new GetTreatyProporsionalQuery()
                {
                    tgl_akhir = model.TanggalAkhir
                });
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}