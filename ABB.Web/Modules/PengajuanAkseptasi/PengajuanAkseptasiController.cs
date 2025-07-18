using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PengajuanAkseptasi.Commands;
using ABB.Application.PengajuanAkseptasi.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Hubs;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PengajuanAkseptasi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Web.Modules.PengajuanAkseptasi
{
    public class PengajuanAkseptasiController : AuthorizedBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly ILogger<PengajuanAkseptasiController> _logger;
        private static List<RekananDto> _rekanans;

        public PengajuanAkseptasiController(IConfiguration configuration, IReportGeneratorService reportGeneratorService,
            ILogger<PengajuanAkseptasiController> logger)
        {
            _configuration = configuration;
            _reportGeneratorService = reportGeneratorService;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            _rekanans = await Mediator.Send(new GetRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            
            return View();
        }
        
        public async Task<ActionResult> GetPengajuanAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetPengajuanAkseptasisQuery() { SearchKeyword = searchkeyword, DatabaseName = Request.Cookies["DatabaseValue"]});
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        
        [HttpPost]
        public async Task<IActionResult> GetLampiranPengajuanAkseptasi([FromBody] PengajuanAkseptasiModel model)
        {
            var command = Mapper.Map<GetPengajuanAkseptasiAttachmentQuery>(model);
            var result = await Mediator.Send(command);

            return Json(result);
        }
        
        [HttpGet]
        public  IActionResult View(PengajuanAkseptasiModel parameterModel)
        {
            return PartialView(parameterModel);
        }
        
        [HttpGet]
        public  IActionResult Info()
        {
            return PartialView();
        }
        
        public async Task<ActionResult> GetPengajuanAkseptasiStatus([DataSourceRequest] DataSourceRequest request, PengajuanAkseptasiStatusViewModel model)
        {
            var query = Mapper.Map<GetPengajuanAkseptasiStatusQuery>(model);
            query.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await Mediator.Send(query);
            return Json(result.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<IActionResult> GetLampiranPengajuanAkseptasiStatus([DataSourceRequest] DataSourceRequest request, PengajuanAkseptasiStatusAttachmentViewModel model)
        {
            var query = Mapper.Map<GetPengajuanAkseptasiStatusAttachmentsQuery>(model);
            query.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await Mediator.Send(query);
            return Json(result.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<IActionResult> GenerateKeteranganResiko(string kd_cob, string kd_scob)
        {
            var result = await Mediator.Send(new GenerateKeteranganResikoQuery()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result);
        }

        public IActionResult Submit()
        {
            return View();
        }

        public IActionResult BatalAkseptasi()
        {
            return View();
        }
        
        public async Task<IActionResult> ApprovalAkseptasi(ApprovalPengajuanAkseptasiModel model)
        {
            try
            {
                var command = Mapper.Map<ApprovalPengajuanAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                var result = await Mediator.Send(command);

                foreach (var notifTo in result.Item2)
                {
                    await ApplicationHub.SendPengajuanAkseptasiNotification(notifTo, model.nomor_pengajuan, "Submit");
                }
                
                return Json(new { Result = "OK", Message = result.Item1 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
        
        [HttpGet]
        public  IActionResult Add()
        {
            var model = new PengajuanAkseptasiModel();
            model.kd_cb = Request.Cookies["UserCabang"].Trim();
            return PartialView(model);
        }

        [HttpGet]
        public  IActionResult Edit(PengajuanAkseptasiModel parameterModel)
        {
            return PartialView(parameterModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> SavePengajuanAkseptasi([FromBody] PengajuanAkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SavePengajuanAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("~/Modules/Shared/Components/PengajuanAkseptasi/_PengajuanAkseptasi.cshtml" ,model);
        }

        public async Task<IActionResult> SaveLampiranPengajuanAkseptasi(PengajuanAkseptasiAttachmentViewModel model)
        {
            try
            {
                var command = Mapper.Map<SavePengajuanAkspetasiAttachmentCommand>(model);
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
        
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                
                return Json(new { Result = "ERROR", Message = ex.Errors.FirstOrDefault().Value[0] });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLampiranPengajuanAkseptasi([FromBody] PengajuanAkseptasiAttachmentModel model)
        {
            try
            {
                var command = Mapper.Map<DeletePengajuanAkseptasiAttachmentCommand>(model);
                await Mediator.Send(command);

                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = "Failed Delete Lampiran " + e.Message });
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
        
        public JsonResult GetStatusPolis()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Leader (Sebagai Leader Koasuransi)", Value = "L" },
                new DropdownOptionDto() { Text = "Member (Sebagai Member Koasuransi)", Value = "M" },
                new DropdownOptionDto() { Text = "Transaksi Direct", Value = "O" },
                new DropdownOptionDto() { Text = "Inward Fakultatif", Value = "C" }
            };

            return Json(result);
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
            var result = new List<DropdownOptionDto>();

            foreach (var rekanan in _rekanans.Where(w => w.kd_grp_rk.Trim() == kd_grp_rk && w.kd_cb.Trim() == kd_cb))
            {
                result.Add(new DropdownOptionDto()
                {
                    Text = rekanan.nm_rk,
                    Value = rekanan.kd_rk
                });
            }

            return Json(result);
        }
        
        public JsonResult GetKodeMarketing()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Marketing", Value = "M" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeGrpPas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / Reas", Value = "5" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetDocumentNames()
        {
            var command = new GetDokumensQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }
        
        [HttpGet]
        public async Task<JsonResult> GetKodeRekananTertanggung(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var command = new GetKodeRekananTertanggungQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }

        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateReport([FromBody] PengajuanAkseptasiModel model)
        {
            try
            {
                var command = Mapper.Map<GetReportPengajuanAkseptasiQuery>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];

                var sessionId = HttpContext.Session.GetString("SessionId");

                if (string.IsNullOrWhiteSpace(sessionId))
                    throw new Exception("Session user tidak ditemukan");
                
                var reportTemplate = await Mediator.Send(command);
                
                _reportGeneratorService.GenerateReport("PengajuanAkseptasi.pdf", reportTemplate.Item1, sessionId);
                _reportGeneratorService.GenerateReport("KeteranganPengajuanAkseptasi.pdf", reportTemplate.Item2, sessionId);

                return Ok(new { Status = "OK", Data = sessionId});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public async Task<ActionResult> GeneratePstShare(string st_pas)
        {
            try
            {
                var result = await Mediator.Send(new GeneratePstShareQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    st_pas = st_pas
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
        
        public async Task<JsonResult> GetKodeTol(string kd_cob)
        {
            var command = new GetKodeTolQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }
        
        public async Task<ActionResult> GenerateNilaiLimit(string kd_cob, string kd_tol, decimal pst_share, decimal nilai_ttl_ptg)
        {
            try
            {
                var result = await Mediator.Send(new GenerateNilaiLimitQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    kd_cob = kd_cob,
                    kd_tol = kd_tol,
                    pst_share = pst_share,
                    nilai_ttl_ptg = nilai_ttl_ptg
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }
    }
}