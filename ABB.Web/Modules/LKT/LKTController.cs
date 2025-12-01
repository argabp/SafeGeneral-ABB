using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Queries;
using ABB.Application.LKTs.Commands;
using ABB.Application.LKTs.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LKT.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LKT
{
    public class LKTController : AuthorizedBaseController
    {
        private static List<RekananDto> _rekanans;
        private static List<DropdownOptionDto> _cabangs;
        private static List<DropdownOptionDto> _cobs;
        private static List<SCOBDto> _scobs;
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            _rekanans = await Mediator.Send(new GetRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty
            });
            _cabangs =  await Mediator.Send(new GetCabangQuery()
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
            
            return View();
        }
        
        public async Task<ActionResult> GetLKTs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLKTsQuery()
            {
                SearchKeyword = searchkeyword, 
                DatabaseName = Request.Cookies["DatabaseValue"],
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });

            var counter = 1;
            foreach (var data in ds)
            {
                data.Id = counter;
                data.nomor_register = "K." + data.kd_cb.Trim() + "." + data.kd_scob.Trim() 
                                      + "." + data.kd_thn.Trim() + "." + data.no_kl.Trim();
                counter++;
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }
        
        public async Task<IActionResult> Edit(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_kl, Int16 no_mts, string st_tipe_dla, Int16 no_dla)
        {
            var command = new GetLKTQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                st_tipe_dla = st_tipe_dla,
                no_dla = no_dla
            };
            
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            
            return View(Mapper.Map<LKTViewModel>(result));
        }
        
        public async Task<IActionResult> View(string kd_cb, string kd_cob, string kd_scob,
            string kd_thn, string no_kl, Int16 no_mts, string st_tipe_dla, Int16 no_dla)
        {
            var command = new GetLKTQuery()
            {
                kd_cb = kd_cb,
                kd_cob = kd_cob,
                kd_scob = kd_scob,
                kd_thn = kd_thn,
                no_kl = no_kl,
                no_mts = no_mts,
                st_tipe_dla = st_tipe_dla,
                no_dla = no_dla
            };
            
            command.DatabaseName = Request.Cookies["DatabaseValue"];
            
            var result = await Mediator.Send(command);

            result.kd_cb = result.kd_cb.Trim();
            result.kd_cob = result.kd_cob.Trim();
            result.kd_scob = result.kd_scob.Trim();
            result.kd_grp_pas = result.kd_grp_pas.Trim();
            result.kd_rk_pas = result.kd_rk_pas.Trim();
            
            return View(Mapper.Map<LKTViewModel>(result));
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveLKT([FromBody] LKTViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveLKTCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                
                var entity = await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan, Model = entity});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
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
        
        public JsonResult GetStatusTipeDLA()
        {
            var result = new List<DropdownOptionDto>()
            {
                new DropdownOptionDto() { Text = "Tertanggung", Value = "T" },
                new DropdownOptionDto() { Text = "Koasuransi", Value = "K" }
            };

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
        
        public JsonResult GetKodeRekananPas(string kd_grp_pas)
        {
            return Json(_rekanans.Where(w => w.kd_grp_rk == kd_grp_pas)
                .Select(rekanan => new DropdownOptionDto() { Text = rekanan.nm_rk, Value = rekanan.kd_rk })
                .ToList());
        }
    }
}