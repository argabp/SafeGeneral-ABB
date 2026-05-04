using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.ProsesPremiXOLKeluars.Commands;
using ABB.Application.ProsesPremiXOLKeluars.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.ProsesPremiXOLKeluar.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.ProsesPremiXOLKeluar
{
    public class ProsesPremiXOLKeluarController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetProsesPremiXOLKeluars(GridRequest grid)
        {
            var result = await Mediator.Send(new GetProsesPremiXOLKeluarsQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<ActionResult> GetTreatyKeluars(GridRequest grid, string kd_cb, string kd_jns_sor)
        {
            var result = await Mediator.Send(new GetTreatyKeluarXOLsQuery()
            {
                Grid = grid,
                kd_cb = kd_cb,
                kd_jns_sor = kd_jns_sor
            });
            
            return Json(result);
        }
        
        public IActionResult Add()
        {
            return View(new ProsesPremiXOLKeluarViewModel()
            {
                flag_closing = "N",
                nilai_prm = 0,
                tgl_input = DateTime.Now,
                kd_cb = "PS10",
                kd_jns_sor = "XOL"
            });
        }
        
        public IActionResult TreatyKeluar()
        {
            return View();
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_npps, string kd_mtu, string no_tr)
        {
            var command = new GetProsesPremiXOLKeluarQuery()
            {
                kd_cb = kd_cb,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                kd_jns_sor = kd_jns_sor,
                kd_tty_npps = kd_tty_npps,
                kd_mtu = kd_mtu,
                no_tr = no_tr
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_jns_sor = result.kd_jns_sor.Trim();
            
            return View(Mapper.Map<ProsesPremiXOLKeluarViewModel>(result));
        }
        
        public async Task<IActionResult> View(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_npps, string kd_mtu, string no_tr)
        {
            var command = new GetProsesPremiXOLKeluarQuery()
            {
                kd_cb = kd_cb,
                kd_thn = kd_thn,
                kd_bln = kd_bln,
                kd_jns_sor = kd_jns_sor,
                kd_tty_npps = kd_tty_npps,
                kd_mtu = kd_mtu,
                no_tr = no_tr
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_jns_sor = result.kd_jns_sor.Trim();
            
            return View(Mapper.Map<ProsesPremiXOLKeluarViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveProsesPremiXOLKeluar([FromBody] ProsesPremiXOLKeluarViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveProsesPremiXOLKeluarCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteProsesPremiXOLKeluar(string kd_cb, string kd_thn, string kd_bln,
            string kd_jns_sor, string kd_tty_npps, string kd_mtu, string no_tr)
        {
            try
            {
                var command = new DeleteProsesPremiXOLKeluarCommand()
                {
                    kd_cb = kd_cb,
                    kd_thn = kd_thn,
                    kd_bln = kd_bln,
                    kd_jns_sor = kd_jns_sor,
                    kd_tty_npps = kd_tty_npps,
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

        public async Task<IActionResult> Proses([FromBody] ProsesPremiXOLKeluarModel model)
        {
            try
            {
                var command = Mapper.Map<ProsesPremiXOLKeluarCommand>(model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }

        public async Task<IActionResult> CancelProses([FromBody] ProsesPremiXOLKeluarModel model)
        {
            try
            {
                var command = Mapper.Map<CancelProsesPremiXOLKeluarCommand>(model);
                var result = await Mediator.Send(command);
                
                return Json(new { Result = "OK", Message = result.Item2 });
            }
            catch (Exception e)
            {
                return Json(new
                    { Result = "ERROR", Message = e.InnerException == null ? e.Message : e.InnerException.Message });
            }
        }
    }
}