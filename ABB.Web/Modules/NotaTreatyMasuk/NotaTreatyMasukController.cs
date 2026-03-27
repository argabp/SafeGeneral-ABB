using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.NotaTreatyMasuks.Commands;
using ABB.Application.NotaTreatyMasuks.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaTreatyMasuk.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaTreatyMasuk
{
    public class NotaTreatyMasukController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetNotaTreatyMasuks(GridRequest grid)
        {
            var result = await Mediator.Send(new GetNotaTreatyMasuksQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetTreatyMasuks(GridRequest grid, string kd_cb, string kd_jns_sor)
        {
            var result = await Mediator.Send(new GetTreatyMasuksQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new NotaTreatyMasukViewModel()
            {
                flag_closing = "N",
                kuartal_tr = 5,
                nilai_prm = 0,
                nilai_kms = 0,
                nilai_kl = 0,
                tgl_input = DateTime.Now
            });
        }
        
        public IActionResult TreatyMasuk()
        {
            return View();
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_msk, string kd_mtu, string no_tr)
        {
            var command = new GetNotaTreatyMasukQuery()
            {
                kd_cb = kd_cb,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                kd_jns_sor = kd_jns_sor,
                kd_tty_msk = kd_tty_msk,
                kd_mtu = kd_mtu,
                no_tr = no_tr
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_jns_sor = result.kd_jns_sor.Trim();
            
            return View(Mapper.Map<NotaTreatyMasukViewModel>(result));
        }
        
        public async Task<IActionResult> Nota(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_msk, string kd_mtu, string no_tr)
        {
            var command = new GetNotaQuery()
            {
                kd_cb = kd_cb,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                kd_jns_sor = kd_jns_sor,
                kd_tty_msk = kd_tty_msk,
                kd_mtu = kd_mtu,
                no_tr = no_tr
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_jns_sor = result.kd_jns_sor.Trim();
            result.kd_rk_sor = result.kd_rk_sor.Trim();
            result.kd_mtu = result.kd_mtu.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            
            return View(Mapper.Map<NotaViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNotaTreatyMasuk([FromBody] NotaTreatyMasukViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaTreatyMasukCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveNota([FromBody] NotaViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteNotaTreatyMasuk(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_msk, string kd_mtu, string no_tr)
        {
            try
            {
                var command = new DeleteNotaTreatyMasukCommand()
                {
                    kd_cb = kd_cb,
                    kd_thn = kd_thn,
                    kd_bln = kd_bln,
                    kd_jns_sor = kd_jns_sor,
                    kd_tty_msk = kd_tty_msk,
                    kd_mtu = kd_mtu,
                    no_tr = no_tr
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

        public async Task<JsonResult> GetMataUang()
        {
            var cobs = await Mediator.Send(new GetMataUangPSTQuery());
             
            return Json(cobs);
        }

        public async Task<JsonResult> GetUsers()
        {
            var cobs = await Mediator.Send(new GetUsersQuery());
             
            return Json(cobs);
        }
        
        public JsonResult GetKuartalTransaksi()
        {
            var dropdownOptionDtos = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "I (Pertama)", Value = "1" },
                new DropdownOptionDto() { Text = "II (Kedua)", Value = "2" },
                new DropdownOptionDto() { Text = "III (Ketiga)", Value = "3" },
                new DropdownOptionDto() { Text = "IV (Keempat)", Value = "4" },
                new DropdownOptionDto() { Text = "Nota Premi/Klaim", Value = "5" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetJenisSor()
        {
            var result = await Mediator.Send(new GetJenisSorPSTQuery());

            result = result.Where(w => w.Value != "XOL").ToList();
            
            return Json(result);
        }

        public async Task<JsonResult> GetCOB()
        {
            var cobs = await Mediator.Send(new GetCobPSTQuery());
            
            return Json(cobs);
        }
        
        public async Task<JsonResult> GetGroupPas()
        {
            var result = await Mediator.Send(new GetKodeTertujuPSTQuery());

            return Json(result);
        }
        
        public async Task<JsonResult> GetRekananPas(string kd_grp_pas, string kd_cb)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupAndCabangPSTQuery()
            {
                kd_grp_rk = kd_grp_pas,
                kd_cb = kd_cb
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetRekananSor(string kd_jns_sor, string kd_cb)
        {
            var result = await Mediator.Send(new GetTreatyMasukPSTQuery()
            {
                kd_jns_sor = kd_jns_sor,
                kd_cb = kd_cb
            });

            return Json(result);
        }
    }
}