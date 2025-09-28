using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Services;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.RenewalReminder.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RenewalReminder.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RenewalReminder
{
    public class RenewalReminderController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public RenewalReminderController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View(new RenewalReminderViewModel()
            {
                kd_cb = Request.Cookies["UserCabang"]?.Trim() ?? string.Empty
            });
        }

        public async Task<JsonResult> GetCabang()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            dropdownData.AddRange(result);
            return Json(dropdownData);
        }

        public async Task<JsonResult> GetCOB()
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            dropdownData.AddRange(result);
            return Json(dropdownData);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] RenewalReminderViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetRenewalReminderQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("RenewalReminder.pdf", reportTemplate, sessionId, Orientation.Landscape);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}