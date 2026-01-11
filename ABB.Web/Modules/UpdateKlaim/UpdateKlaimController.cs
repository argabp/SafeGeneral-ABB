using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.ApprovalMutasiKlaims.Queries;
using ABB.Application.Common.Services;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Application.UpdateKlaims.Commands;
using ABB.Application.UpdateKlaims.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RegisterKlaim.Models;
using ABB.Web.Modules.UpdateKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.UpdateKlaim
{
    public class UpdateKlaimController : AuthorizedBaseController
     {
         private readonly IReportGeneratorService _reportGeneratorService;
 
         public UpdateKlaimController(IReportGeneratorService reportGeneratorService)
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
         
         public async Task<ActionResult> GetUpdateKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
         {
             var ds = await Mediator.Send(new GetUpdateKlaimsQuery() { SearchKeyword = searchkeyword, KodeCabang = Request.Cookies["UserCabang"], DatabaseName = Request.Cookies["DatabaseValue"]});
             
             var counter = 1;
             foreach (var data in ds)
             {
                 data.Id = counter;
                 data.register_klaim = "K." + data.kd_cb.Trim() + "." + data.kd_scob.Trim() 
                                             + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();

                 counter++;
             }
             return Json(ds.AsQueryable().ToDataSourceResult(request));
         }
         
         [HttpGet]
         public  IActionResult Info()
         {
             return PartialView();
         }
         
         public async Task<ActionResult> GetUpdateKlaimStatus([DataSourceRequest] DataSourceRequest request, UpdateStatusViewModel model)
         {
             var query = Mapper.Map<GetPengajuanKlaimStatusQuery>(model);
             query.DatabaseName = Request.Cookies["DatabaseValue"];
             var result = await Mediator.Send(query);
             return Json(result.AsQueryable().ToDataSourceResult(request));
         }
         
         public async Task<IActionResult> GetLampiranUpdateKlaimStatus([DataSourceRequest] DataSourceRequest request, UpdateStatusAttachmentViewModel model)
         {
             var query = Mapper.Map<GetPengajuanKlaimStatusAttachmentsQuery>(model);
             query.DatabaseName = Request.Cookies["DatabaseValue"];
             var result = await Mediator.Send(query);
             return Json(result.AsQueryable().ToDataSourceResult(request));
         }
         
         public IActionResult RejectView()
         {
             return View("Reject");
         }
         
         public IActionResult CloosedView()
         {
             return View("Closed");
         }
         
         public IActionResult FinalView()
         {
             return View("Final");
         }
 
         [HttpPost]
         public async Task<IActionResult> UpdateKlaim([FromBody] UpdateKlaimViewModel model)
         {
             try
             {
                 var command = Mapper.Map<UpdateKlaimCommand>(model);
                 command.DatabaseName = Request.Cookies["DatabaseValue"];
                 var result = await Mediator.Send(command);
 
                 foreach (var notifTo in result.Item2)
                 {
                     await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_berkas, model.status_name);
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
 
                 _reportGeneratorService.GenerateReport("UpdateKlaim.pdf", reportTemplate.Item1, sessionId, right: 10, top: 10, left: 10, bottom: 10);
                 _reportGeneratorService.GenerateReport("KeteranganUpdateKlaim.pdf", reportTemplate.Item2,
                     sessionId, right: 10, top: 10, left: 10, bottom: 10);
 
                 return Ok(new { Status = "OK", Data = sessionId });
             }
             catch (Exception e)
             {
                 return Ok(new
                     { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
             }
         }
     }
}