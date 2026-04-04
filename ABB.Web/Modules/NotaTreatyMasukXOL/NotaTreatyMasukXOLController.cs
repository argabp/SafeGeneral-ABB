using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.NotaTreatyMasukXOLs.Commands;
using ABB.Application.NotaTreatyMasukXOLs.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.NotaTreatyMasukXOL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.NotaTreatyMasukXOL
{
    public class NotaTreatyMasukXOLController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetNotaTreatyMasukXOLs(GridRequest grid)
        {
            var result = await Mediator.Send(new GetNotaTreatyMasukXOLsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetTreatyMasuks(GridRequest grid, string kd_cb, string kd_jns_sor)
        {
            var result = await Mediator.Send(new GetTreatyMasukXOLsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new NotaTreatyMasukXOLViewModel()
            {
                flag_closing = "N",
                kuartal_tr = 5,
                nilai_prm = 0,
                nilai_kms = 0,
                nilai_kl = 0,
                tgl_input = DateTime.Now,
                kd_jns_sor = "XOL"
            });
        }
        
        public IActionResult TreatyMasuk()
        {
            return View();
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_msk, string kd_mtu, string no_tr)
        {
            var command = new GetNotaTreatyMasukXOLQuery()
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
            
            return View(Mapper.Map<NotaTreatyMasukXOLViewModel>(result));
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
        public async Task<IActionResult> SaveNotaTreatyMasukXOL([FromBody] NotaTreatyMasukXOLViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveNotaTreatyMasukXOLCommand>(model);
                
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
        public async Task<IActionResult> DeleteNotaTreatyMasukXOL(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_msk, string kd_mtu, string no_tr)
        {
            try
            {
                var command = new DeleteNotaTreatyMasukXOLCommand()
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
                new DropdownOptionDto() { Text = "Nota Premi", Value = "5" },
                new DropdownOptionDto() { Text = "Nota Klaim", Value = "6" }
            };

            return Json(dropdownOptionDtos);
        }
        
        public async Task<JsonResult> GetJenisSor()
        {
            var result = await Mediator.Send(new GetJenisSorPSTQuery());

            result = result.Where(w => w.Value == "XOL").ToList();
            
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