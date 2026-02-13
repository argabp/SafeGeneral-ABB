using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.CancelCSM.Commands;
using ABB.Application.CancelCSM.Queries;
using ABB.Application.Common.Dtos;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesCSM.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.CancelCSM
{
    public class CancelCSMController : AuthorizedBaseController
    {
        private readonly ProgressBarDto _progressBarDto;

        public CancelCSMController(ProgressBarDto progressBarDto)
        {            
            _progressBarDto = progressBarDto;
        }
        
        public IActionResult Index()
        {

            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            _progressBarDto.ResetProgress();
            var model = new ProsesCSMViewModel();

            return View(model);
        }

        public JsonResult GetTipeTransaksi()
        {
            var result = new List<TipeTransaksi>()
            {
                new TipeTransaksi() { Text = "A1", Value = "A1" },
                new TipeTransaksi() { Text = "A2", Value = "A2" },
                new TipeTransaksi() { Text = "A3", Value = "A3" },
                new TipeTransaksi() { Text = "B1", Value = "B1" },
                new TipeTransaksi() { Text = "B2", Value = "B2" },
                new TipeTransaksi() { Text = "C1", Value = "C1" },
                new TipeTransaksi() { Text = "C2", Value = "C2" },
                new TipeTransaksi() { Text = "K1", Value = "K1" },
                new TipeTransaksi() { Text = "K2", Value = "K2" },
                new TipeTransaksi() { Text = "K3", Value = "K3" },
                new TipeTransaksi() { Text = "K4", Value = "K4" },
                new TipeTransaksi() { Text = "K5", Value = "K5" }
            };

            return Json(result);
        }

        public JsonResult GetKodeMetode()
        {
            var result = new List<KodeMetode>()
            {
                new KodeMetode() { Text = "PAA", Value = "1" },
                new KodeMetode() { Text = "GMM", Value = "2" }
            };

            return Json(result);
        }
        
        public async Task<ActionResult> GetViewSourceData([DataSourceRequest] DataSourceRequest request, ProsesCSMViewModel model)
        {
            var ds = await Mediator.Send(new GetViewSourceDataCancelQuery() { KodeMetode = model.KodeMetode});
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Proses([FromBody] ProsesViewModel model)
        {
            try
            {
                if(model.KodeMetode == "1")
                    await Mediator.Send(new ProsesPAACommand() { Id = model.Id, KodeMetode = model.KodeMetode, TipeTransaksi = model.TipeTransaksi, Type = "Manual"});
                else
                    await Mediator.Send(new ProsesGMMCommand() { Id = model.Id, KodeMetode = model.KodeMetode, TipeTransaksi = model.TipeTransaksi, Type = "Manual" });
                
                return Json(new { Result = "OK", Message = "Success Process Data" });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProsesAll([FromBody] ProsesViewModel model)
        {
            try
            {
                if(model.KodeMetode == "1")
                    await Mediator.Send(new ProsesPAACommand() { Id = model.Id, KodeMetode = model.KodeMetode, TipeTransaksi = model.TipeTransaksi, Type = "All"});
                else
                    await Mediator.Send(new ProsesGMMCommand() { Id = model.Id, KodeMetode = model.KodeMetode, TipeTransaksi = model.TipeTransaksi, Type = "All" });

                return Json(new { Result = "OK", Message = "Success Process All Data" });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        [HttpGet]
        public IActionResult GetProgress()
        {
            return Json(_progressBarDto);
        }

        [HttpGet]
        public IActionResult GetProgressDone()
        {
            _progressBarDto.ResetProgress();
            return Ok();
        }
    }
}