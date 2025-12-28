using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.Rekanans.Queries;
using ABB.Application.TertanggungPrincipals.Commands;
using ABB.Application.TertanggungPrincipals.Queries;
using ABB.Web.Extensions;
using ABB.Web.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Rekanan.Models;
using ABB.Web.Modules.TertanggungPrincipal.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using GetKodeGroupRekanansQuery = ABB.Application.TertanggungPrincipals.Queries.GetKodeGroupRekanansQuery;

namespace ABB.Web.Modules.TertanggungPrincipal
{
    public class TertanggungPrincipalController : AuthorizedBaseController
    {
        private static List<LookupDetailDto> _lookupDetails;
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            ViewBag.KodeCabang = Request.Cookies["UserCabang"];
            
            ViewBag.bentukflag =  new List<IInputGroupItem>()
            {
                new InputGroupItemModel ()
                {
                    Label = "Baru",
                    Enabled = true,
                    CssClass = "",
                    Encoded = false,
                    Value = "1",
                },
                new InputGroupItemModel ()
                {
                    Label = "Lama",
                    Enabled = true,
                    Encoded = false,
                    CssClass = "",
                    Value = "2"
                }
            };

            _lookupDetails = await Mediator.Send(new GetAllLookupDetailsQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            
            return View();
        }
        
        public async Task<ActionResult> GetRekanans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetTertanggungPrincipalsQuery()
            {
                SearchKeyword = searchkeyword, 
                ModuleType = Request.Cookies["Module"] ?? string.Empty,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveRekanan([FromBody] SaveRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty;
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            var view = string.IsNullOrWhiteSpace(model.kd_rk) ? "AddRekananView" : "EditRekananView";
            
            return PartialView(view, model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteRekanan([FromBody] DeleteRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailRekananRetail([FromBody] SaveDetailRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTertanggungPrincipalRetailCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailRekananRetailView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailRekananCorporate([FromBody] SaveDetailRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTertanggungPrincipalCorporateCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailRekananCorporateView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailRekananRetailFull([FromBody] SaveDetailRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTertanggungPrincipalRetailFullCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailRekananRetailFullView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailRekananCorporateFull([FromBody] SaveDetailRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTertanggungPrincipalCorporateFullCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailRekananCorporateFullView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailSlikRetail([FromBody] DetailSlikViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailSlikRetailCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailSlikRetailView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailSlikCorporate([FromBody] DetailSlikViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailSlikCorporateCommand>(model);
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
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView("EditDetailSlikCorporateView", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailRekanan([FromBody] DeleteRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailSlik([FromBody] DeleteRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailSlikCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetKodeCabang()
        {
            var result = await Mediator.Send(new GetKodeCabangsQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKodeGroupRekanan()
        {
            var result = await Mediator.Send(new GetKodeGroupRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKotaRekanan()
        {
            var result = await Mediator.Send(new GetKotaRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public JsonResult GetFlagSic()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Retail", Value = "R" },
                new DropdownOptionDto() { Text = "Corporate", Value = "C" }
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult AddRekananView()
        {
            return PartialView(new SaveRekananViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditRekananView(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var detailRekanan = await Mediator.Send(new GetRekananQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<SaveRekananViewModel>(detailRekanan));
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailRekananView(string kd_cb, string kd_grp_rk, string kd_rk, string flag_sic)
        {
            var detailDetailRekanan = await Mediator.Send(new GetDetailRekananQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            string view;
            if (kd_grp_rk.Trim() == "P")
            {
                view = flag_sic == "R" ? "EditDetailRekananRetailFullView" : "EditDetailRekananCorporateFullView";
            } else
            {
                view = flag_sic == "R" ? "EditDetailRekananRetailView" : "EditDetailRekananCorporateView";
            }

            return PartialView(view, detailDetailRekanan == null
                    ? new SaveDetailRekananViewModel()
                    : Mapper.Map<SaveDetailRekananViewModel>(detailDetailRekanan));
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailSlikView(string kd_cb, string kd_grp_rk, string kd_rk, string flag_sic)
        {
            var detailDetailRekanan = await Mediator.Send(new GetDetailSlikQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            string view = flag_sic == "R" ? "EditDetailSlikRetailView" : "EditDetailSlikCorporateView";

            return PartialView(view, detailDetailRekanan == null
                    ? new DetailSlikViewModel()
                    {
                        tgl_lap_keu_debitur = DateTime.Now,
                        tgl_akta_pendirian = DateTime.Now,
                        tgl_akta_berubah_takhir = DateTime.Now,
                        tgl_pemeringkatan = DateTime.Now,
                        tgl_lhr_pasangan = DateTime.Now
                    }
                    : Mapper.Map<DetailSlikViewModel>(detailDetailRekanan));
        }
        
        public JsonResult GetKelamin()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Laki-laki", Value = "1" },
                new DropdownOptionDto() { Text = "Perempuan", Value = "0" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeHubunganPelapor()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "005").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeGolonganDebitur()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "006").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeNegara()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "001").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodePekerjaan()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "002").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeBidangUsaha()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "003").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeSumberPenghasilan()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "004").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeBentukBadanUsaha()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "007").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeKabupatenKota()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "008").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
        
        public JsonResult GetKodeJabatan()
        {
            return Json(_lookupDetails.Where(w => w.kd_lookup == "009").Select(s => new DropdownOptionDto()
            {
                Text = s.nm_detail_lookup,
                Value = s.no_lookup.ToString()
            }).ToList());
        }
    }
}