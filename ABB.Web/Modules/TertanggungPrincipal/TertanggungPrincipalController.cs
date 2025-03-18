using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.Rekanans.Commands;
using ABB.Application.Rekanans.Queries;
using ABB.Application.TertanggungPrincipals.Commands;
using ABB.Application.TertanggungPrincipals.Queries;
using ABB.Web.Extensions;
using ABB.Web.Models;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Rekanan.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.TertanggungPrincipal
{
    public class TertanggungPrincipalController : AuthorizedBaseController
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
        
        public async Task<ActionResult> GetRekanans([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetTertanggungPrincipalsQuery()
            {
                SearchKeyword = searchkeyword, 
                ModuleType = Request.Cookies["Module"] ?? string.Empty,
                DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty,
                KodeCabang = Request.Cookies["UserCabang"] ?? string.Empty
            });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> SaveRekanan([FromBody] SaveRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"] ?? string.Empty;
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Tertanggung & Principal"});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            var view = string.IsNullOrWhiteSpace(model.kd_rk) ? "AddRekananView" : "EditRekananView";
            
            return PartialView(view, model);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteRekanan([FromBody] DeleteRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Tertanggung & Principal"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SaveDetailRekanan([FromBody] SaveDetailRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveDetailTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Detail Tertanggung & Principal"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteDetailRekanan([FromBody] DeleteRekananViewModel model)
        {
            try
            {
                var command = Mapper.Map<DeleteDetailTertanggungPrincipalCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Detail Tertanggung & Principal"});
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

        public async Task<JsonResult> GetKodeGroupRekanan()
        {
            var result = await Mediator.Send(new GetKodeGroupRekanansQuery()
            {
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(result);
        }

        public async Task<JsonResult> GetKotaRekanan()
        {
            var result = await Mediator.Send(new GetKotaRekanansQuery()
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
        public IActionResult AddRekananView()
        {
            return PartialView(new SaveRekananViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditRekananView(string kd_cb, string kd_grp_rk, string kd_rk)
        {
            var detailRekanan = await Mediator.Send(new GetRekananQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<SaveRekananViewModel>(detailRekanan));
        }

        [HttpGet]
        public async Task<IActionResult> EditDetailRekananView(string kd_cb, string kd_grp_rk, string kd_rk, string flag_sic)
        {
            var detailDetailRekanan = await Mediator.Send(new GetDetailRekananQuery()
            {
                kd_cb = kd_cb,
                kd_grp_rk = kd_grp_rk,
                kd_rk = kd_rk,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            string view = flag_sic == "R" ? "EditDetailRekananRetailView" : "EditDetailRekananCorporateView";

            return PartialView(view, detailDetailRekanan == null
                    ? new SaveDetailRekananViewModel()
                    : Mapper.Map<SaveDetailRekananViewModel>(detailDetailRekanan));
        }
    }
}