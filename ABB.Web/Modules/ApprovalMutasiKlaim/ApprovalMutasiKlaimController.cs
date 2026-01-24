using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.ApprovalMutasiKlaims.Commands;
using ABB.Application.ApprovalMutasiKlaims.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.MutasiKlaims.Commands;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Web.Modules.ApprovalMutasiKlaim.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.MutasiKlaim.Models;
using ABB.Web.Modules.RegisterKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ApprovalMutasiKlaim
{
    public class ApprovalMutasiKlaimController : AuthorizedBaseController
     {
         private readonly IReportGeneratorService _reportGeneratorService;
 
         public ApprovalMutasiKlaimController(IReportGeneratorService reportGeneratorService)
         {
             _reportGeneratorService = reportGeneratorService;
         }
         
         public async Task<IActionResult> Index()
         {
             ViewBag.Module = Request.Cookies["Module"];
             ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
             ViewBag.UserLogin = CurrentUser.UserId;
             
             return View();
         }
         
         public async Task<ActionResult> GetApprovalMutasiKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
         {
             var ds = await Mediator.Send(new GetApprovalMutasiKlaimsQuery() { SearchKeyword = searchkeyword, KodeCabang = Request.Cookies["UserCabang"], DatabaseName = Request.Cookies["DatabaseValue"]});
             return Json(ds.AsQueryable().ToDataSourceResult(request));
         }
         
         public async Task<IActionResult> Edit(RegisterKlaimModel parameterModel)
         {
             return PartialView(parameterModel);
         }
        
         [HttpPost]
         public async Task<IActionResult> Edit([FromBody] EditMutasiKlaimViewModel model)
         {
             try
             {
                 var command = Mapper.Map<SaveMutasiKlaimCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                 await Mediator.Send(command);
                 return Json(new { Result = "OK", Message = Constant.DataDisimpan});
             }
             catch (Exception ex)
             {
                 return Json(new { Result = "ERROR", ex.Message });
             }
         }
         
         [HttpGet]
         public  IActionResult Info()
         {
             return PartialView();
         }
         
         public async Task<ActionResult> GetApprovalMutasiKlaimStatus([DataSourceRequest] DataSourceRequest request, ApprovalMutasiStatusViewModel model)
         {
             var query = Mapper.Map<GetPengajuanKlaimStatusQuery>(model);
             query.DatabaseName = Request.Cookies["DatabaseValue"];
             var result = await Mediator.Send(query);
             return Json(result.AsQueryable().ToDataSourceResult(request));
         }
         
         public async Task<IActionResult> GetLampiranApprovalMutasiKlaimStatus([DataSourceRequest] DataSourceRequest request, ApprovalMutasiStatusAttachmentViewModel model)
         {
             var query = Mapper.Map<GetPengajuanKlaimStatusAttachmentsQuery>(model);
             query.DatabaseName = Request.Cookies["DatabaseValue"];
             var result = await Mediator.Send(query);
             return Json(result.AsQueryable().ToDataSourceResult(request));
         }
         
         public IActionResult ProcessView()
         {
             return View("Process");
         }
         
         public IActionResult SettledView()
         {
             return View("Settled");
         }
         
         public IActionResult EscalatedView()
         {
             return View("Escalated");
         }
         
         public IActionResult RevisedView()
         {
             return View("Revised");
         }
         
         public IActionResult ClosedView()
         {
             return View("Closed");
         }
         
         public async Task<IActionResult> ApprovalMutasiKlaim(ApprovalMutasiKlaimModel model)
         {
             try
             {
                 var command = Mapper.Map<ApprovalMutasiKlaimCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                 var result = await Mediator.Send(command);
 
                 foreach (var notifTo in result.Item2)
                 {
                     await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_berkas, model.status_name);
                 }
                 
                 return Json(new { Result = "OK", Message = model.status_name + " Sucessfully" });
             }
             catch (Exception e)
             {
                 return Json(new
                     { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
         
         public async Task<IActionResult> ApprovalMutasiKlaimClosed(ApprovalMutasiKlaimModel model)
         {
             try
             {
                 var command = Mapper.Map<ApprovalMutasiKlaimClosedCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                 var result = await Mediator.Send(command);
 
                 foreach (var notifTo in result.Item2)
                 {
                     await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_berkas, model.status_name);
                 }
                 
                 return Json(new { Result = "OK", Message = model.status_name + " Sucessfully" });
             }
             catch (Exception e)
             {
                 return Json(new
                     { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
         
         public async Task<IActionResult> ApprovalMutasiKlaimEsc(ApprovalMutasiKlaimEscModel model)
         {
             try
             {
                 var command = Mapper.Map<ApprovalMutasiKlaimEscCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                 var result = await Mediator.Send(command);
 
                 foreach (var notifTo in result.Item2)
                 {
                     await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_berkas, "Escalated");
                 }
                 
                 return Json(new { Result = "OK", Message = result.Item1 });
             }
             catch (Exception e)
             {
                 return Json(new
                     { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
         
         public async Task<IActionResult> ApprovalMutasiKlaimRev(ApprovalMutasiKlaimEscModel model)
         {
             try
             {
                 var command = Mapper.Map<ApprovalMutasiKlaimRevCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                 var result = await Mediator.Send(command);
 
                 foreach (var notifTo in result.Item2)
                 {
                     await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_berkas, "Revised");
                 }
                 
                 return Json(new { Result = "OK", Message = result.Item1 });
             }
             catch (Exception e)
             {
                 return Json(new
                     { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
 
         [HttpPost]
         public async Task<ActionResult> GenerateReport([FromBody] RegisterKlaimModel model)
         {
             try
             {
                 var command = Mapper.Map<GetReportPengajuanKlaimQuery>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
 
                 var sessionId = HttpContext.Session.GetString("SessionId");
 
                 if (string.IsNullOrWhiteSpace(sessionId))
                     throw new Exception("Session user tidak ditemukan");
 
                 var reportTemplate = await Mediator.Send(command);
 
                 _reportGeneratorService.GenerateReport("ApprovalMutasiKlaim.pdf", reportTemplate.Item1, sessionId, right: 10, top: 10, left: 10, bottom: 10);
                 _reportGeneratorService.GenerateReport("KeteranganApprovalMutasiKlaim.pdf", reportTemplate.Item2,
                     sessionId, right: 10, top: 10, left: 10, bottom: 10);
 
                 return Ok(new { Status = "OK", Data = sessionId });
             }
             catch (Exception e)
             {
                 return Ok(new
                     { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
 
         public async Task<JsonResult> GetUserSign()
         {
             var result = await Mediator.Send(new GetUserSignEscKlaimQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"],kd_cb = Request.Cookies["UserCabang"] ?? string.Empty
             });
 
             return Json(result);
         }
     
         public async Task<JsonResult> GetUserSignRevised(string kd_cb, string kd_cob, string kd_scob, string kd_thn, string no_kl)
         {
             var result = await Mediator.Send(new GetUserSignRevisedQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"],
                 kd_cb = kd_cb,
                 kd_cob = kd_cob,
                 kd_scob = kd_scob,
                 kd_thn = kd_thn,
                 no_kl = no_kl
             });
     
             return Json(result);
         }
         
         public async Task<JsonResult> GetCabang()
         {
             var cabangs = await Mediator.Send(new GetCabangQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"]
             });
             
             return Json(cabangs);
         }

         public async Task<JsonResult> GetCOB()
         {
             var cobs = await Mediator.Send(new GetCobQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"]
             });
             
             return Json(cobs);
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
        
         public async Task<JsonResult> GetMataUang()
         {
             var mataUang = await Mediator.Send(new GetMataUangQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"]
             });
             
             return Json(mataUang);
         }
        
         public JsonResult GetTipeMutasi()
         {
             var tipeMutasi = new List<DropdownOptionDto>()
             {
                 new DropdownOptionDto() { Text = "PLA", Value = "P" },
                 new DropdownOptionDto() { Text = "DLA", Value = "D" },
                 new DropdownOptionDto() { Text = "Beban", Value = "B" },
                 new DropdownOptionDto() { Text = "Recovery", Value = "R" }
             };
             
             return Json(tipeMutasi);
         }
        
         public JsonResult GetValiditas()
         {
             var result = new List<DropdownOptionDto>()
             {
                 new DropdownOptionDto() { Text = "Klaim Normal", Value = "A" },
                 new DropdownOptionDto() { Text = "Ex Gratia", Value = "B" },
                 new DropdownOptionDto() { Text = "Salvage", Value = "C" },
                 new DropdownOptionDto() { Text = "Subrogari", Value = "D" }
             };

             return Json(result);
         }
        
         public JsonResult GetKeterangan()
         {
             var result = new List<DropdownOptionDto>()
             {
                 new DropdownOptionDto() { Text = "Adjuster Fee", Value = "Adjuster Fee" },
                 new DropdownOptionDto() { Text = "lawyer Fee", Value = "lawyer Fee" },
                 new DropdownOptionDto() { Text = "Surveyor Fee", Value = "Surveyor Fee" },
                 new DropdownOptionDto() { Text = "Biaya Survei", Value = "Biaya Survei" },
                 new DropdownOptionDto() { Text = "Solicter Fee", Value = "Solicter Fee" },
                 new DropdownOptionDto() { Text = "Biaya Lain-lain", Value = "Biaya Lain-lain" }
             };

             return Json(result);
         }
        
         public async Task<JsonResult> GetUsers()
         {
             var users = await Mediator.Send(new GetUsersQuery());
             
             return Json(users);
         }
        
         public JsonResult GetGroupRekanan()
         {
             var result = new List<DropdownOptionDto>()
             {
                 new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
             };

             return Json(result);
         }
        
         public async Task<JsonResult> GetRekanan(string kd_grp_pas)
         {
             var result = await Mediator.Send(new GetRekanansByKodeGroupQuery()
             {
                 DatabaseName = Request.Cookies["DatabaseValue"],
                 kd_grp_rk = kd_grp_pas
             });

             return Json(result);
         }
     }
}