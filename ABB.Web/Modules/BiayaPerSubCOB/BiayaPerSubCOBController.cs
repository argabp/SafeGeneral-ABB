using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.BiayaPerSubCOBs.Commands;
using ABB.Application.BiayaPerSubCOBs.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.BiayaPerSubCOB.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.BiayaPerSubCOB
{
    public class BiayaPerSubCOBController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetBiayaPerSubCOBs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetBiayaPerSubCOBsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] BiayaPerSubCOBViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveBiayaPerSubCOBCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan"});
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
        public async Task<IActionResult> Delete(string kd_mtu, string kd_cob, string kd_scob)
        {
            try
            {
                var command = new DeleteBiayaPerSubCOBCommand()
                    { kd_mtu = kd_mtu, kd_cob = kd_cob, kd_scob = kd_scob, 
                        DatabaseName = Request.Cookies["DatabaseValue"]};
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

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
            return PartialView(new BiayaPerSubCOBViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_mtu, string kd_cob, string kd_scob)
        {
            var biayaPerSubCOB = await Mediator.Send(new GetBiayaPerSubCOBQuery()
            {
                kd_mtu = kd_mtu,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            biayaPerSubCOB.kd_mtu = biayaPerSubCOB.kd_mtu.Trim();
            biayaPerSubCOB.kd_scob = biayaPerSubCOB.kd_scob.Trim();
            biayaPerSubCOB.kd_cob = biayaPerSubCOB.kd_cob.Trim();
            
            return PartialView(Mapper.Map<BiayaPerSubCOBViewModel>(biayaPerSubCOB));
        }
    }
}