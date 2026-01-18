using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Approvals.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Queries;
using ABB.Application.LimitKlaims.Commands;
using ABB.Application.LimitKlaims.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LimitKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LimitKlaim
{
    public class LimitKlaimController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetLimitKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLimitKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetLimitKlaimDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cb, string kd_cob, string kd_scob, int thn)
        {
            var ds = await Mediator.Send(new GetLimitKlaimDetilsQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                thn = thn,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            return View(new LimitKlaimViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LimitKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddLimitKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob, int thn)
        {
            var limitKlaim = await Mediator.Send(new GetLimitKlaimQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                thn = thn,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            limitKlaim.kd_cb = kd_cb.Trim();
            limitKlaim.kd_cob = kd_cob.Trim();
            limitKlaim.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<LimitKlaimViewModel>(limitKlaim));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] LimitKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditLimitKlaimCommand>(model);
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
        public async Task<IActionResult> Delete([FromBody] DeleteLimitKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteLimitKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public IActionResult AddDetail(string kd_cb, string kd_cob, string kd_scob, int thn)
        {
            return View(new LimitKlaimDetilViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                thn = thn
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] LimitKlaimDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddLimitKlaimDetilCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<IActionResult> EditDetil(string kd_cb, string kd_cob, string kd_scob, int thn, string kd_user)
        {
            var limitKlaimDetil = await Mediator.Send(new GetLimitKlaimDetilQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                thn = thn,
                kd_user = kd_user,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            limitKlaimDetil.kd_cb = kd_cb.Trim();
            limitKlaimDetil.kd_cob = kd_cob.Trim();
            limitKlaimDetil.kd_scob = kd_scob.Trim();
            
            return View("EditDetail",Mapper.Map<LimitKlaimDetilViewModel>(limitKlaimDetil));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromBody] LimitKlaimDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditLimitKlaimDetilCommand>(model);
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
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteLimitKlaimDetilViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteLimitKlaimDetilCommand>(model);
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