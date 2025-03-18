using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Akuisisis.Commands;
using ABB.Application.Akuisisis.Queries;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common.Exceptions;
using ABB.Application.KapasitasCabangs.Commands;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Akuisisi.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KapasitasCabang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KapasitasCabang
{
    public class KapasitasCabangController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetKapasitasCabangs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKapasitasCabangsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] KapasitasCabangViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKapasitasCabangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Kapasitas Cabang"});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_cb, string kd_cob, string kd_scob, int thn)
        {
            try
            {
                var command = new DeleteKapasitasCabangCommand()
                {
                    kd_cb = kd_cb, 
                    kd_cob = kd_cob, 
                    kd_scob = kd_scob,
                    thn = thn,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Kapasitas Cabang"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
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

        public async Task<JsonResult> GetSCOB()
        {
            var result = await Mediator.Send(new GetSubCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new KapasitasCabangViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob, int thn)
        {
            var kapasitasCabang = await Mediator.Send(new GetKapasitasCabangQuery()
            {
                kd_cb = kd_cb,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                thn = thn,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            kapasitasCabang.kd_cb = kapasitasCabang.kd_cb.Trim();
            kapasitasCabang.kd_scob = kapasitasCabang.kd_scob.Trim();
            kapasitasCabang.kd_cob = kapasitasCabang.kd_cob.Trim();
            
            return PartialView(Mapper.Map<KapasitasCabangViewModel>(kapasitasCabang));
        }
    }
}