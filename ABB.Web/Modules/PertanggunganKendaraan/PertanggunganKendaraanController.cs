using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.PertanggunganKendaraans.Commands;
using ABB.Application.PertanggunganKendaraans.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.PertanggunganKendaraan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.PertanggunganKendaraan
{
    public class PertanggunganKendaraanController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetPertanggunganKendaraans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetPertanggunganKendaraansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            } );
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<ActionResult> GetDetailPertanggunganKendaraans([DataSourceRequest] DataSourceRequest request, string kd_cob, string kd_scob, string kd_jns_ptg)
        {
            if (string.IsNullOrWhiteSpace(kd_cob) || string.IsNullOrWhiteSpace(kd_scob) || string.IsNullOrWhiteSpace(kd_jns_ptg))
                return Ok();

            var ds = await Mediator.Send(new GetDetailPertanggunganKendaraansQuery()
            {
                kd_cob = kd_cob,
                kd_jns_ptg = kd_jns_ptg,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            foreach (var data in ds)
            {
                data.text_stn_rate_tjh = data.stn_rate_tjh == 1 ? "%" : "%o";
                    
            }
            
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] PertanggunganKendaraanViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.kd_scob))
                {
                    var command = Mapper.Map<AddPertanggunganKendaraanCommand>(model);
                    command.DatabaseName = Request.Cookies["DatabaseValue"];
                    await Mediator.Send(command);
                    return Json(new { Result = "OK", Message = "Successfully Add Pertanggungan Kendaraan"});
                }
                else
                {
                    var command = Mapper.Map<EditPertanggunganKendaraanCommand>(model);  
                    command.DatabaseName = Request.Cookies["DatabaseValue"];
                    await Mediator.Send(command);
                    return Json(new { Result = "OK", Message = "Successfully Edit Pertanggungan Kendaraan"});
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(string kd_cob, string kd_scob, string kd_jns_ptg)
        {
            try
            {
                await Mediator.Send(new DeletePertanggunganKendaraanCommand()
                {
                    kd_scob = kd_scob,
                    kd_jns_ptg = kd_jns_ptg,
                    kd_cob = kd_cob,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });
                return Json(new { Result = "OK", Message = "Successfully Delete Pertanggungan Kendaraan"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetail([FromBody] DetailPertanggunganKendaraanViewModel model)
        {
            try
            {
                if (model.no_urut == 0)
                {
                    var command = Mapper.Map<AddDetailPertanggunganKendaraanCommand>(model);
                    command.DatabaseName = Request.Cookies["DatabaseValue"];
                    await Mediator.Send(command);
                    return Json(new { Result = "OK", Message = "Successfully Add Detail Pertanggungan Kendaraan"});
                }
                else
                {
                    var command = Mapper.Map<EditDetailPertanggunganKendaraanCommand>(model); 
                    command.DatabaseName = Request.Cookies["DatabaseValue"]; 
                    await Mediator.Send(command);
                    return Json(new { Result = "OK", Message = "Successfully Edit Detail Pertanggungan Kendaraan"});
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDetail(string kd_cob, string kd_scob, string kd_jns_ptg, short no_urut)
        {
            try
            {
                await Mediator.Send(new DeleteDetailPertanggunganKendaraanCommand()
                {
                    kd_scob = kd_scob,
                    kd_jns_ptg = kd_jns_ptg,
                    kd_cob = kd_cob,
                    no_urut = no_urut,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                });
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Pertanggungan Kendaraan"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
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

        public async Task<JsonResult> GetCOB()
        {
            var result = await Mediator.Send(new GetCobQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView(new PertanggunganKendaraanViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cob, string kd_scob, string kd_jns_ptg)
        {
            var detailPertanggunganKendaraan = await Mediator.Send(new GetPertanggunganKendaraanQuery()
            {
                kd_cob = kd_cob,
                kd_jns_ptg = kd_jns_ptg,
                kd_scob = kd_scob,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            detailPertanggunganKendaraan.desk = detailPertanggunganKendaraan.desk?.Trim();
            detailPertanggunganKendaraan.kd_cob = detailPertanggunganKendaraan.kd_cob.Trim();
            
            return PartialView(Mapper.Map<PertanggunganKendaraanViewModel>(detailPertanggunganKendaraan));
        }

        [HttpGet]
        public IActionResult AddDetail(string kd_cob, string kd_scob, string kd_jns_ptg)
        {
            return PartialView(new DetailPertanggunganKendaraanViewModel() { kd_cob = kd_cob, kd_jns_ptg = kd_jns_ptg, kd_scob = kd_scob});
        }

        [HttpGet]
        public async Task<IActionResult> EditDetail(string kd_cob, string kd_scob, string kd_jns_ptg, short no_urut)
        {
            var detailDetailPertanggunganKendaraan = await Mediator.Send(new GetDetailPertanggunganKendaraanQuery()
            {
                kd_cob = kd_cob,
                kd_jns_ptg = kd_jns_ptg,
                kd_scob = kd_scob,
                no_urut = no_urut,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<DetailPertanggunganKendaraanViewModel>(detailDetailPertanggunganKendaraan));
        }
    }
}