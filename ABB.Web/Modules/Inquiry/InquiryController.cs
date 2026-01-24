using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Inquiries.Queries;
using ABB.Application.Alokasis.Commands;
using ABB.Application.Alokasis.Queries;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.Inquiries.Commands;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Inquiry.Models;
using ABB.Web.Modules.PolisInduk.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using GetKodeWilayahQuery = ABB.Application.Common.Queries.GetKodeWilayahQuery;

namespace ABB.Web.Modules.Inquiry
{
    public class InquiryController : AuthorizedBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        #region Inquiry
        
        public async Task<ActionResult> GetInquiries([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetInquiriesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            
            var statusPolis = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Leader (Sebagai Leader Koasuransi)", Value = "L" },
                new DropdownOptionDto() { Text = "Member (Sebagai Member Koasuransi)", Value = "M" },
                new DropdownOptionDto() { Text = "Transaksi Direct", Value = "O" },
                new DropdownOptionDto() { Text = "Inward Fakultatif", Value = "C" }
            };

            foreach (var data in ds)
            {
                data.st_pas = statusPolis.FirstOrDefault(w => w.Value == data.st_pas)?.Text ?? string.Empty;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
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
        
        [HttpGet]
        public async Task<IActionResult> View(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt)
        {
            return PartialView(new InquiryParameterViewModel()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_cb = kd_cb
            });
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
        public async Task<JsonResult> GetKodeRekanan(string kd_grp_rk, string kd_cb, string no_fax)
        {
            List<DropdownOptionDto> result;
            
            if (string.IsNullOrWhiteSpace(no_fax))
            {
                result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    kd_cb = kd_cb,
                    kd_grp_rk = kd_grp_rk
                });
            }
            else
            {
                result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangAndNoFaxQuery()
                {
                    DatabaseName = Request.Cookies["DatabaseValue"],
                    kd_cb = kd_cb,
                    kd_grp_rk = kd_grp_rk,
                    no_fax = no_fax
                });
            }

            return Json(result);
        }
        
        [HttpGet]
        public async Task<JsonResult> GetObligees(string kd_grp_rk)
        {
            var result = await Mediator.Send(new GetObligeeQueries()
            {
                kd_grp_rk = kd_grp_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
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

        #endregion

        #region Resiko View

        #region Resiko

        public async Task<ActionResult> GetInquiryResikos([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt)
        {
            var ds = await Mediator.Send(new GetInquiryResikosQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewResiko(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryResikoQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            inquiryResiko.kd_tol = inquiryResiko.kd_tol?.Trim();
            
            return PartialView(Mapper.Map<InquiryResikoViewModel>(inquiryResiko));
        }

        #endregion

        #region Coverage

        public async Task<ActionResult> GetInquiryCoverages([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryCoveragesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewCoverage(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_cvrg)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryCoverageQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                kd_cvrg = kd_cvrg
            });

            inquiryResiko.kd_cvrg = inquiryResiko.kd_cvrg.Trim();
            
            return PartialView(Mapper.Map<InquiryResikoCoverageViewModel>(inquiryResiko));
        }

        #endregion

        #region Obyek

        public async Task<ActionResult> GetInquiryObyeks([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryObyeksQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetInquiryObyekCITs([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryObyekCITsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetInquiryObyekCISs([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryObyekCISsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewObyekFire(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryObyekQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_oby = no_oby
            });

            var data = Mapper.Map<InquiryResikoObyekViewModel>(inquiryResiko);
            
            return PartialView(data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewObyekCIT(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryObyekCITQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_oby = no_oby
            });

            var data = Mapper.Map<InquiryResikoObyekCITViewModel>(inquiryResiko);
            
            return PartialView(data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewObyekCIS(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryObyekCISQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_oby = no_oby
            });

            var data = Mapper.Map<InquiryResikoObyekCISViewModel>(inquiryResiko);
            
            return PartialView(data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewObyekFireFull(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            Int16 no_rsk, string kd_endt, Int16 no_oby)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryObyekQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_oby = no_oby
            });

            var data = Mapper.Map<InquiryResikoObyekViewModel>(inquiryResiko);
            
            return PartialView(data);
        }

        #endregion

        #region Other
        
        [HttpPost]
        public async Task<IActionResult> GetResikoOther([FromBody] InquiryResikoParameterViewModel model)
        {
            switch (model.kd_cob.Trim())
            {
                case "B":
                    var inquiryBondingViewModel = new InquiryResikoOtherBondingViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var bondingCommand = Mapper.Map<GetInquiryOtherBondingQuery>(model);
                    bondingCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var bondingResult = await Mediator.Send(bondingCommand);

                    if (bondingResult == null)
                    {
                        inquiryBondingViewModel.grp_obl = "004";
                        inquiryBondingViewModel.grp_kontr = "005";
                        inquiryBondingViewModel.kd_rumus = "F";
                        inquiryBondingViewModel.grp_jns_pekerjaan = "012";
                        inquiryBondingViewModel.kd_grp_obl = "O";
                        inquiryBondingViewModel.kd_grp_surety = "5";
                        inquiryBondingViewModel.kd_rk_surety = "000214";
                        inquiryBondingViewModel.kd_grp_prc = "P";
                    
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherBonding.cshtml", inquiryBondingViewModel);
                    }
                
                    Mapper.Map(bondingResult, inquiryBondingViewModel);
                    inquiryBondingViewModel.kd_cb = inquiryBondingViewModel.kd_cb.Trim();
                    inquiryBondingViewModel.kd_cob = inquiryBondingViewModel.kd_cob.Trim();
                    inquiryBondingViewModel.kd_scob = inquiryBondingViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherBonding.cshtml", inquiryBondingViewModel);
                case "C":
                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherCargo.cshtml" , model);
                case "M":var inquiryMotorViewModel = new InquiryResikoOtherMotorViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var motorCommand = Mapper.Map<GetInquiryOtherMotorQuery>(model);
                    motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var motorResult = await Mediator.Send(motorCommand);

                    if (motorResult == null)
                    {
                        inquiryMotorViewModel.grp_jns_kend = "001";
                        inquiryMotorViewModel.kd_guna = "000";
                        inquiryMotorViewModel.nilai_casco = 0;
                        inquiryMotorViewModel.nilai_tjh = 0;
                        inquiryMotorViewModel.nilai_tjp = 0;
                        inquiryMotorViewModel.nilai_pap = 0;
                        inquiryMotorViewModel.nilai_pad = 0;
                        inquiryMotorViewModel.pst_rate_prm = 0;
                        inquiryMotorViewModel.stn_rate_prm = 1;
                        inquiryMotorViewModel.pst_rate_hh = 0;
                        inquiryMotorViewModel.stn_rate_hh = 1;
                        inquiryMotorViewModel.nilai_rsk_sendiri = 0;
                        inquiryMotorViewModel.nilai_prm_casco = 0;
                        inquiryMotorViewModel.nilai_prm_tjh = 0;
                        inquiryMotorViewModel.nilai_prm_tjp = 0;
                        inquiryMotorViewModel.nilai_prm_pap = 0;
                        inquiryMotorViewModel.nilai_prm_pad = 0;
                        inquiryMotorViewModel.nilai_prm_hh = 0;
                        inquiryMotorViewModel.nilai_pap_med = 0;
                        inquiryMotorViewModel.nilai_pad_med = 0;
                        inquiryMotorViewModel.nilai_prm_pap_med = 0;
                        inquiryMotorViewModel.nilai_prm_pad_med = 0;
                        inquiryMotorViewModel.nilai_prm_aog = 0;
                        inquiryMotorViewModel.pst_rate_aog = 0;
                        inquiryMotorViewModel.stn_rate_prm = 1;
                        inquiryMotorViewModel.pst_rate_banjir = 0;
                        inquiryMotorViewModel.stn_rate_banjir = 1;
                        inquiryMotorViewModel.nilai_prm_banjir = 0;
                        inquiryMotorViewModel.validitas = "A";
                        inquiryMotorViewModel.pst_rate_trs = 0;
                        inquiryMotorViewModel.stn_rate_trs = 1;
                        inquiryMotorViewModel.nilai_prm_trs = 0;
                        inquiryMotorViewModel.pst_rate_tjh = 0;
                        inquiryMotorViewModel.stn_rate_tjh = 1;
                        inquiryMotorViewModel.pst_rate_tjp = 0;
                        inquiryMotorViewModel.stn_rate_tjp = 1;
                        inquiryMotorViewModel.pst_rate_pap = 0;
                        inquiryMotorViewModel.stn_rate_pap = 1;
                        inquiryMotorViewModel.pst_rate_pad = 0;
                        inquiryMotorViewModel.stn_rate_pad = 1;
                        inquiryMotorViewModel.kd_endt = "I";

                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherMotor.cshtml", inquiryMotorViewModel);
                    }
                
                    Mapper.Map(motorResult, inquiryMotorViewModel);
                    inquiryMotorViewModel.kd_cb = inquiryMotorViewModel.kd_cb.Trim();
                    inquiryMotorViewModel.kd_cob = inquiryMotorViewModel.kd_cob.Trim();
                    inquiryMotorViewModel.kd_scob = inquiryMotorViewModel.kd_scob.Trim();
                    inquiryMotorViewModel.kd_guna = inquiryMotorViewModel.kd_guna.Trim();
                    inquiryMotorViewModel.validitas = inquiryMotorViewModel.validitas.Trim();
                    inquiryMotorViewModel.kd_jns_ptg = inquiryMotorViewModel.kd_jns_ptg.Trim();
                    inquiryMotorViewModel.warna_kend = inquiryMotorViewModel.warna_kend?.Trim();
                    inquiryMotorViewModel.kd_wilayah = inquiryMotorViewModel.kd_wilayah?.Trim();
                    
                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherMotor.cshtml", inquiryMotorViewModel);
                case "F":
                    var inquiryFireViewModel = new InquiryResikoOtherFireViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var fireCommand = Mapper.Map<GetInquiryOtherFireQuery>(model);
                    fireCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var fireResult = await Mediator.Send(fireCommand);

                    if (fireResult == null)
                    {
                        inquiryFireViewModel.kd_penerangan = "1";
                        inquiryFireViewModel.kategori_gd = "E";
                    
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherFire.cshtml", inquiryFireViewModel);
                    }
                
                    Mapper.Map(fireResult, inquiryFireViewModel);
                    inquiryFireViewModel.kd_cb = inquiryFireViewModel.kd_cb.Trim();
                    inquiryFireViewModel.kd_cob = inquiryFireViewModel.kd_cob.Trim();
                    inquiryFireViewModel.kd_scob = inquiryFireViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherFire.cshtml", inquiryFireViewModel);
                case "H":
                    var inquiryHullViewModel = new InquiryResikoOtherHullViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var hullCommand = Mapper.Map<GetInquiryOtherHullQuery>(model);
                    hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var hullResult = await Mediator.Send(hullCommand);

                    if (hullResult == null)
                    {
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHull.cshtml", inquiryHullViewModel);
                    }
                
                    Mapper.Map(hullResult, inquiryHullViewModel);
                    inquiryHullViewModel.kd_cb = inquiryHullViewModel.kd_cb.Trim();
                    inquiryHullViewModel.kd_cob = inquiryHullViewModel.kd_cob.Trim();
                    inquiryHullViewModel.kd_scob = inquiryHullViewModel.kd_scob.Trim();
                    inquiryHullViewModel.merk_kapal = inquiryHullViewModel.merk_kapal?.Trim();
                    inquiryHullViewModel.kd_kapal = inquiryHullViewModel.kd_kapal.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHull.cshtml", inquiryHullViewModel);
                case "P":
                    var inquiryPaViewModel = new InquiryResikoOtherPAViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var paCommand = Mapper.Map<GetInquiryOtherPAQuery>(model);
                    paCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var paResult = await Mediator.Send(paCommand);

                    if (paResult == null)
                    {
                        inquiryPaViewModel.thn_akh = "1";
                        inquiryPaViewModel.nilai_harga_ptg = 0;
                        inquiryPaViewModel.pst_rate_std = 0;
                        inquiryPaViewModel.pst_rate_bjr = 0;
                        inquiryPaViewModel.pst_rate_tl = 0;
                        inquiryPaViewModel.pst_rate_gb = 0;
                        inquiryPaViewModel.nilai_prm_std = 0;
                        inquiryPaViewModel.nilai_prm_bjr = 0;
                        inquiryPaViewModel.nilai_prm_tl = 0;
                        inquiryPaViewModel.nilai_bia_adm = 0;
                        inquiryPaViewModel.nilai_prm_btn = 0;
                        inquiryPaViewModel.flag_std = "2";
                        inquiryPaViewModel.flag_bjr = "2";
                        inquiryPaViewModel.flag_tl = "1";
                        inquiryPaViewModel.flag_gb = "1";
                        inquiryPaViewModel.pst_rate_phk = 0;
                        inquiryPaViewModel.nilai_prm_phk = 0;
                        inquiryPaViewModel.nilai_bia_mat = 0;
                        inquiryPaViewModel.nilai_ptg_std = 0;
                        inquiryPaViewModel.nilai_ptg_bjr = 0;
                        inquiryPaViewModel.nilai_ptg_tl = 0;
                        inquiryPaViewModel.nilai_ptg_gb = 0;
                        inquiryPaViewModel.nilai_ptg_hh = 0;
                        inquiryPaViewModel.stn_rate_std = 10;
                        inquiryPaViewModel.stn_rate_bjr = 10;
                        inquiryPaViewModel.stn_rate_gb = 10;
                        inquiryPaViewModel.stn_rate_tl = 10;
                        inquiryPaViewModel.stn_rate_phk = 0;
                        inquiryPaViewModel.kd_grp_asj = "5";
                        inquiryPaViewModel.nilai_prm_gb = 0;
                        inquiryPaViewModel.nilai_ptg_phk = 0;
                    
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherPA.cshtml", inquiryPaViewModel);
                    }
                
                    Mapper.Map(paResult, inquiryPaViewModel);
                    inquiryPaViewModel.kd_cb = inquiryPaViewModel.kd_cb.Trim();
                    inquiryPaViewModel.kd_cob = inquiryPaViewModel.kd_cob.Trim();
                    inquiryPaViewModel.kd_scob = inquiryPaViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherPA.cshtml", inquiryPaViewModel);
                case "V":
                    var inquiryHoleInOneViewModel = new InquiryResikoOtherHoleInOneViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var holeInOneCommand = Mapper.Map<GetInquiryOtherHoleInOneQuery>(model);
                    holeInOneCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var holeInOneResult = await Mediator.Send(holeInOneCommand);

                    if (holeInOneResult == null)
                    {
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHoleInOne.cshtml", inquiryHoleInOneViewModel);
                    }
                
                    Mapper.Map(holeInOneResult, inquiryHoleInOneViewModel);
                    inquiryHoleInOneViewModel.kd_cb = inquiryHoleInOneViewModel.kd_cb.Trim();
                    inquiryHoleInOneViewModel.kd_cob = inquiryHoleInOneViewModel.kd_cob.Trim();
                    inquiryHoleInOneViewModel.kd_scob = inquiryHoleInOneViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHoleInOne.cshtml", inquiryHoleInOneViewModel);
                default:
                    return NotFound();
            }
        }
        
        #region Other Motor

        public async Task<ActionResult> GetInquiryOtherMotorDetails([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryOtherMotorDetailsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_endt = kd_endt,
                no_rsk = no_rsk
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewOtherMotorDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            short no_rsk, string kd_endt, decimal thn_ptg_kend)
        {
            var inquiryResikoOtherMotor = await Mediator.Send(new GetInquiryOtherMotorDetailQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                thn_ptg_kend = thn_ptg_kend
            });
            
            return PartialView(Mapper.Map<InquiryResikoOtherMotorDetailViewModel>(inquiryResikoOtherMotor));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetKodeOkupasiDetail(string kd_zona, string kd_kls_konstr, 
            string kd_okup, string kd_scob)
        {
            var result = await Mediator.Send(new GetKodeOkupasiDetailQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_zona = kd_zona,
                kd_kls_konstr = kd_kls_konstr,
                kd_okup = kd_okup,
                kd_scob = kd_scob
            });
            
            return Ok(result);
        }

        #endregion

        #region Other Cargo

        public async Task<ActionResult> GetInquiryOtherCargoDetails([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, Int16 no_rsk, string kd_endt)
        {
            var ds = await Mediator.Send(new GetInquiryOtherCargoDetailsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_endt = kd_endt,
                no_rsk = no_rsk
            });
        
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewOtherCargoDetail(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            short no_rsk, string kd_endt, short no_urut)
        {
            var inquiryResikoOtherCargo = await Mediator.Send(new GetInquiryOtherCargoDetailQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                no_rsk = no_rsk,
                kd_endt = kd_endt,
                no_urut = no_urut
            });

            inquiryResikoOtherCargo.kd_angkut = inquiryResikoOtherCargo.kd_angkut?.Trim();
            
            return PartialView(Mapper.Map<InquiryResikoOtherCargoDetailViewModel>(inquiryResikoOtherCargo));
        }

        #endregion
        
        #region Other Bonding

        public JsonResult GetKodePrincipal()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Principal", Value = "P" }
            };

            return Json(result);
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
                no_pol = no_pol,
                SearchKeyword = searchkeyword
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewDetailAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string kd_grp_sor, short no_updt, 
            Int16 no_rsk, string kd_endt, string kd_jns_sor, string kd_rk_sor,
            string no_pol, Int16 no_updt_reas, string kd_grp_sb_bis)
        {
            var inquiryResiko = await Mediator.Send(new GetDetailAlokasiQuery()
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
            
            return PartialView(Mapper.Map<DetailAlokasiViewModel>(inquiryResiko));
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

        public async Task<JsonResult> GetLokasi()
        {
            var result = await Mediator.Send(new GetLokasisQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

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
        
        public async Task<JsonResult> GetKodePropinsi()
        {
            var result = await Mediator.Send(new GetKodeWilayahQuery()
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
        
        public async Task<ActionResult> GetInquiryPranotas([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt)
        {
            var ds = await Mediator.Send(new GetInquiryPranotasQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewPranota(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, string kd_mtu)
        {
            var inquiryPranota = await Mediator.Send(new GetInquiryPranotaQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_mtu = kd_mtu
            });
            
            return PartialView(Mapper.Map<InquiryPranotaViewModel>(inquiryPranota));
        }

        #endregion

        #region Koasuransi

        public async Task<ActionResult> GetInquiryPranotaKoass([DataSourceRequest] DataSourceRequest request, 
            string searchkeyword, string kd_cb, string kd_cob, string kd_scob, 
            string kd_thn, string no_pol, Int16 no_updt, string kd_mtu)
        {
            var ds = await Mediator.Send(new GetInquiryPranotaKoassQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_mtu = kd_mtu
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpGet]
        public async Task<IActionResult> ViewPranotaKoas(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_pol, short no_updt, 
            string kd_mtu, string kd_grp_pas, string kd_rk_pas)
        {
            var inquiryResiko = await Mediator.Send(new GetInquiryPranotaKoasQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_pol = no_pol,
                no_updt = no_updt,
                kd_mtu = kd_mtu,
                kd_grp_pas = kd_grp_pas,
                kd_rk_pas = kd_rk_pas
            });
            
            return PartialView(Mapper.Map<InquiryPranotaKoasViewModel>(inquiryResiko));
        }

        #endregion

        #endregion
        
        

        [HttpGet]
        public async Task<IActionResult> CheckCoverage(string kd_cob, string kd_scob)
        {
            var result = await Mediator.Send(new CheckCoverageCommand()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            if (string.IsNullOrWhiteSpace(result.Item1))
            {
                return PartialView("~/Modules/Inquiry/Views/EmptyWithMessage.cshtml", result.Item2);
            }
            else
            {
                return PartialView("~/Modules/Inquiry/Components/InquiryCoverage/_InquiryCoverage.cshtml", new InquiryResikoCoverageViewModel());
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> CheckObyek(string kd_cob, string kd_scob, decimal? pst_share)
        {
            var result = await Mediator.Send(new CheckObyekCommand()
            {
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                pst_share = pst_share ?? 0,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            if (string.IsNullOrWhiteSpace(result.Item1))
            {
                return View("~/Modules/Inquiry/Views/EmptyWithMessage.cshtml", result.Item2);
            }

            var resultView = result.Item1.Split(",")[2];
            switch (Constant.AkseptasiObyekViewMapping[resultView])
            {
                case "_ObyekFire":
                    return View("~/Modules/Inquiry/Components/InquiryObyek/_InquiryObyekFire.cshtml",
                        new InquiryResikoObyekViewModel());
                case "_ObyekFireFull":
                    return View("~/Modules/Inquiry/Components/InquiryObyek/_InquiryObyekFireFull.cshtml",
                        new InquiryResikoObyekViewModel());
                case "_ObyekCIS":
                    return View("~/Modules/Inquiry/Components/InquiryObyek/_InquiryObyekCIS.cshtml",
                        new InquiryResikoObyekCISViewModel());
                case "_ObyekCIT":
                    return View("~/Modules/Inquiry/Components/InquiryObyek/_InquiryObyekCIT.cshtml",
                        new InquiryResikoObyekCITViewModel());
                default:
                    return View("~/Modules/Inquiry/Views/EmptyWithMessage.cshtml");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckOther([FromBody] InquiryResikoParameterViewModel model)
        {
            var result = await Mediator.Send(new CheckOtherCommand()
            {
                kd_cob = model.kd_cob,
                kd_scob = model.kd_scob,
                pst_share = model.pst_share ?? 0,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            

            if (string.IsNullOrWhiteSpace(result.Item1))
            {
                return View("~/Modules/Inquiry/Views/EmptyWithMessage.cshtml", result.Item2);
            }

            var resultView = result.Item1.Split(",")[2];
            switch (Constant.AkseptasiOtherViewMapping[resultView])
            {
                case "_OtherBonding":
                    var inquiryBondingViewModel = new InquiryResikoOtherBondingViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var bondingCommand = Mapper.Map<GetInquiryOtherBondingQuery>(model);
                    bondingCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var bondingResult = await Mediator.Send(bondingCommand);

                    if (bondingResult == null)
                    {
                        inquiryBondingViewModel.grp_obl = "004";
                        inquiryBondingViewModel.grp_kontr = "005";
                        inquiryBondingViewModel.kd_rumus = "F";
                        inquiryBondingViewModel.grp_jns_pekerjaan = "012";
                        inquiryBondingViewModel.kd_grp_obl = "O";
                        inquiryBondingViewModel.kd_grp_surety = "5";
                        inquiryBondingViewModel.kd_rk_surety = "000214";
                        inquiryBondingViewModel.kd_grp_prc = "P";
                    
                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherBonding.cshtml", inquiryBondingViewModel);
                    }
                
                    Mapper.Map(bondingResult, inquiryBondingViewModel);
                    inquiryBondingViewModel.kd_cb = inquiryBondingViewModel.kd_cb.Trim();
                    inquiryBondingViewModel.kd_cob = inquiryBondingViewModel.kd_cob.Trim();
                    inquiryBondingViewModel.kd_scob = inquiryBondingViewModel.kd_scob.Trim();

                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherBonding.cshtml", inquiryBondingViewModel);
                case "_OtherCargo":
                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherCargo.cshtml", model);
                case "_OtherMotor":
                    var inquiryMotorViewModel = new InquiryResikoOtherMotorViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var motorCommand = Mapper.Map<GetInquiryOtherMotorQuery>(model);
                    motorCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var motorResult = await Mediator.Send(motorCommand);

                    if (motorResult == null)
                    {
                        inquiryMotorViewModel.grp_jns_kend = "001";
                        inquiryMotorViewModel.kd_guna = "000";
                        inquiryMotorViewModel.nilai_casco = 0;
                        inquiryMotorViewModel.nilai_tjh = 0;
                        inquiryMotorViewModel.nilai_tjp = 0;
                        inquiryMotorViewModel.nilai_pap = 0;
                        inquiryMotorViewModel.nilai_pad = 0;
                        inquiryMotorViewModel.pst_rate_prm = 0;
                        inquiryMotorViewModel.stn_rate_prm = 1;
                        inquiryMotorViewModel.pst_rate_hh = 0;
                        inquiryMotorViewModel.stn_rate_hh = 1;
                        inquiryMotorViewModel.nilai_rsk_sendiri = 0;
                        inquiryMotorViewModel.nilai_prm_casco = 0;
                        inquiryMotorViewModel.nilai_prm_tjh = 0;
                        inquiryMotorViewModel.nilai_prm_tjp = 0;
                        inquiryMotorViewModel.nilai_prm_pap = 0;
                        inquiryMotorViewModel.nilai_prm_pad = 0;
                        inquiryMotorViewModel.nilai_prm_hh = 0;
                        inquiryMotorViewModel.nilai_pap_med = 0;
                        inquiryMotorViewModel.nilai_pad_med = 0;
                        inquiryMotorViewModel.nilai_prm_pap_med = 0;
                        inquiryMotorViewModel.nilai_prm_pad_med = 0;
                        inquiryMotorViewModel.nilai_prm_aog = 0;
                        inquiryMotorViewModel.pst_rate_aog = 0;
                        inquiryMotorViewModel.stn_rate_prm = 1;
                        inquiryMotorViewModel.pst_rate_banjir = 0;
                        inquiryMotorViewModel.stn_rate_banjir = 1;
                        inquiryMotorViewModel.nilai_prm_banjir = 0;
                        inquiryMotorViewModel.validitas = "A";
                        inquiryMotorViewModel.pst_rate_trs = 0;
                        inquiryMotorViewModel.stn_rate_trs = 1;
                        inquiryMotorViewModel.nilai_prm_trs = 0;
                        inquiryMotorViewModel.pst_rate_tjh = 0;
                        inquiryMotorViewModel.stn_rate_tjh = 1;
                        inquiryMotorViewModel.pst_rate_tjp = 0;
                        inquiryMotorViewModel.stn_rate_tjp = 1;
                        inquiryMotorViewModel.pst_rate_pap = 0;
                        inquiryMotorViewModel.stn_rate_pap = 1;
                        inquiryMotorViewModel.pst_rate_pad = 0;
                        inquiryMotorViewModel.stn_rate_pad = 1;
                        inquiryMotorViewModel.kd_endt = "I";

                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherMotor.cshtml", inquiryMotorViewModel);
                    }
                
                    Mapper.Map(motorResult, inquiryMotorViewModel);
                    inquiryMotorViewModel.kd_cb = inquiryMotorViewModel.kd_cb.Trim();
                    inquiryMotorViewModel.kd_cob = inquiryMotorViewModel.kd_cob.Trim();
                    inquiryMotorViewModel.kd_scob = inquiryMotorViewModel.kd_scob.Trim();
                    inquiryMotorViewModel.kd_guna = inquiryMotorViewModel.kd_guna.Trim();
                    inquiryMotorViewModel.validitas = inquiryMotorViewModel.validitas.Trim();
                    inquiryMotorViewModel.kd_jns_ptg = inquiryMotorViewModel.kd_jns_ptg.Trim();
                    inquiryMotorViewModel.warna_kend = inquiryMotorViewModel.warna_kend?.Trim();
                    inquiryMotorViewModel.kd_wilayah = inquiryMotorViewModel.kd_wilayah?.Trim();
                    
                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherMotor.cshtml", inquiryMotorViewModel);
                case "_OtherFire":
                    var inquiryFireViewModel = new InquiryResikoOtherFireViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var fireCommand = Mapper.Map<GetInquiryOtherFireQuery>(model);
                    fireCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var fireResult = await Mediator.Send(fireCommand);

                    if (fireResult == null)
                    {
                        inquiryFireViewModel.kd_penerangan = "1";
                        inquiryFireViewModel.kategori_gd = "E";

                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherFire.cshtml", inquiryFireViewModel);
                    }
                
                    Mapper.Map(fireResult, inquiryFireViewModel);
                    inquiryFireViewModel.kd_cb = inquiryFireViewModel.kd_cb.Trim();
                    inquiryFireViewModel.kd_cob = inquiryFireViewModel.kd_cob.Trim();
                    inquiryFireViewModel.kd_scob = inquiryFireViewModel.kd_scob.Trim();
                    
                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherFire.cshtml", inquiryFireViewModel);
                case "_OtherHull":
                    var inquiryHullViewModel = new InquiryResikoOtherHullViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var hullCommand = Mapper.Map<GetInquiryOtherHullQuery>(model);
                    hullCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var hullResult = await Mediator.Send(hullCommand);

                    if (hullResult == null)
                    {
                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHull.cshtml", inquiryHullViewModel);
                    }
                
                    Mapper.Map(hullResult, inquiryHullViewModel);
                    inquiryHullViewModel.kd_cb = inquiryHullViewModel.kd_cb.Trim();
                    inquiryHullViewModel.kd_cob = inquiryHullViewModel.kd_cob.Trim();
                    inquiryHullViewModel.kd_scob = inquiryHullViewModel.kd_scob.Trim();
                    inquiryHullViewModel.merk_kapal = inquiryHullViewModel.merk_kapal?.Trim();
                    inquiryHullViewModel.kd_kapal = inquiryHullViewModel.kd_kapal.Trim();
                    
                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHull.cshtml", inquiryHullViewModel);
                case "_OtherPA":
                    var inquiryPaViewModel = new InquiryResikoOtherPAViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var paCommand = Mapper.Map<GetInquiryOtherPAQuery>(model);
                    paCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var paResult = await Mediator.Send(paCommand);

                    if (paResult == null)
                    {
                        inquiryPaViewModel.thn_akh = "1";
                        inquiryPaViewModel.nilai_harga_ptg = 0;
                        inquiryPaViewModel.pst_rate_std = 0;
                        inquiryPaViewModel.pst_rate_bjr = 0;
                        inquiryPaViewModel.pst_rate_tl = 0;
                        inquiryPaViewModel.pst_rate_gb = 0;
                        inquiryPaViewModel.nilai_prm_std = 0;
                        inquiryPaViewModel.nilai_prm_bjr = 0;
                        inquiryPaViewModel.nilai_prm_tl = 0;
                        inquiryPaViewModel.nilai_bia_adm = 0;
                        inquiryPaViewModel.nilai_prm_btn = 0;
                        inquiryPaViewModel.flag_std = "2";
                        inquiryPaViewModel.flag_bjr = "2";
                        inquiryPaViewModel.flag_tl = "1";
                        inquiryPaViewModel.flag_gb = "1";
                        inquiryPaViewModel.pst_rate_phk = 0;
                        inquiryPaViewModel.nilai_prm_phk = 0;
                        inquiryPaViewModel.nilai_bia_mat = 0;
                        inquiryPaViewModel.nilai_ptg_std = 0;
                        inquiryPaViewModel.nilai_ptg_bjr = 0;
                        inquiryPaViewModel.nilai_ptg_tl = 0;
                        inquiryPaViewModel.nilai_ptg_gb = 0;
                        inquiryPaViewModel.nilai_ptg_hh = 0;
                        inquiryPaViewModel.stn_rate_std = 10;
                        inquiryPaViewModel.stn_rate_bjr = 10;
                        inquiryPaViewModel.stn_rate_gb = 10;
                        inquiryPaViewModel.stn_rate_tl = 10;
                        inquiryPaViewModel.stn_rate_phk = 0;
                        inquiryPaViewModel.kd_grp_asj = "5";
                        inquiryPaViewModel.kd_jns_kr = "PA";
                        inquiryPaViewModel.nilai_prm_gb = 0;
                        inquiryPaViewModel.nilai_ptg_phk = 0;
                        inquiryPaViewModel.tgl_lahir = DateTime.Now;
                        inquiryPaViewModel.tgl_real = DateTime.Now;
                        inquiryPaViewModel.tgl_akh_ptg = DateTime.Now;
                        inquiryPaViewModel.tgl_mul_ptg = DateTime.Now;
                        inquiryPaViewModel.tgl_input = DateTime.Now;

                        return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherPA.cshtml", inquiryPaViewModel);
                    }
                
                    Mapper.Map(paResult, inquiryPaViewModel);
                    inquiryPaViewModel.kd_cb = inquiryPaViewModel.kd_cb.Trim();
                    inquiryPaViewModel.kd_cob = inquiryPaViewModel.kd_cob.Trim();
                    inquiryPaViewModel.kd_scob = inquiryPaViewModel.kd_scob.Trim();

                    return View("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherPA.cshtml", inquiryPaViewModel);
                case "_OtherHoleInOne":
                    var inquiryHoleInOneViewModel = new InquiryResikoOtherHoleInOneViewModel()
                    {
                        kd_cb = model.kd_cb.Trim(),
                        kd_cob = model.kd_cob.Trim(),
                        kd_scob = model.kd_scob.Trim(),
                        kd_thn = model.kd_thn.Trim(),
                        no_updt = model.no_updt,
                        no_pol = model.no_pol
                    };
                    
                    var holeInOneCommand = Mapper.Map<GetInquiryOtherHoleInOneQuery>(model);
                    holeInOneCommand.DatabaseName = Request.Cookies["DatabaseValue"];
                    var holeInOneResult = await Mediator.Send(holeInOneCommand);

                    if (holeInOneResult == null)
                    {
                        return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHoleInOne.cshtml", inquiryHoleInOneViewModel);
                    }
                
                    Mapper.Map(holeInOneResult, inquiryHoleInOneViewModel);
                    inquiryHoleInOneViewModel.kd_cb = inquiryHoleInOneViewModel.kd_cb.Trim();
                    inquiryHoleInOneViewModel.kd_cob = inquiryHoleInOneViewModel.kd_cob.Trim();
                    inquiryHoleInOneViewModel.kd_scob = inquiryHoleInOneViewModel.kd_scob.Trim();

                    return PartialView("~/Modules/Inquiry/Components/InquiryOther/_InquiryOtherHoleInOne.cshtml", inquiryHoleInOneViewModel);
                default:
                    return View("~/Modules/Inquiry/Views/EmptyWithMessage.cshtml");
            }
        }
    }
}