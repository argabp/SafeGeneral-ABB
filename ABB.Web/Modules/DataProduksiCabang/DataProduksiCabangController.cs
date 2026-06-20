using System;
using System.Threading.Tasks;
using ABB.Application.Common.Queries;
using ABB.Application.DataProduksiCabangs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DataProduksiCabang.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.Web.Modules.DataProduksiCabang
{
    public class DataProduksiCabangController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            var model = new DataProduksiCabangViewModel()
            {
                kd_cb = Request.Cookies["UserCabang"]?.Trim() ?? string.Empty
            };
            
            return View(model);
        }

        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var result = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetDataProduksiCabangs([FromBody] DataProduksiCabangViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetDataProduksiCabangsQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                var ds = await Mediator.Send(command);
                
                return Json(JsonConvert.DeserializeObject(ds));
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}