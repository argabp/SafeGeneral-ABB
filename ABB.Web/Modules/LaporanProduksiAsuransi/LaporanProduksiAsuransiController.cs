using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.LaporanProduksiAsuransi.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LaporanProduksiAsuransi.Models;
using DinkToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LaporanProduksiAsuransi
{
    public class LaporanProduksiAsuransiController : AuthorizedBaseController
    {
        private readonly IReportGeneratorService _reportGeneratorService;

        public LaporanProduksiAsuransiController(IReportGeneratorService reportGeneratorService)
        {
            _reportGeneratorService = reportGeneratorService;
        }
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View(new LaporanProduksiAsuransiViewModel()
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

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var dropdownData = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    Text = "",
                    Value = ""
                }
            };
            var result = await Mediator.Send(new GetSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            dropdownData.AddRange(result);
            return Json(dropdownData);
        }

        public async Task<JsonResult> GetKodeTertanggung()
        {
            var result = await Mediator.Send(new GetKodeTertanggungQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        [HttpGet]
        public async Task<JsonResult> GetKodeRekanan(string kd_grp_rk, string kd_cb)
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto()
                {
                    
                    Text = "",
                    Value = ""
                }
            };
            
            var rekanan = await Mediator.Send(new GetRekanansByKodeGroupAndCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rk = kd_grp_rk,
                kd_cb = kd_cb
            });
            
            result.AddRange(rekanan);

            return Json(result);
        }
        
        public JsonResult GetKodeMarketing()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "", Value = "" },
                new DropdownOptionDto() { Text = "Marketing", Value = "M" }
            };

            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] LaporanProduksiAsuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<GetLaporanProduksiAsuransiQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("LaporanProduksiAsuransi.pdf", reportTemplate, sessionId, Orientation.Landscape,
                    5, 5, 5, 5, PaperKind.Legal);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}