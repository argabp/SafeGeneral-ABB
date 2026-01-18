using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.ApprovalKlaims.Commands;
using ABB.Application.ApprovalKlaims.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Queries;
using ABB.Web.Modules.ApprovalKlaim.Models;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ApprovalKlaim
{
    public class ApprovalKlaimController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetApprovalKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetApprovalKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetApprovalKlaimDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cb, string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetApprovalKlaimDetailsQuery()
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
            return View(new ApprovalKlaimViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ApprovalKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddApprovalKlaimCommand>(model);
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
            var approval = await Mediator.Send(new GetApprovalKlaimQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            approval.kd_cb = kd_cb.Trim();
            approval.kd_cob = kd_cob.Trim();
            approval.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<ApprovalKlaimViewModel>(approval));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ApprovalKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditApprovalKlaimCommand>(model);
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
        public async Task<IActionResult> Delete([FromBody] DeleteApprovalKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteApprovalKlaimCommand>(model);
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
            return View(new ApprovalKlaimDetailViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] ApprovalKlaimDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddApprovalKlaimDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        public async Task<IActionResult> EditDetail(string kd_cb, string kd_cob, string kd_scob, Int16 kd_status,
                string kd_user, string kd_user_sign)
        {
            var detail = await Mediator.Send(new GetApprovalKlaimDetailQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_status = kd_status,
                kd_user = kd_user,
                kd_user_sign = kd_user_sign,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            detail.kd_cb = kd_cb.Trim();
            detail.kd_cob = kd_cob.Trim();
            detail.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<ApprovalKlaimDetailViewModel>(detail));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromBody] ApprovalKlaimDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditApprovalKlaimDetailCommand>(model);
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
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteApprovalKlaimDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteApprovalKlaimDetailCommand>(model);
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

        public async Task<JsonResult> GetKodeUserSign()
        {
            var result = await Mediator.Send(new GetKodeUserSignKlaimQuery());

            return Json(result);
        }

        public async Task<JsonResult> GetKodeStatus()
        {
            var result = await Mediator.Send(new GetKodeStatusKlaimQuery());

            return Json(result);
        }
    }
}