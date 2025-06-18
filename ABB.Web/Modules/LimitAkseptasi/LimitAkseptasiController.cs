using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Approvals.Queries;
using ABB.Application.Common;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.LimitAkseptasis.Commands;
using ABB.Application.LimitAkseptasis.Quries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LimitAkseptasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LimitAkseptasi
{
    public class LimitAkseptasiController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetLimitAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLimitAkseptasisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetLimitAkseptasiDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cb, string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetLimitAkseptasiDetilsQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            return View(new LimitAkseptasiViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LimitAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddLimitAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob)
        {
            var limitAkseptasi = await Mediator.Send(new GetLimitAkseptasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            limitAkseptasi.kd_cb = kd_cb.Trim();
            limitAkseptasi.kd_cob = kd_cob.Trim();
            limitAkseptasi.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<LimitAkseptasiViewModel>(limitAkseptasi));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] LimitAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditLimitAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteLimitAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteLimitAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public IActionResult AddDetail(string kd_cb, string kd_cob, string kd_scob)
        {
            return View(new LimitAkseptasiDetilViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] LimitAkseptasiDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddLimitAkseptasiDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteLimitAkseptasiDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteLimitAkseptasiDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
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

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

     
            return Json(result);
        }

        public async Task<JsonResult> GetUsers()
        {
            var result = await Mediator.Send(new GetKodeUserSignQuery());

            return Json(result);
        }

        public async Task<JsonResult> GetKodeStatus()
        {
            var result = await Mediator.Send(new GetKodeStatusQuery());

            return Json(result);
        }
    }
}