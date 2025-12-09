using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.Common.Services;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.RegisterKlaims.Commands;
using ABB.Application.RegisterKlaims.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RegisterKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ABB.Web.Modules.RegisterKlaim
{
    public class RegisterKlaimController : AuthorizedBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IReportGeneratorService _reportGeneratorService;
        private readonly ILogger<RegisterKlaimController> _logger;
        private static List<DropdownOptionDto> _cabangs;
        private static List<DropdownOptionDto> _cobs;
        private static List<SCOBDto> _scobs;

        public RegisterKlaimController(IConfiguration configuration, IReportGeneratorService reportGeneratorService,
            ILogger<RegisterKlaimController> logger)
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
            
            _cabangs =  await Mediator.Send(new GetCabangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            _cobs = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            _scobs = await Mediator.Send(new GetAllSCOBQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return View();
        }
        
        public async Task<ActionResult> GetRegisterKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRegisterKlaimsQuery()
            {
                SearchKeyword = searchkeyword, 
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            foreach (var data in ds)
            {
                data.register_klaim = "K." + data.kd_cb.Trim() + "." + data.kd_scob.Trim() 
                                      + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        
        [HttpPost]
        public async Task<IActionResult> GetDokumenRegisterKlaims([FromBody] RegisterKlaimModel model)
        {
            var command = Mapper.Map<GetDokumenRegisterKlaimsQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            
            var result = await Mediator.Send(command);

            return Json(result);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> GetAkseptasiPolis([DataSourceRequest] DataSourceRequest request, 
            string searchKeyword, string kd_cb, string kd_cob, string kd_scob)
        {
            var ds = await Mediator.Send(new GetAkseptasisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = kd_cb,
                kd_scob = kd_scob,
                kd_cob = kd_cob,
                SearchKeyword = searchKeyword
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpGet]
        public  IActionResult Add()
        {
            var model = new RegisterKlaimModel();
            model.kd_cb = Request.Cookies["UserCabang"].Trim();
            return PartialView(model);
        }

        [HttpGet]
        public  IActionResult Edit(RegisterKlaimModel parameterModel)
        {
            return PartialView(parameterModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveRegisterKlaim([FromBody] RegisterKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveRegisterKlaimCommand>(model);
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

            return PartialView("~/Modules/Shared/Components/RegisterKlaim/_RegisterKlaim.cshtml" ,model);
        }

        public async Task<IActionResult> SaveDokumenRegisterKlaim(DokumenRegisterKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDokumenRegisterKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
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
        public async Task<IActionResult> SaveAnalisaDanEvaluasi([FromBody] AnalisaDanEvaluasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAnalisaDanEvaluasiCommand>(model);
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

            return PartialView("~/Modules/Shared/Components/AnalisaDanEvaluasi/_AnalisaDanEvaluasi.cshtml" ,model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDokumenRegisterKlaim([FromBody] DokumenRegisterKlaimModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDokumenRegisterKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);

                return Json(new { Result = "OK", Message = Constant.DataDisimpan });
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = "Failed Delete Lampiran " + e.Message });
            }
        }
        
        public JsonResult GetCabang()
        {
            return Json(_cabangs);
        }

        public JsonResult GetCOB()
        {
            return Json(_cobs);
        }

        public JsonResult GetSCOB(string kd_cob)
        {
            var result = new List<DropdownOptionDto>();

            if (string.IsNullOrWhiteSpace(kd_cob))
                kd_cob = string.Empty;
            
            foreach (var scob in _scobs.Where(w => w.kd_cob == kd_cob.Trim()))
            {
                result.Add(new DropdownOptionDto()
                {
                    Text = scob.nm_scob,
                    Value = scob.kd_scob
                });
            }

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
        
        public JsonResult GetJenisRegister()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Polis", Value = "N" },
                new DropdownOptionDto() { Text = "-", Value = "Y" }
            };

            return Json(result);
        }
        
        public JsonResult GetRefPolis()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Lama", Value = "Y" },
                new DropdownOptionDto() { Text = "Baru", Value = "N" }
            };

            return Json(result);
        }
        
        public JsonResult GetStatusKlaim()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Diajukan (PLA)", Value = "P" },
                new DropdownOptionDto() { Text = "Disetujui (DLA)", Value = "D" },
                new DropdownOptionDto() { Text = "Ditolak", Value = "T" }
            };

            return Json(result);
        }
        
        public JsonResult GetJenisPenyelesaian()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Teknis", Value = "T" },
                new DropdownOptionDto() { Text = "Ex-gratia", Value = "E" },
                new DropdownOptionDto() { Text = "Compromise", Value = "C" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeKlaim()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Total Loss", Value = "T" },
                new DropdownOptionDto() { Text = "Partial Loss", Value = "P" },
                new DropdownOptionDto() { Text = "TJH Pihak Ketiga", Value = "L" },
                new DropdownOptionDto() { Text = "Kecelakaan Diri", Value = "A" },
                new DropdownOptionDto() { Text = "Lain-lain", Value = "C" },
                new DropdownOptionDto() { Text = "Comprehensif", Value = "C" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeSebab(string kd_cob)
        {
            var result = await Mediator.Send(new GetKodeSebabQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeWilayah()
        {
            var result = await Mediator.Send(new GetKodeWilayahQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetUsers()
        {
            var result = await Mediator.Send(new GetUsersQuery());

            return Json(result);
        }

        public async Task<JsonResult> GetDocumentNames(string kd_cob)
        {
            var result = await Mediator.Send(new GetKodeDokumenQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }

        public async Task<JsonResult> GetPolLama(string flag_tty_msk)
        {
            var result = await Mediator.Send(new GetPolLamaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                flag_tty_msk = flag_tty_msk
            });

            return Json(result);
        }

        public async Task<JsonResult> GetSebabKerugian(string kd_cob, string kd_sebab)
        {
            var result = await Mediator.Send(new GetSebabKerugianQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob,
                kd_sebab = kd_sebab
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKodeTahun([FromBody] TglRegistrasiViewModel model)
        {
            var result = await Mediator.Send(new GetKodeTahunQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                tgl_reg = model.tgl_reg
            });

            return Json(result);
        }

        public async Task<JsonResult> GetTanggalDanBuktiLunas([FromBody] AkseptasiPolisViewModel model)
        {
            var query = Mapper.Map<GetTanggalDanBuktiLunasQuery>(model);
            query.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await Mediator.Send(query);

            return Json(result);
        }

        public async Task<JsonResult> GetKeterangan([FromBody] KeteranganViewModel model)
        {
            var query = Mapper.Map<GetKeteranganQuery>(model);
            query.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await Mediator.Send(query);

            return Json(result);
        }

        public IActionResult AkseptasiPolis()
        {
            return View();
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

                _reportGeneratorService.GenerateReport("RegisterKlaim.pdf", reportTemplate.Item1, sessionId);
                _reportGeneratorService.GenerateReport("KeteranganRegisterKlaim.pdf", reportTemplate.Item2,
                    sessionId);

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