using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Obligees.Commands;
using ABB.Application.Obligees.Queries;
using ABB.Web.Extensions;
using ABB.Web.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Obligee.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Obligee
{
    public class ObligeeController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
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
            
            return View();
        }
        
        public async Task<ActionResult> GetObligees([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetObligeesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
                
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveObligee([FromBody] SaveObligeeViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveObligeeCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Obligee"});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            var view = string.IsNullOrWhiteSpace(model.kd_rk) ? "AddObligeeView" : "EditObligeeView";
            
            return PartialView(view, model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteObligee([FromBody] DeleteObligeeViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteObligeeCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Obligee"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailObligee([FromBody] SaveDetailObligeeViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailObligeeCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Detail Obligee"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailObligee([FromBody] DeleteObligeeViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailObligeeCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Obligee"});
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

        public async Task<JsonResult> GetKodeGroupObligee()
        {
            var result = await Mediator.Send(new GetKodeGroupObligeesQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKotaObligee()
        {
            var result = await Mediator.Send(new GetKotaObligeesQuery()
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
        public IActionResult AddObligeeView()
        {
            return PartialView(new SaveObligeeViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditObligeeView(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var detailObligee = await Mediator.Send(new GetObligeeQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<SaveObligeeViewModel>(detailObligee));
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailObligeeView(string kd_cb, string kd_grp_rk, string kd_rk, string flag_sic)
        {
            var detailDetailObligee = await Mediator.Send(new GetDetailObligeeQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            string view = flag_sic == "R" ? "EditDetailObligeeRetailView" : "EditDetailObligeeCorporateView";

            return PartialView(view, detailDetailObligee == null
                    ? new SaveDetailObligeeViewModel()
                    : Mapper.Map<SaveDetailObligeeViewModel>(detailDetailObligee));
        }
    }
}