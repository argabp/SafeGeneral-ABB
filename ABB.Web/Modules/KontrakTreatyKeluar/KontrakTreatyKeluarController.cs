using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.KontrakTreatyKeluars.Commands;
using ABB.Application.KontrakTreatyKeluars.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.KontrakTreatyKeluar.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.KontrakTreatyKeluar
{
    public class KontrakTreatyKeluarController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetKontrakTreatyKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetKontrakTreatyKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluars(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new KontrakTreatyKeluarParameterViewModel());
        }
        
        public IActionResult Edit(string kd_cb, string kd_jns_sor, string kd_tty_pps)
        {
            var viewModel = new KontrakTreatyKeluarParameterViewModel()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            };
            
            return View(viewModel);
        }
        
        public IActionResult AddDetail()
        {
            return View(new DetailKontrakTreatyKeluarViewModel()
            {
                pst_com = 0,
                pst_share = 0
            });
        }
        
        public IActionResult EditDetail(string kd_grp_pas, string kd_rk_pas, decimal pst_com, 
            decimal pst_share, string? kd_grp_sb_bis, string? kd_rk_sb_bis)
        {
            var data = new DetailKontrakTreatyKeluarViewModel()
            {
                kd_grp_pas = kd_grp_pas,
                kd_rk_pas = kd_rk_pas,
                pst_com = pst_com,
                pst_share = pst_share,
                kd_grp_sb_bis = kd_grp_sb_bis,
                kd_rk_sb_bis = kd_rk_sb_bis
            };
            
            return View(data);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveKontrakTreatyKeluar([FromBody] KontrakTreatyKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveKontrakTreatyKeluarCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteKontrakTreatyKeluar(string kd_cb, string kd_jns_sor, string kd_tty_pps)
        {
            try
            {
                var command = new DeleteKontrakTreatyKeluarCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public async Task<JsonResult> GetCabang()
        {
            var result = await Mediator.Send(new GetCabangPSTQuery());

            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
             
            return Json(cobs);
        }
        
        public JsonResult GetKodePas()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetKodeRekanan(string kd_grp, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp,
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetJenisSor()
        {
            var result = await Mediator.Send(new GetJenisSorPSTQuery());

            return Json(result);
        }

        public JsonResult GetSumberBisnis()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Broker", Value = "2" },
                new DropdownOptionDto() { Text = "-", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }

        public JsonResult GetFrekuwensiPelaporan()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Bulanan", Value = "B" },
                new DropdownOptionDto() { Text = "Triwulan", Value = "T" }
            };

            return Json(dropdownOptionDtos);
        }

        public JsonResult GetFaktorSOR()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Yes", Value = "Y" },
                new DropdownOptionDto() { Text = "No", Value = "N" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetNmTtyNpps(string kd_cob, string nm_jns_sor,
            decimal thn_tty_pps)
        {
            var result = await Mediator.Send(new GetNmTtyNppsQuery()
            {
                kd_cob = kd_cob,
                nm_jns_sor = nm_jns_sor,
                thn_tty_pps = thn_tty_pps
            });

            return Json(result);
        }

        #region SCOB
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarSCOBs(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarSCOBsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailKontrakTreatyKeluarSCOB([FromBody] DetailKontrakTreatyKeluarSCOBViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailKontrakTreatyKeluarSCOBCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }

        public IActionResult AddDetailKontrakTreatyKeluarSCOB()
        {
            return View(new DetailKontrakTreatyKeluarSCOBViewModel());
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailKontrakTreatyKeluarSCOB(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_cob, string kd_scob)
        {
            try
            {
                var command = new DeleteDetailKontrakTreatyKeluarSCOBCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob
                };
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBPSTQuery()
            {
                kd_cob = kd_cob
            });

            return Json(result);
        }

        #endregion

        #region Coverage
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarCoverages(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarCoveragesQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailKontrakTreatyKeluarCoverage([FromBody] DetailKontrakTreatyKeluarCoverageViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailKontrakTreatyKeluarCoverageCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }

        public IActionResult AddDetailKontrakTreatyKeluarCoverage()
        {
            return View(new DetailKontrakTreatyKeluarCoverageViewModel()
            {
                pst_kms_reas = 0
            });
        }
        
        public async Task<IActionResult> EditDetailKontrakTreatyKeluarCoverage(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_cvrg)
        {
            var command = new GetDetailKontrakTreatyKeluarCoverageQuery()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps,
                kd_cvrg = kd_cvrg
            };
            
            var result = await Mediator.Send(command);
            result.kd_cvrg = result.kd_cvrg.Trim();
            
            return View(Mapper.Map<DetailKontrakTreatyKeluarCoverageViewModel>(result));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailKontrakTreatyKeluarCoverage(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_cvrg)
        {
            try
            {
                var command = new DeleteDetailKontrakTreatyKeluarCoverageCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps,
                    kd_cvrg = kd_cvrg
                };
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetCoverage()
        {
            var result = await Mediator.Send(new GetCoveragePSTQuery());

            return Json(result);
        }

        #endregion

        #region Exclude
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarExcludes(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarExcludesQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailKontrakTreatyKeluarExclude([FromBody] DetailKontrakTreatyKeluarExcludeViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailKontrakTreatyKeluarExcludeCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }

        public IActionResult AddDetailKontrakTreatyKeluarExclude()
        {
            return View(new DetailKontrakTreatyKeluarExcludeViewModel());
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailKontrakTreatyKeluarExclude(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_okup)
        {
            try
            {
                var command = new DeleteDetailKontrakTreatyKeluarExcludeCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps,
                    kd_okup = kd_okup
                };
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> GetOkupasi()
        {
            var result = await Mediator.Send(new GetOkupasiPSTQuery());

            return Json(result);
        }

        #endregion

        #region Koasuransi
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarKoasuransis(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarKoasuransisQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailKontrakTreatyKeluarKoasuransi([FromBody] DetailKontrakTreatyKeluarKoasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailKontrakTreatyKeluarKoasuransiCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }

        public IActionResult AddDetailKontrakTreatyKeluarKoasuransi()
        {
            return View(new DetailKontrakTreatyKeluarKoasuransiViewModel()
            {
                pst_bts_koas = 0,
                pst_share_mul = 0,
                pst_share_akh = 0
            });
        }
        
        public async Task<IActionResult> EditDetailKontrakTreatyKeluarKoasuransi(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, int no_urut)
        {
            var command = new GetDetailKontrakTreatyKeluarKoasuransiQuery()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps,
                no_urut = no_urut
            };
            
            var result = await Mediator.Send(command);
            
            return View(Mapper.Map<DetailKontrakTreatyKeluarKoasuransiViewModel>(result));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailKontrakTreatyKeluarKoasuransi(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, int no_urut)
        {
            try
            {
                var command = new DeleteDetailKontrakTreatyKeluarKoasuransiCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps,
                    no_urut = no_urut
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

        #region Table Of Limit
        
        public async Task<ActionResult> GetDetailKontrakTreatyKeluarTableOfLimits(GridRequest grid, string kd_cb,
            string kd_jns_sor, string kd_tty_pps)
        {
            var result = await Mediator.Send(new GetDetailKontrakTreatyKeluarTableOfLimitsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps
            });
            
            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailKontrakTreatyKeluarTableOfLimit([FromBody] DetailKontrakTreatyKeluarTableOfLimitViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailKontrakTreatyKeluarTableOfLimitCommand>(model);
                
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

            return PartialView(string.IsNullOrWhiteSpace(model.kd_tty_pps) ? "Add" : "Edit" , model);
        }

        public IActionResult AddDetailKontrakTreatyKeluarTableOfLimit()
        {
            return View(new DetailKontrakTreatyKeluarTableOfLimitViewModel());
        }
        
        public async Task<IActionResult> EditDetailKontrakTreatyKeluarTableOfLimit(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_okup, string category_rsk, string kd_kls_konstr)
        {
            var command = new GetDetailKontrakTreatyKeluarTableOfLimitQuery()
            {
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor,
                kd_tty_pps = kd_tty_pps,
                kd_okup = kd_okup,
                category_rsk = category_rsk,
                kd_kls_konstr = kd_kls_konstr
            };
            
            var result = await Mediator.Send(command);
            result.kd_okup = result.kd_okup.Trim();
            result.category_rsk = result.category_rsk.Trim();
            result.kd_kls_konstr = result.kd_kls_konstr.Trim();
            
            return View(Mapper.Map<DetailKontrakTreatyKeluarTableOfLimitViewModel>(result));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetailKontrakTreatyKeluarTableOfLimit(string kd_cb, string kd_jns_sor, 
            string kd_tty_pps, string kd_okup, string category_rsk, string kd_kls_konstr)
        {
            try
            {
                var command = new DeleteDetailKontrakTreatyKeluarTableOfLimitCommand()
                {
                    kd_cb = kd_cb, 
                    kd_jns_sor = kd_jns_sor, 
                    kd_tty_pps = kd_tty_pps,
                    kd_okup = kd_okup,
                    category_rsk = category_rsk,
                    kd_kls_konstr = kd_kls_konstr
                };
                
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetKategoriResiko()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "I-LOW", Value = "1" },
                new DropdownOptionDto() { Text = "II-MEDIUM", Value = "2" },
                new DropdownOptionDto() { Text = "III-HIGHT", Value = "3" }
            };

            return Json(dropdownOptionDtos);
        }

        public async Task<JsonResult> GetKelasKonstruksi()
        {
            var result = await Mediator.Send(new GetKelasKonstruksiPSTQuery());

            return Json(result);
        }

        #endregion
    }
}