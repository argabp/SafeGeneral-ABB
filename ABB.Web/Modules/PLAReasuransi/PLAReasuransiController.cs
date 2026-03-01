using System;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Grids.Models;
using ABB.Application.Common.Queries;
using ABB.Application.PLAReasuransis.Commands;
using ABB.Application.PLAReasuransis.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PLAReasuransi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PLAReasuransi
{
    public class PLAReasuransiController : AuthorizedBaseController
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetPLAReasuransis(GridRequest grid)
        {
            var result = await Mediator.Send(new GetPLAReasuransisQuery()
            {
                Grid = grid
            });
            
            return Json(result);
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_kl, Int16 no_mts, Int16 no_pla)
        {
            var command = new GetPLAReasuransiQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                no_pla = no_pla
            };
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            
            return View(Mapper.Map<PLAReasuransiViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SavePLAReasuransi([FromBody] PLAReasuransiViewModel model)
        {
            try
            {
                var command = Mapper.Map<SavePLAReasuransiCommand>(model);
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
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

        public async Task<JsonResult> GetSCOB(string kd_cob)
        {
            var result = await Mediator.Send(new GetSCOBPSTQuery()
            {
                kd_cob = kd_cob
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodePas()
        {
            var result = await Mediator.Send(new GetKodeTertujuQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }
        
        public async Task<JsonResult> GetKodeRekananPas(string kd_grp_pas)
        {
            var result = await Mediator.Send(new GetRekanansByKodeGroupQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"],
                kd_grp_rk = kd_grp_pas
            });

            return Json(result);
        }
    }
}