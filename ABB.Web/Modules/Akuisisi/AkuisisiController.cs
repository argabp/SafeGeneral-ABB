using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Akuisisis.Commands;
using ABB.Application.Akuisisis.Queries;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common.Exceptions;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Akuisisi.Models;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Akuisisi
{
    public class AkuisisiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetAkuisisis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAkuisisisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AkuisisiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkuisisiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Akuisisi"});
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
        public async Task<IActionResult> Delete(string kd_mtu, string kd_cob, string kd_scob, int kd_thn)
        {
            try
            {
                var command = new DeleteAkuisisiCommand()
                    { 
                        kd_mtu = kd_mtu, 
                        kd_cob = kd_cob, kd_scob = kd_scob, 
                        kd_thn = kd_thn, 
                        DatabaseName = Request.Cookies["DatabaseValue"]
                    };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Akuisisi"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
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
            return PartialView(new AkuisisiViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_mtu, string kd_cob, string kd_scob, int kd_thn)
        {
            var akuisisi = await Mediator.Send(new GetAkuisisiQuery()
            {
                kd_mtu = kd_mtu,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                kd_thn = kd_thn,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            akuisisi.kd_mtu = akuisisi.kd_mtu.Trim();
            akuisisi.kd_scob = akuisisi.kd_scob.Trim();
            akuisisi.kd_cob = akuisisi.kd_cob.Trim();
            
            return PartialView(Mapper.Map<AkuisisiViewModel>(akuisisi));
        }
    }
}