using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Approvals.Commands;
using ABB.Application.Approvals.Queries;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Approval.Models;
using ABB.Web.Modules.Base;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Approval
{
    public class ApprovalController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetApprovals([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetApprovalsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetApprovalDetails([DataSourceRequest] DataSourceRequest request, 
            string kd_cb, string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetApprovalDetailsQuery()
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
            return View(new ApprovalViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ApprovalViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddApprovalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Approval"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob)
        {
            var approval = await Mediator.Send(new GetApprovalQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            approval.kd_cb = kd_cb.Trim();
            approval.kd_cob = kd_cob.Trim();
            approval.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<ApprovalViewModel>(approval));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ApprovalViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditApprovalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Approval"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteApprovalViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteApprovalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Approval"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public IActionResult AddDetail(string kd_cb, string kd_cob, string kd_scob)
        {
            return View(new ApprovalDetailViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail([FromBody] ApprovalDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddApprovalDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Approval Detail"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        public async Task<IActionResult> EditDetail(string kd_cb, string kd_cob, string kd_scob, Int16 kd_status)
        {
            var detail = await Mediator.Send(new GetApprovalDetailQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_status = kd_status,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            detail.kd_cb = kd_cb.Trim();
            detail.kd_cob = kd_cob.Trim();
            detail.kd_scob = kd_scob.Trim();
            
            return View(Mapper.Map<ApprovalDetailViewModel>(detail));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromBody] ApprovalDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditApprovalDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Approval Detail"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetail([FromBody] DeleteApprovalDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteApprovalDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Approval Detail"});

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