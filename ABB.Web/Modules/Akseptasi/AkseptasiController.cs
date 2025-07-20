using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Alokasis.Commands;
using ABB.Application.Alokasis.Queries;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Akseptasi.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PolisInduk.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using GetPolisIndukQuery = ABB.Application.Akseptasis.Queries.GetPolisIndukQuery;

namespace ABB.Web.Modules.Akseptasi
{
    public class AkseptasiController : AuthorizedBaseController
    {
        private static List<RekananDto> _rekanans;

        public async Task<ActionResult> Index()
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

        #region Akspetasi
        
        public async Task<ActionResult> GetAkseptasis([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetAkseptasisQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<IActionResult> SaveAkseptasi(AkseptasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan", Model = entity });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt)
        {
            try
            {
                var command = new DeleteAkseptasiCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ClosingAkseptasi([FromBody] AkseptasiParameterViewModel model)
        {
            try
            {
                var command = Mapper.Map<ClosingAkseptasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CopyResiko([FromForm] CopyResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<CopyResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
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

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new AkseptasiParameterViewModel()
            {
                kd_cb = Request.Cookies["UserCabang"],
                kd_cob = string.Empty,
                kd_scob = string.Empty,
                kd_thn = string.Empty,
                no_aks = string.Empty
            };
            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt)
        {
            return PartialView(new AkseptasiParameterViewModel()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_cb = kd_cb
            });
        }

        [HttpGet]
        public async Task<IActionResult> KeteranganEndorsment(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt)
        {
            var command = new GetAkseptasiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_cb = kd_cb
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            
            return PartialView(Mapper.Map<KeteranganEndorsmentViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKeteranganEndorsment([FromBody] KeteranganEndorsmentViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKeteranganEndorsmentCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("KeteranganEndorsment", model);
        }
        
        public JsonResult GetStatusAkseptasi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "New", Value = "1" },
                new DropdownOptionDto() { Text = "Reject", Value = "2" },
                new DropdownOptionDto() { Text = "Pending", Value = "3" },
                new DropdownOptionDto() { Text = "Cancel", Value = "4" },
                new DropdownOptionDto() { Text = "False", Value = "5" },
                new DropdownOptionDto() { Text = "Reopen", Value = "6" },
                new DropdownOptionDto() { Text = "Endors", Value = "8" }
            };

            return Json(result);
        }
        
        public JsonResult GetStatusCoverPolis()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Hold Cover", Value = "H" },
                new DropdownOptionDto() { Text = "Cover Note", Value = "C" },
                new DropdownOptionDto() { Text = "Polis", Value = "X" }
            };

            return Json(result);
        }

        public async Task<JsonResult> GetNomorPolisInduk()
        {
            var result = await Mediator.Send(new GetNomorPolisInduksQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

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
        
        public JsonResult GetKodeBroker()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Agen Perorangan Lepas", Value = "0" },
                new DropdownOptionDto() { Text = "Broker", Value = "2" },
            };

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
        
        public JsonResult GetKodePersAsuransi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / Reas", Value = "5" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeBank()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Bank", Value = "6" }
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetTahunUnderwriting([FromBody] TahunUnderwritingViewModel model)
        {
            var command = Mapper.Map<GetTahunUnderwritingQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            var result = await Mediator.Send(command);

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetPolisInduk(string no_pol_induk)
        {
            var command = new GetPolisIndukQuery()
            {
                no_pol_induk = no_pol_induk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetJangkaWaktuPertanggungan([FromBody] JangkaWaktuPertanggunganViewModel model)
        {
            var command = Mapper.Map<GetJangkaWaktuPertanggunganQuery>(model);
            command.DatabaseName = Request.Cookies["DatabaseValue"];
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

        [HttpGet]
        public async Task<JsonResult> GetKodeAkseptasi(string st_pas, string kd_grp_sb_bis, string kd_rk_sb_bis)
        {
            var command = new GenerateKodeAkseptasiQuery()
            {
                st_pas = st_pas,
                kd_grp_sb_bis = kd_grp_sb_bis,
                kd_rk_sb_bis = kd_rk_sb_bis,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            
            var result = await Mediator.Send(command);

            return Json(result);
        }

        #endregion

        #region Resiko View

        #region Resiko

        public async Task<ActionResult> GetAkseptasiResikos([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt)
        {
            var ds = await Mediator.Send(new GetAkseptasiResikosQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiResiko([FromBody] AkseptasiResikoViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiResikoCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditResiko", model);
        }
        
        [HttpGet]
        public IActionResult AddResiko(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, 
            DateTime tgl_mul_ptg, DateTime tgl_akh_ptg)
        {
            var viewModel = new AkseptasiResikoViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.tgl_mul_ptg = tgl_mul_ptg;
            viewModel.tgl_akh_ptg = tgl_akh_ptg;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.kd_mtu_prm = "001";
            viewModel.pst_rate_prm = 0;
            viewModel.stn_rate_prm = 1;
            viewModel.nilai_prm = 0;
            viewModel.pst_dis = 0;
            viewModel.nilai_dis = 0;
            viewModel.pst_kms = 0;
            viewModel.nilai_kms = 0;
            viewModel.nilai_insentif = 0;
            viewModel.nilai_kl = 0;
            viewModel.nilai_ttl_ptg = 0;
            viewModel.pst_share_bgu = 100;
            viewModel.tgl_mul_ptg = DateTime.Now;
            viewModel.tgl_akh_ptg = DateTime.Now.AddYears(1);
            viewModel.jk_wkt_ptg = Convert.ToInt16((viewModel.tgl_akh_ptg.Value - viewModel.tgl_mul_ptg.Value).TotalDays);
            viewModel.faktor_prd = 100;

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditResiko(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt)
        {
            var akseptasiResiko = await Mediator.Send(new GetAkseptasiResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });
            
            return PartialView(Mapper.Map<AkseptasiResikoViewModel>(akseptasiResiko));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteResiko(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt)
        {
            try
            {
                var command = new DeleteAkseptasiResikoCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Coverage

        public async Task<ActionResult> GetAkseptasiCoverages([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetAkseptasiCoveragesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiCoverage([FromBody] AkseptasiResikoCoverageViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiCoverageCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditCoverage", model);
        }
        
        [HttpGet]
        public IActionResult AddCoverage(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, Int16 no_rsk)
        {
            var viewModel = new AkseptasiResikoCoverageViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.no_rsk = no_rsk;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.pst_rate_prm = 0;
            viewModel.stn_rate_prm = 1;
            viewModel.pst_dis = 0;
            viewModel.pst_kms = 0;
            
            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditCoverage(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_cvrg)
        {
            var akseptasiResiko = await Mediator.Send(new GetAkseptasiCoverageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                kd_cvrg = kd_cvrg
            });

            akseptasiResiko.kd_cvrg = akseptasiResiko.kd_cvrg.Trim();
            
            return PartialView(Mapper.Map<AkseptasiResikoCoverageViewModel>(akseptasiResiko));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteCoverage(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_cvrg)
        {
            try
            {
                var command = new DeleteAkseptasiCoverageCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    kd_cvrg = kd_cvrg,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<JsonResult> GetFlagPKK(string kd_cvrg)
        {
            var result = await Mediator.Send(new GetFlagPKKQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cvrg = kd_cvrg
            });

            return Json(result);
        }

        #endregion

        #region Obyek

        public async Task<ActionResult> GetAkseptasiObyeks([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetAkseptasiObyeksQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiObyek([FromBody] AkseptasiResikoObyekViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiObyekCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditObyek", model);
        }
        
        [HttpGet]
        public IActionResult AddObyek(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, Int16 no_rsk)
        {
            var viewModel = new AkseptasiResikoObyekViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.no_rsk = no_rsk;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.nilai_ttl_ptg = 0;
            viewModel.pst_adj = 100;
            
            //TODO harcoded
            viewModel.no_oby = 1;

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditObyek(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            var akseptasiResiko = await Mediator.Send(new GetAkseptasiObyekQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_oby = no_oby
            });
            
            return PartialView(Mapper.Map<AkseptasiResikoObyekViewModel>(akseptasiResiko));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteObyek(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            try
            {
                var command = new DeleteAkseptasiObyekCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    no_oby = no_oby,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Other
        
        [HttpPost]
        public async Task<IActionResult> GetResikoOther([FromBody] AkseptasiResikoParameterViewModel model)
        {

            switch (model.kd_cob.Trim())
            {
                case "B":
                    var akseptasiBondingViewModel = new AkseptasiResikoOtherBondingViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var bondingCommand = Mapper.Map<GetAkseptasiOtherBondingQuery>(model);
                    bondingCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var bondingResult = await Mediator.Send(bondingCommand);

                    if (bondingResult == null)
                    {
                        akseptasiBondingViewModel.grp_obl = "004";
                        akseptasiBondingViewModel.grp_kontr = "005";
                        akseptasiBondingViewModel.kd_rumus = "F";
                        akseptasiBondingViewModel.grp_jns_pekerjaan = "012";
                        akseptasiBondingViewModel.kd_grp_obl = "O";
                        akseptasiBondingViewModel.kd_grp_surety = "5";
                    
                        return PartialView("~/Modules/Akseptasi/Components/Other/_OtherBonding.cshtml", akseptasiBondingViewModel);
                    }
                
                    Mapper.Map(bondingResult, akseptasiBondingViewModel);
                    akseptasiBondingViewModel.kd_cb = akseptasiBondingViewModel.kd_cb.Trim();
                    akseptasiBondingViewModel.kd_cob = akseptasiBondingViewModel.kd_cob.Trim();
                    akseptasiBondingViewModel.kd_scob = akseptasiBondingViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherBonding.cshtml", akseptasiBondingViewModel);
                case "C":
                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherCargo.cshtml" , model);
                case "M":
                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherMotor.cshtml" , model);
                case "F":
                    var akseptasiFireViewModel = new AkseptasiResikoOtherFireViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var fireCommand = Mapper.Map<GetAkseptasiOtherFireQuery>(model);
                    fireCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var fireResult = await Mediator.Send(fireCommand);

                    if (fireResult == null)
                    {
                        akseptasiFireViewModel.kd_penerangan = "1";
                        akseptasiFireViewModel.kategori_gd = "E";
                    
                        return PartialView("~/Modules/Akseptasi/Components/Other/_OtherFire.cshtml", akseptasiFireViewModel);
                    }
                
                    Mapper.Map(fireResult, akseptasiFireViewModel);
                    akseptasiFireViewModel.kd_cb = akseptasiFireViewModel.kd_cb.Trim();
                    akseptasiFireViewModel.kd_cob = akseptasiFireViewModel.kd_cob.Trim();
                    akseptasiFireViewModel.kd_scob = akseptasiFireViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherFire.cshtml", akseptasiFireViewModel);
                case "H":
                    var akseptasiHullViewModel = new AkseptasiResikoOtherHullViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var hullCommand = Mapper.Map<GetAkseptasiOtherHullQuery>(model);
                    hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var hullResult = await Mediator.Send(hullCommand);

                    if (hullResult == null)
                    {
                        return PartialView("~/Modules/Akseptasi/Components/Other/_OtherHull.cshtml", akseptasiHullViewModel);
                    }
                
                    Mapper.Map(hullResult, akseptasiHullViewModel);
                    akseptasiHullViewModel.kd_cb = akseptasiHullViewModel.kd_cb.Trim();
                    akseptasiHullViewModel.kd_cob = akseptasiHullViewModel.kd_cob.Trim();
                    akseptasiHullViewModel.kd_scob = akseptasiHullViewModel.kd_scob.Trim();
                    akseptasiHullViewModel.merk_kapal = akseptasiHullViewModel.merk_kapal?.Trim();
                    akseptasiHullViewModel.kd_kapal = akseptasiHullViewModel.kd_kapal.Trim();

                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherHull.cshtml", akseptasiHullViewModel);
                case "P":
                    var akseptasiPaViewModel = new AkseptasiResikoOtherPAViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var paCommand = Mapper.Map<GetAkseptasiOtherPAQuery>(model);
                    paCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var paResult = await Mediator.Send(paCommand);

                    if (paResult == null)
                    {
                        akseptasiPaViewModel.thn_akh = "1";
                        akseptasiPaViewModel.nilai_harga_ptg = 0;
                        akseptasiPaViewModel.pst_rate_std = 0;
                        akseptasiPaViewModel.pst_rate_bjr = 0;
                        akseptasiPaViewModel.pst_rate_tl = 0;
                        akseptasiPaViewModel.pst_rate_gb = 0;
                        akseptasiPaViewModel.nilai_prm_std = 0;
                        akseptasiPaViewModel.nilai_prm_bjr = 0;
                        akseptasiPaViewModel.nilai_prm_tl = 0;
                        akseptasiPaViewModel.nilai_bia_adm = 0;
                        akseptasiPaViewModel.nilai_prm_btn = 0;
                        akseptasiPaViewModel.flag_std = "2";
                        akseptasiPaViewModel.flag_bjr = "2";
                        akseptasiPaViewModel.flag_tl = "1";
                        akseptasiPaViewModel.flag_gb = "1";
                        akseptasiPaViewModel.pst_rate_phk = 0;
                        akseptasiPaViewModel.nilai_prm_phk = 0;
                        akseptasiPaViewModel.nilai_bia_mat = 0;
                        akseptasiPaViewModel.nilai_ptg_std = 0;
                        akseptasiPaViewModel.nilai_ptg_bjr = 0;
                        akseptasiPaViewModel.nilai_ptg_tl = 0;
                        akseptasiPaViewModel.nilai_ptg_gb = 0;
                        akseptasiPaViewModel.nilai_ptg_hh = 0;
                        akseptasiPaViewModel.stn_rate_std = 10;
                        akseptasiPaViewModel.stn_rate_bjr = 10;
                        akseptasiPaViewModel.stn_rate_gb = 10;
                        akseptasiPaViewModel.stn_rate_tl = 10;
                        akseptasiPaViewModel.stn_rate_phk = 0;
                        akseptasiPaViewModel.kd_grp_asj = "5";
                    
                        return PartialView("~/Modules/Akseptasi/Components/Other/_OtherPA.cshtml", akseptasiPaViewModel);
                    }
                
                    Mapper.Map(paResult, akseptasiPaViewModel);
                    akseptasiPaViewModel.kd_cb = akseptasiPaViewModel.kd_cb.Trim();
                    akseptasiPaViewModel.kd_cob = akseptasiPaViewModel.kd_cob.Trim();
                    akseptasiPaViewModel.kd_scob = akseptasiPaViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherPA.cshtml", akseptasiPaViewModel);
                case "V":
                    var akseptasiHoleInOneViewModel = new AkseptasiResikoOtherHoleInOneViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_aks = model.no_aks
                    };
                    
                    var holeInOneCommand = Mapper.Map<GetAkseptasiOtherHoleInOneQuery>(model);
                    holeInOneCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var holeInOneResult = await Mediator.Send(holeInOneCommand);

                    if (holeInOneResult == null)
                    {
                        return PartialView("~/Modules/Akseptasi/Components/Other/_OtherHoleInOne.cshtml", akseptasiHoleInOneViewModel);
                    }
                
                    Mapper.Map(holeInOneResult, akseptasiHoleInOneViewModel);
                    akseptasiHoleInOneViewModel.kd_cb = akseptasiHoleInOneViewModel.kd_cb.Trim();
                    akseptasiHoleInOneViewModel.kd_cob = akseptasiHoleInOneViewModel.kd_cob.Trim();
                    akseptasiHoleInOneViewModel.kd_scob = akseptasiHoleInOneViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Akseptasi/Components/Other/_OtherHoleInOne.cshtml", akseptasiHoleInOneViewModel);
                default:
                    return NotFound();
            }
        }

        #region Other Fire

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherFire([FromBody] AkseptasiResikoOtherFireViewModel model)
        {
            try
            {
                var fireCommand = Mapper.Map<SaveAkseptasiOtherFireCommand>(model);
                fireCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(fireCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        #endregion

        #region Other Motor

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherMotor([FromBody] AkseptasiResikoOtherMotorViewModel model)
        {
            try
            {
                var motorCommand = Mapper.Map<SaveAkseptasiOtherMotorCommand>(model);
                motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(motorCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }
        
        public async Task<ActionResult> GetAkseptasiOtherMotorDetails([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetAkseptasiOtherMotorDetailsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_endt = kd_endt,
                no_rsk = no_rsk
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherMotorDetail([FromBody] AkseptasiResikoOtherMotorDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiOtherMotorDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("AddOtherMotorDetail", model);
        }
        
        [HttpGet]
        public IActionResult AddOtherMotorDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks)
        {
            var viewModel = new AkseptasiResikoOtherMotorDetailViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.nilai_casco = 0;
            viewModel.nilai_tjh = 0;
            viewModel.nilai_tjp = 0;
            viewModel.nilai_pap = 0;
            viewModel.nilai_pad = 0;
            viewModel.nilai_rsk_sendiri = 0;
            viewModel.nilai_prm_casco = 0;
            viewModel.nilai_prm_tjh = 0;
            viewModel.nilai_prm_tjp = 0;
            viewModel.nilai_prm_pap = 0;
            viewModel.nilai_prm_pad = 0;
            viewModel.nilai_prm_hh = 0;
            viewModel.st_deffered = "N";

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditOtherMotorDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            short no_rsk, string kd_endt, decimal thn_ptg_kend)
        {
            var akseptasiResikoOtherMotor = await Mediator.Send(new GetAkseptasiOtherMotorDetailQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                thn_ptg_kend = thn_ptg_kend
            });
            
            return PartialView(Mapper.Map<AkseptasiResikoOtherMotorDetailViewModel>(akseptasiResikoOtherMotor));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteOtherMotorDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            short no_rsk, string kd_endt, decimal thn_ptg_kend)
        {
            try
            {
                var command = new DeleteAkseptasiOtherMotorDetailCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    thn_ptg_kend = thn_ptg_kend,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Other Cargo
        
        public async Task<IActionResult> SaveAkseptasiOtherCargo(AkseptasiResikoOtherCargoViewModel model)
        {
            try
            {
                var motorCommand = Mapper.Map<SaveAkseptasiOtherCargoCommand>(model);
                motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(motorCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        public async Task<ActionResult> GetAkseptasiOtherCargoDetails([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetAkseptasiOtherCargoDetailsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_endt = kd_endt,
                no_rsk = no_rsk
            });
        
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherCargoDetail([FromBody] AkseptasiResikoOtherCargoDetailViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiOtherCargoDetailCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        
            return PartialView("AddOtherCargoDetail", model);
        }
        
        [HttpGet]
        public IActionResult AddOtherCargoDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks)
        {
            var viewModel = new AkseptasiResikoOtherCargoDetailViewModel();
        
            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
        
            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditOtherCargoDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            short no_rsk, string kd_endt, short no_urut)
        {
            var akseptasiResikoOtherCargo = await Mediator.Send(new GetAkseptasiOtherCargoDetailQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_urut = no_urut
            });

            akseptasiResikoOtherCargo.kd_angkut = akseptasiResikoOtherCargo.kd_angkut?.Trim();
            
            return PartialView(Mapper.Map<AkseptasiResikoOtherCargoDetailViewModel>(akseptasiResikoOtherCargo));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteOtherCargoDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            short no_rsk, string kd_endt, short no_urut)
        {
            try
            {
                var command = new DeleteAkseptasiOtherCargoDetailCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    no_urut = no_urut,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
        
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
        
        #region Other Bonding

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherBonding([FromBody] AkseptasiResikoOtherBondingViewModel model)
        {
            try
            {
                var bondingCommand = Mapper.Map<SaveAkseptasiOtherBondingCommand>(model);
                bondingCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(bondingCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        #endregion

        #region Other PA

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherPA([FromBody] AkseptasiResikoOtherPAViewModel model)
        {
            try
            {
                var paCommand = Mapper.Map<SaveAkseptasiOtherPACommand>(model);
                paCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(paCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        #endregion
        
        #region Other Hull

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherHull([FromBody] AkseptasiResikoOtherHullViewModel model)
        {
            try
            {
                var hullCommand = Mapper.Map<SaveAkseptasiOtherHullCommand>(model);
                hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(hullCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        #endregion
        
        #region Other Hole In One

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiOtherHoleInOne([FromBody] AkseptasiResikoOtherHoleInOneViewModel model)
        {
            try
            {
                var hullCommand = Mapper.Map<SaveAkseptasiOtherHoleInOneCommand>(model);
                hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(hullCommand);
                
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("Add", model);
        }

        #endregion
        
        #endregion

        #region Alokasi

        public async Task<ActionResult> GetDetailAlokasis([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, Int16 no_updt, Int16 no_rsk, string kd_endt, string no_pol)
        {
            var ds = await Mediator.Send(new GetDetailAlokasisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_pol = no_pol
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailAlokasi([FromBody] DetailAlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailAlokasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditDetailAlokasi", model);
        }
        
        [HttpGet]
        public IActionResult AddDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, Int16 no_rsk)
        {
            var viewModel = new DetailAlokasiViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_rsk = no_rsk;
            viewModel.no_updt = 0;
            viewModel.kd_endt = "I";
            viewModel.no_updt_reas = 0;
            viewModel.kd_grp_sb_bis = "5";
            viewModel.no_pol = "00000";
            
            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            var akseptasiResiko = await Mediator.Send(new GetDetailAlokasiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                kd_grp_sor = kd_grp_sor,
                kd_jns_sor = kd_jns_sor,
                kd_rk_sor = kd_rk_sor,
                no_pol = no_pol,
                no_updt_reas = no_updt_reas,
                kd_grp_sb_bis = kd_grp_sb_bis
            });
            
            return PartialView(Mapper.Map<DetailAlokasiViewModel>(akseptasiResiko));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            try
            {
                var command = new DeleteDetailAlokasiCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_updt = no_updt,
                    no_rsk = no_rsk,
                    kd_endt = kd_endt,
                    kd_grp_sor = kd_grp_sor,
                    kd_jns_sor = kd_jns_sor,
                    kd_rk_sor = kd_rk_sor,
                    no_pol = no_pol,
                    no_updt_reas = no_updt_reas,
                    kd_grp_sb_bis = kd_grp_sb_bis,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> ProsesAlokasi([FromBody] ProsesAlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesAlokasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                var result =  await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result});
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetGroupAndRekananSor(string kd_jns_sor, string kd_cob, string kd_cb,
            decimal thn_uw, decimal nilai_ttl_ptg, decimal nilai_prm)
        {
            try
            {
                var command = new GetGroupAndRekananSorQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    kd_jns_sor = kd_jns_sor,
                    kd_cob = kd_cob,
                    kd_cb = kd_cb,
                    thn_uw = thn_uw,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetJenisSor()
        {
            try
            {
                var command = new GetJenisSorQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                
                var result = await Mediator.Send(command);
                
                return Json(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRekananSor(string? jns_lookup)
        {
            try
            {
                if (jns_lookup == null)
                    return Ok();
                
                var command = new GetRekananSorQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    jns_lookup = jns_lookup
                };
                
                var result = await Mediator.Send(command);
                
                return Json(result);
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetShareAndPremiReas(decimal nilai_ttl_ptg_reas, string kd_jns_sor,
            decimal nilai_ttl_ptg, decimal nilai_prm, decimal net_prm)
        {
            try
            {
                var command = new GetShareAndPremiReasQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    nilai_ttl_ptg_reas = nilai_ttl_ptg_reas,
                    kd_jns_sor = kd_jns_sor,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm,
                    net_prm = net_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTTLAndPremiReas(decimal pst_share, decimal nilai_prm_reas,
            string kd_jns_sor, decimal nilai_prm, decimal net_prm, decimal nilai_ttl_ptg)
        {
            try
            {
                var command = new GetTTLAndPremiReasQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    pst_share = pst_share,
                    nilai_prm_reas = nilai_prm_reas,
                    kd_jns_sor = kd_jns_sor,
                    nilai_ttl_ptg = nilai_ttl_ptg,
                    nilai_prm = nilai_prm,
                    net_prm = net_prm
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetKmsReas(decimal pst_kms_reas, decimal nilai_prm_reas,
            decimal nilai_adj_reas)
        {
            try
            {
                var command = new GetKmsReasQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    pst_kms_reas = pst_kms_reas,
                    nilai_prm_reas = nilai_prm_reas,
                    nilai_adj_reas = nilai_adj_reas
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAdjReas(decimal pst_share, decimal pst_adj_reas,
            byte stn_adj_reas, decimal pst_kms, decimal nilai_prm_reas, decimal nilai_prm,
            decimal pst_rate_prm, byte stn_rate_prm, string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk)
        {
            try
            {
                var command = new GetAdjReasQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    pst_share = pst_share,
                    pst_adj_reas = pst_adj_reas,
                    stn_adj_reas = stn_adj_reas,
                    pst_kms = pst_kms,
                    nilai_prm_reas = nilai_prm_reas,
                    nilai_prm = nilai_prm,
                    pst_rate_prm = pst_rate_prm,
                    stn_rate_prm = stn_rate_prm,
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_pol = no_pol,
                    no_updt = no_updt,
                    no_rsk = no_rsk
                };
                
                var result = await Mediator.Send(command);

                return Ok(new { Status = "OK", Data = result });
            }
            catch (Exception e)
            {
                return Ok( new { Status = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message});
            }
        }

        #endregion

        #region Copy Endors
        
        public async Task<ActionResult> GetCopyEndors([DataSourceRequest] DataSourceRequest request,
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, string kd_thn, string no_pol, Int16 no_updt)
        {
            var ds = await Mediator.Send(new GetCopyEndorsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public IActionResult CopyEndors()
        {
            return PartialView();
        }
        
        [HttpPost]
        public async Task<IActionResult> CopyEndorsDelete([FromBody] CopyEndorsDto model)
        {
            try
            {
                var command = Mapper.Map<CopyEndorsUpdateDeleteCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("CopyEddors", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CopyEndorsUpdate([FromBody] CopyEndorsDto model)
        {
            try
            {
                var command = Mapper.Map<CopyEndorsUpdateDeleteCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("CopyEddors", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> CopyEndorsInsert([FromBody] CopyEndorsDto model)
        {
            try
            {
                var command = Mapper.Map<CopyEndorsInsertCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("CopyEddors", model);
        }

        #endregion
        
        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public JsonResult GetRatePremi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "%", Value = "1" },
                new DropdownOptionDto() { Text = "%o", Value = "10" }
            };

            return Json(result);
        }
        
        public JsonResult GetJangkaWaktu()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Flat", Value = "365" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeTOL(string kd_cob)
        {
            var result = await Mediator.Send(new GetKodeTOLQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cob = kd_cob
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKemendagri()
        {
            var result = await Mediator.Send(new GetKodeKemendagriQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeCoverage()
        {
            var result = await Mediator.Send(new GetKodeCoverageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetJaminan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Jaminan Pokok", Value = "Y" },
                new DropdownOptionDto() { Text = "Jaminan Tambahan", Value = "N" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeGroupObyek()
        {
            var result = await Mediator.Send(new GetKodeGroupObyekQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeSurety(string kd_cb, string kd_grp_surety)
        {
            var result = await Mediator.Send(new GetKodeSurety()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_grp_surety = kd_grp_surety
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKodeKontrak(string grp_kontr)
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = grp_kontr
            });

            return Json(result);
        }
        
        public JsonResult GetKodeRumus()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Proposional", Value = "P" },
                new DropdownOptionDto() { Text = "Flat", Value = "F" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeEndorsment()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Insert", Value = "I" },
                new DropdownOptionDto() { Text = "Delete", Value = "D" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetTTDSurety(string kd_cb)
        {
            var result = await Mediator.Send(new GetTTDSuretyQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeZona()
        {
            var result = await Mediator.Send(new GetKodeZonaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetLokasiResiko()
        {
            var result = await Mediator.Send(new GetLokasiResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodePropinsi()
        {
            var result = await Mediator.Send(new GetKodePropinsiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKabupaten(string? kd_prop)
        {
            if (string.IsNullOrWhiteSpace(kd_prop))
                return Json(new List<DropdownOptionDto>());
            
            var result = await Mediator.Send(new GetKodeKabupatenQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_prop = kd_prop
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKecamatan(string kd_prop, string kd_kab)
        {
            if (string.IsNullOrWhiteSpace(kd_prop) || string.IsNullOrWhiteSpace(kd_kab))
                return Json(new List<DropdownOptionDto>());
            
            var result = await Mediator.Send(new GetKodeKecamatanQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_prop = kd_prop,
                kd_kab = kd_kab
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKelurahan(string kd_prop, string kd_kab, string kd_kec)
        {
            if (string.IsNullOrWhiteSpace(kd_prop) || string.IsNullOrWhiteSpace(kd_kab) || string.IsNullOrWhiteSpace(kd_kec))
                return Json(new List<DropdownOptionDto>());
            
            var result = await Mediator.Send(new GetKodeKelurahanQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_prop = kd_prop,
                kd_kab = kd_kab,
                kd_kec = kd_kec
            });

            return Json(result);
        }
        
        public JsonResult GetKodePenerangan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Listrik", Value = "1" },
                new DropdownOptionDto() { Text = "Lain - lain", Value = "2" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKelasKonstruksi()
        {
            var result = await Mediator.Send(new GetKodeKelasKonstruksiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKodeOkupasi()
        {
            var result = await Mediator.Send(new GetKodeKodeOkupasiQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetKategoriGedung()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "A. Simple Risks", Value = "A" },
                new DropdownOptionDto() { Text = "B. Manufacturing Risks", Value = "B" },
                new DropdownOptionDto() { Text = "C. Manufacturing Risks", Value = "C" },
                new DropdownOptionDto() { Text = "D. Highrisk Buildings", Value = "D" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetWarnaKendaraan()
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = "009"
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetJenisKendaraan()
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = "001"
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetMerkKendaraan()
        {
            var result = await Mediator.Send(new GetGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_jns_grp = "M"
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetTipeKendaraan(string kd_grp_rsk)
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = kd_grp_rsk
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetJenisPertanggungan()
        {
            var result = await Mediator.Send(new GetJenisPertanggunganQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeUntuk()
        {
            var result = await Mediator.Send(new GetKodeUntukQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeGuna()
        {
            var result = await Mediator.Send(new GetKodeGunaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKapasitasMesin()
        {
            var result = await Mediator.Send(new GetGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_jns_grp = ""
            });

            return Json(result);
        }
        
        public JsonResult GetValiditas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Polis Sendiri", Value = "A" },
                new DropdownOptionDto() { Text = "Koasuransi", Value = "B" },
                new DropdownOptionDto() { Text = "Endors Penambahan", Value = "C" },
                new DropdownOptionDto() { Text = "Endors Pengurangan", Value = "D" }
            };

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
        
        public async Task<JsonResult> GetKodeAlatAngkut()
        {
            var result = await Mediator.Send(new GetKodeAlatAngkutQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKondisiPtg()
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = "003"
            });

            return Json(result);
        }
        
        public JsonResult GetTranshipment()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Allowed", Value = "Y" },
                new DropdownOptionDto() { Text = "Not Allowed", Value = "N" },
            };

            return Json(result);
        }

        public JsonResult GetJenisAngkut()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Truck", Value = "01" },
                new DropdownOptionDto() { Text = "Kapal Laut", Value = "02" },
                new DropdownOptionDto() { Text = "Pesawat Udara", Value = "03" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeKapal()
        {
            var result = await Mediator.Send(new GetKodeKapalQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
            });

            return Json(result);
        }

        public async Task<JsonResult> GetGroupObligee(string grp_obl)
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = grp_obl
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodePekerjaan(string kd_grp_rk)
        {
            var result = await Mediator.Send(new GetDetailGrupResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rsk = kd_grp_rk
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeJenisKredit(string kd_cb)
        {
            var result = await Mediator.Send(new GetKodeJenisKreditQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb
            });

            return Json(result);
        }

        public JsonResult GetJenisPembayaran()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Angsuran", Value = "A" },
                new DropdownOptionDto() { Text = "Tunai", Value = "T" }
            };

            return Json(result);
        }
        
        public async Task<JsonResult> GetJenisCover(string kd_cb, string kd_jns_kr)
        {
            var result = await Mediator.Send(new GetJenisCoverQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_cb = kd_cb,
                kd_jns_kr = kd_jns_kr
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetAsuransiJiwa(string kd_grp_asj)
        {
            var result = await Mediator.Send(new GetAsuransiJiwaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_asj = kd_grp_asj
            });

            return Json(result);
        }

        public JsonResult GetKelas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "1 (SATU) PEGAWAI NEGERI SIPIL (PNS)", Value = "1" },
                new DropdownOptionDto() { Text = "2 (DUA) BUMN/BUMD DAN PEGAWAI SWASTA", Value = "2" },
                new DropdownOptionDto() { Text = "3 (TIGA) TNI DAN POLRI", Value = "3" },
                new DropdownOptionDto() { Text = "4 (EMPAT) INDUSTRI, PERTAMBANGAN DAN SEJENISNYA", Value = "4" }
            };

            return Json(result);
        }

        public JsonResult GetFlagStd()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Resiko (A)", Value = "2" },
                new DropdownOptionDto() { Text = "Nill", Value = "1" },
                new DropdownOptionDto() { Text = "Resiko (A/B) ", Value = "3" }
            };

            return Json(result);
        }

        public JsonResult GetFlagBjr()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Resiko (B)", Value = "2" },
                new DropdownOptionDto() { Text = "Nill", Value = "1" }
            };

            return Json(result);
        }

        public JsonResult GetFlagGb()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Resiko (C)", Value = "2" },
                new DropdownOptionDto() { Text = "Nill", Value = "1" }
            };

            return Json(result);
        }

        public JsonResult GetFlagTl()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Resiko (ND)", Value = "2" },
                new DropdownOptionDto() { Text = "Nill", Value = "1" }
            };

            return Json(result);
        }

        public JsonResult GetStnRatePhk()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Non PHK", Value = "0" },
                new DropdownOptionDto() { Text = "PHK", Value = "1" },
                new DropdownOptionDto() { Text = "PAW", Value = "2" },
                new DropdownOptionDto() { Text = "MUSIMAN", Value = "3" }
            };

            return Json(result);
        }

        public JsonResult GetJenisKapal()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tanker", Value = "Tanker" },
                new DropdownOptionDto() { Text = "Bulk", Value = "Bulk" },
                new DropdownOptionDto() { Text = "General Cargo", Value = "General Cargo" },
                new DropdownOptionDto() { Text = "LCT", Value = "LCT" }
            };

            return Json(result);
        }

        #endregion

        #region Pranota View

        #region Tertanggung

        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiPranota([FromBody] AkseptasiPranotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiPranotaCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("~/Modules/Akseptasi/Components/Pranota/_Pranota.cshtml");
        }

        #endregion

        #region Koasuransi

        public async Task<ActionResult> GetAkseptasiPranotaKoass([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_aks, Int16 no_updt, string kd_mtu)
        {
            var ds = await Mediator.Send(new GetAkseptasiPranotaKoassQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_mtu = kd_mtu
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveAkseptasiPranotaKoas([FromBody] AkseptasiPranotaKoasViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveAkseptasiPranotaKoasCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }

            return PartialView("EditPranotaKoas", model);
        }
        
        [HttpGet]
        public IActionResult AddPranotaKoas(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, string kd_mtu)
        {
            var viewModel = new AkseptasiPranotaKoasViewModel();

            viewModel.kd_cb = kd_cb;
            viewModel.kd_cob = kd_cob;
            viewModel.kd_scob = kd_scob;
            viewModel.kd_thn = kd_thn;
            viewModel.no_aks = no_aks;
            viewModel.kd_mtu = kd_mtu;
            viewModel.no_updt = 0;
            viewModel.kd_grp_pas = "5";
            viewModel.pst_share = 0;
            viewModel.kd_prm = "PRM";
            viewModel.nilai_prm = 0;
            viewModel.pst_dis = 0;
            viewModel.nilai_dis = 0;
            viewModel.pst_kms = 0;
            viewModel.nilai_kms = 0;
            viewModel.pst_hf = decimal.Parse("2.50");
            viewModel.nilai_hf = 0;
            viewModel.nilai_kl = 0;
            viewModel.pst_pjk = 0;
            viewModel.nilai_pjk = 0;

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditPranotaKoas(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            string kd_mtu, string kd_grp_pas, string kd_rk_pas)
        {
            var akseptasiResiko = await Mediator.Send(new GetAkseptasiPranotaKoasQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_aks = no_aks,
                no_updt = no_updt,
                kd_mtu = kd_mtu,
                kd_grp_pas = kd_grp_pas,
                kd_rk_pas = kd_rk_pas
            });
            
            return PartialView(Mapper.Map<AkseptasiPranotaKoasViewModel>(akseptasiResiko));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeletePranotaKoas(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_aks, short no_updt, 
            string kd_mtu, string kd_grp_pas, string kd_rk_pas)
        {
            try
            {
                var command = new DeleteAkseptasiPranotaKoasCommand()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_aks = no_aks,
                    no_updt = no_updt,
                    kd_mtu = kd_mtu,
                    kd_grp_pas = kd_grp_pas,
                    kd_rk_pas = kd_rk_pas,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #endregion
    }
}