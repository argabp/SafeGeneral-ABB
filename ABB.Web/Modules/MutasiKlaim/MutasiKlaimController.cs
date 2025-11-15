using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.BiayaMaterais.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.MutasiKlaims.Commands;
using ABB.Application.MutasiKlaims.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.MutasiKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.MutasiKlaim
{
    public class MutasiKlaimController : AuthorizedBaseController
    {
        private static List<RekananDto> _rekanans;
        private static List<DropdownOptionDto> _cabangs;
        private static List<DropdownOptionDto> _cobs;
        private static List<SCOBDto> _scobs;
        private static List<DropdownOptionDto> _mataUang;
        private static List<DropdownOptionDto> _tipeMutasi;
        private static List<DropdownOptionDto> _users;
        
        public async Task<ActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;

            _rekanans = await Mediator.Send(new GetRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            
            _cabangs = await Mediator.Send(new GetCabangQuery()
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
            
            _mataUang = await Mediator.Send(new GetMataUangQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            _tipeMutasi = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PLA", Value = "P" },
                new DropdownOptionDto() { Text = "DLA", Value = "D" },
                new DropdownOptionDto() { Text = "Beban", Value = "B" },
                new DropdownOptionDto() { Text = "Recovery", Value = "R" }
            };
            
            _users = await Mediator.Send(new GetUsersQuery());
            
            return View();
        }
        
        public async Task<ActionResult> GetMutasiKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetMutasiKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            var counter = 0;
            foreach (var data in ds)
            {
                counter++;
                data.register_klaim = data.kd_cb.Trim() + "." + data.kd_cob.Trim() +
                                      data.kd_scob.Trim() + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                data.id_detail_grid = data.kd_cb.Trim() + data.kd_cob.Trim() +
                                      data.kd_scob.Trim() + data.kd_thn.Trim() + data.no_kl.Trim();
                data.Id = counter;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetMutasiKlaimDetails([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string kd_cob, string kd_scob, string kd_thn, string no_kl)
        {
            var ds = await Mediator.Send(new GetMutasiKlaimDetailsQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var counter = 0;
            foreach (var data in ds)
            {
                counter++;
                data.Id = counter;
                data.nm_kd_mtu = _mataUang.FirstOrDefault(w => w.Value.Trim() == data.kd_mtu.Trim())?.Text ?? string.Empty;
                data.nm_tipe_mts = _tipeMutasi.FirstOrDefault(w => w.Value.Trim() == data.tipe_mts.Trim())?.Text ?? string.Empty;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetMutasiKlaimObyeks([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string kd_cob, string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var ds = await Mediator.Send(new GetMutasiKlaimObyeksQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var counter = 0;
            foreach (var data in ds)
            {
                counter++;
                data.Id = counter;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetMutasiKlaimBebans([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string kd_cob, string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var ds = await Mediator.Send(new GetMutasiKlaimBebansQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var counter = 0;
            foreach (var data in ds)
            {
                counter++;
                data.Id = counter;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetMutasiKlaimAlokasis([DataSourceRequest] DataSourceRequest request, string kd_cb,
            string kd_cob, string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var ds = await Mediator.Send(new GetMutasiKlaimAlokasisQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            var groupRekanan = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };
            
            var counter = 0;
            foreach (var data in ds)
            {
                counter++;
                data.Id = counter;
                data.nm_grp_pas = groupRekanan.FirstOrDefault(w => w.Value == data.kd_grp_pas)?.Text;
                data.nm_rk_pas = _rekanans.FirstOrDefault(w => w.kd_rk == data.kd_rk_pas && w.kd_grp_rk == data.kd_grp_pas)?.nm_rk;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
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
        
        public JsonResult GetMataUang()
        {
            return Json(_mataUang);
        }
        
        public JsonResult GetTipeMutasi()
        {
            return Json(_tipeMutasi);
        }
        
        public async Task<IActionResult> View(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            var mutasiKlaim = await Mediator.Send(new GetMutasiKlaimQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            mutasiKlaim.kd_cb = mutasiKlaim.kd_cb.Trim();
            mutasiKlaim.kd_cob = mutasiKlaim.kd_cob.Trim();
            mutasiKlaim.kd_scob = mutasiKlaim.kd_scob.Trim();
            mutasiKlaim.validitas = mutasiKlaim.validitas.Trim();
            
            return PartialView(Mapper.Map<MutasiKlaimViewModel>(mutasiKlaim));
        }
        
        [HttpGet]
        public IActionResult Insert(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl)
        {
            return View(new InsertMutasiKlaimViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl
            });
        }
        
        [HttpGet]
        public IActionResult Alokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            return View(new MutasiKlaimAlokasiViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> EditAlokasi(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            string kd_grp_pas, string kd_rk_pas)
        {
            var command = new GetMutasiKlaimAlokasiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                kd_grp_pas = kd_grp_pas,
                kd_rk_pas = kd_rk_pas,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            var mutasiKlaimAlokasi = await Mediator.Send(command);

            var model = Mapper.Map<MutasiKlaimAlokasiViewModel>(mutasiKlaimAlokasi);
            model.IsEdit = true;
            
            return View("Alokasi", model);
        }
        
        [HttpGet]
        public IActionResult ObyekView(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            string tipe_mts, string kd_mtu)
        {
            if (tipe_mts == "B" || tipe_mts == "R")
            {
                return View("BebanView", new MutasiKlaimViewModel()
                {
                    kd_cb = kd_cb,
                    kd_cob = kd_cob,
                    kd_scob = kd_scob,
                    kd_thn = kd_thn,
                    no_kl = no_kl,
                    no_mts = no_mts,
                    tipe_mts = tipe_mts,
                    kd_mtu = kd_mtu
                });
            }
            
            return View(new MutasiKlaimViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts
            });
        }
        
        [HttpGet]
        public IActionResult Obyek(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts)
        {
            return View(new MutasiKlaimObyekViewModel()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> EditObyek(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            Int16 no_oby)
        {
            var command = new GetMutasiKlaimObyekQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                no_oby = no_oby,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            var mutasiKlaimAlokasi = await Mediator.Send(command);

            var model = Mapper.Map<MutasiKlaimObyekViewModel>(mutasiKlaimAlokasi);
            
            return View("Obyek", model);
        }
        
        [HttpGet]
        public IActionResult Beban(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            string tipe_mts, string kd_mtu)
        {
            return View(new MutasiKlaimBebanViewModel()
            {
                kd_cb = kd_cb.Trim(),
                kd_cob = kd_cob.Trim(),
                kd_scob = kd_scob.Trim(),
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                kd_mtu = kd_mtu.Trim(),
                Status = _tipeMutasi.FirstOrDefault(w => w.Value == tipe_mts)?.Text
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> EditBeban(string kd_cb, string kd_cob,
            string kd_scob, string kd_thn, string no_kl, Int16 no_mts,
            Int16 no_urut)
        {
            var command = new GetMutasiKlaimBebanQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                no_urut = no_urut,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };
            var mutasiKlaimAlokasi = await Mediator.Send(command);

            var model = Mapper.Map<MutasiKlaimBebanViewModel>(mutasiKlaimAlokasi);
            model.kd_cb = model.kd_cb.Trim();
            model.kd_cob = model.kd_cob.Trim();
            model.kd_scob = model.kd_scob.Trim();
            model.kd_mtu = model.kd_mtu.Trim();
            model.Status = _tipeMutasi.FirstOrDefault(w => w.Value == model.st_jns)?.Text;
            
            return View("Beban", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] InsertMutasiKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<InsertMutasiKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var result = await Mediator.Send(command);
                return Json(new { Result = "OK", result.Message});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveMutasiKlaimBeban([FromBody] MutasiKlaimBebanViewModel model)
        {
            try
            {
                model.st_jns = model.Status == "Beban" ? "B" : "R";
                var command = Mapper.Map<SaveMutasiKlaimBebanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveMutasiKlaimAlokasi([FromBody] MutasiKlaimAlokasiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveMutasiKlaimAlokasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveMutasiKlaimObyek([FromBody] MutasiKlaimObyekViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveMutasiKlaimObyekCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMutasiKlaim([FromBody] MutasiKlaimModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteMutasiKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Mutasi Klaim"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMutasiKlaimObyek([FromBody] MutasiKlaimObyekModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteMutasiKlaimObyekCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Mutasi Klaim Obyek"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMutasiKlaimBeban([FromBody] MutasiKlaimBebanModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteMutasiKlaimBebanCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Mutasi Klaim Beban"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteMutasiKlaimAlokasi([FromBody] MutasiKlaimAlokasiModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteMutasiKlaimAlokasiCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Mutasi Klaim Alokasi"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public JsonResult GetValiditas()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Klaim Normal", Value = "A" },
                new DropdownOptionDto() { Text = "Ex Gratia", Value = "B" },
                new DropdownOptionDto() { Text = "Salvage", Value = "C" },
                new DropdownOptionDto() { Text = "Subrogari", Value = "D" }
            };

            return Json(result);
        }
        
        public JsonResult GetUsers()
        {
            return Json(_users);
        }
        
        public JsonResult GetGroupRekanan()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "PAS / REAS", Value = "5" }
            };

            return Json(result);
        }
        
        public JsonResult GetRekanan(string kd_grp_pas)
        {
            return Json(_rekanans.Where(w => w.kd_grp_rk == kd_grp_pas)
                .Select(rekanan => new DropdownOptionDto() { Text = rekanan.nm_rk, Value = rekanan.kd_rk })
                .ToList());
        }

        [HttpGet]
        public async Task<JsonResult> GenerateNilaiRecovery(string kd_mtu, double nilai_jns_org)
        {
            var command = new GenerateNilaiRecoveryQuery()
            {
                kd_mtu = kd_mtu,
                nilai_org = nilai_jns_org,
                DatabaseName = Request.Cookies["DatabaseValue"]
            };

            var result = await Mediator.Send(command);

            return Json(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> ClosingMutasiKlaim([FromBody] ClosingMutasiKlaimViewModel model)
        {
            try
            {
                var command = Mapper.Map<ClosingMutasiKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                var result = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
    }
}