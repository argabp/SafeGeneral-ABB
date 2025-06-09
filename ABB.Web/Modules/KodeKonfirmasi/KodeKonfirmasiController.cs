using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.KodeKonfirmasis.Commands;
using ABB.Application.KodeKonfirmasis.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KodeKonfirmasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KodeKonfirmasi
{
    public class KodeKonfirmasiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKodeKonfirmasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetKodeKonfirmasisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] KodeKonfirmasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddKodeKonfirmasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                command.UserId = CurrentUser.UserId;
                var kodeKonfirmasi = await Mediator.Send(command);
                return PartialView("Add", Mapper.Map<KodeKonfirmasiViewModel>(kodeKonfirmasi));
                // return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_cb, string kd_cob, string kd_scob, string kd_thn, string no_aks, string kd_konfirm)
        {
            try
            {
                var command = new DeleteKodeKonfirmasiCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    kd_konfirm = kd_konfirm,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new KodeKonfirmasiViewModel();
            return PartialView(viewModel);
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
    }
}