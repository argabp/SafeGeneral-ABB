using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Common.Queries;
using ABB.Application.KapasitasCabangs.Queries;
using ABB.Application.PolisInduks.Commands;
using ABB.Application.PolisInduks.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PolisInduk.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PolisInduk
{
    public class PolisIndukController : AuthorizedBaseController
    {
        private static List<RekananDto> _rekanans;

        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];

            _rekanans = await Mediator.Send(new GetRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            
            return View();
        }
        
        public async Task<ActionResult> GetPolisInduks([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetPolisInduksQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PolisIndukViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddPolisIndukCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add Polis Induk"});
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
        public async Task<IActionResult> Edit([FromBody] PolisIndukViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditPolisIndukCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit Polis Induk"});
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
        public async Task<IActionResult> Delete(string no_pol_induk)
        {
            try
            {
                var command = new DeletePolisIndukCommand()
                {
                    no_pol_induk = no_pol_induk,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Polis Induk"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
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

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new PolisIndukViewModel();
            viewModel.kd_grp_ttg = "9";
            viewModel.kd_grp_brk = "2";
            viewModel.st_pas = "O";
            viewModel.kd_grp_pas = "5";
            viewModel.kd_grp_bank = "6";
            viewModel.kd_grp_sb_bis = "0";
            viewModel.pst_share_bgu = 100;
            viewModel.faktor_prd = 100;
            viewModel.kd_grp_mkt = "M";
            return PartialView(viewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string no_pol_induk)
        {
            var polisInduk = await Mediator.Send(new GetPolisIndukQuery()
            {
                no_pol_induk = no_pol_induk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            polisInduk.kd_cb = polisInduk.kd_cb.Trim();
            polisInduk.kd_scob = polisInduk.kd_scob.Trim();
            polisInduk.kd_cob = polisInduk.kd_cob.Trim();
            
            return PartialView(Mapper.Map<PolisIndukViewModel>(polisInduk));
        }
        
        public JsonResult GetSatuanRatePremi()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "%", Value = "1" },
                new DropdownOptionDto() { Text = "%o", Value = "10" }
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
        
        public JsonResult GetStatusPolis()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Leader (Sebagai Leader Koasuransi)", Value = "L" },
                new DropdownOptionDto() { Text = "Member (Sebagai Member Koasuransi)", Value = "M" },
                new DropdownOptionDto() { Text = "Transaksi Direct", Value = "O" }
            };

            return Json(result);
        }
        
        public JsonResult GetKodeBroker()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Ag Perorangan Lepas", Value = "0" },
                new DropdownOptionDto() { Text = "Ag Perorangan Kontrak", Value = "1" },
                new DropdownOptionDto() { Text = "Broker", Value = "2" },
                new DropdownOptionDto() { Text = "Ag Lembaga Lepas", Value = "3" }
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

        public async Task<JsonResult> GetMataUang()
        {
            var result = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

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
    }
}