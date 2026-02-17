using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.ProsesCSM.Commands;
using ABB.Application.ProsesCSM.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesCSM.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesCSM
{
    public class ProsesCSMController : AuthorizedBaseController
    {
        private readonly ProgressBarDto _progressBarDto;

        public ProsesCSMController(ProgressBarDto progressBarDto)
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
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "A1", Value = "A1" },
                new DropdownOptionDto() { Text = "A2", Value = "A2" },
                new DropdownOptionDto() { Text = "A3", Value = "A3" },
                new DropdownOptionDto() { Text = "B1", Value = "B1" },
                new DropdownOptionDto() { Text = "B2", Value = "B2" },
                new DropdownOptionDto() { Text = "C1", Value = "C1" },
                new DropdownOptionDto() { Text = "C2", Value = "C2" },
                new DropdownOptionDto() { Text = "K1", Value = "K1" },
                new DropdownOptionDto() { Text = "K2", Value = "K2" },
                new DropdownOptionDto() { Text = "K3", Value = "K3" },
                new DropdownOptionDto() { Text = "K4", Value = "K4" },
                new DropdownOptionDto() { Text = "K5", Value = "K5" }
            };

            return Json(result);
        }

        public JsonResult GetKodeMetode()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAA", Value = "1" },
                new DropdownOptionDto() { Text = "GMM", Value = "2" }
            };

            return Json(result);
        }
        
        public async Task<ActionResult> GetViewSourceData(GridRequest grid, ProsesCSMViewModel model)
        {
            var result = await Mediator.Send(new GetViewSourceDataQuery()
            {
                KodeMetode = model.KodeMetode,
                Grid = grid
            });

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Proses([FromBody] ProsesViewModel model)
        {
            try
            {
                if(model.KodeMetode == "1")
                    await Mediator.Send(new ProsesPAACommand() { 
                        Datas = model.Datas,
                        Type = "Manual",
                        KodeMetode = model.KodeMetode
                    });
                else
                    await Mediator.Send(new ProsesGMMCommand() { 
                        Datas = model.Datas,
                        Type = "Manual",
                        KodeMetode = model.KodeMetode
                    });
                
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
                    await Mediator.Send(new ProsesPAACommand() {
                        Datas = model.Datas,
                        KodeMetode = model.KodeMetode,
                        Type = "All"
                    });
                else
                    await Mediator.Send(new ProsesGMMCommand() {
                        Datas = model.Datas,
                        KodeMetode = model.KodeMetode,
                        Type = "All"
                    });

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