using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.DokumenKlaims.Commands;
using ABB.Application.DokumenKlaims.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.DokumenKlaim.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.DokumenKlaim
{
    public class DokumenKlaimController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetDokumenKlaims([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetDokumenKlaimsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DokumenKlaimViewModel model)
        {
            try
            {

                var command = Mapper.Map<SaveDokumenKlaimCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Dokumen Klaim"});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return PartialView("Add", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteDokumenKlaim(string kd_cob, string kd_dok)
        {
            try
            {
                var command = new DeleteDokumenKlaimCommand()
                {
                    kd_cob = kd_cob, kd_dok = kd_dok,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Dokumen Klaim"});

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
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
            return PartialView(new DokumenKlaimViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cob, string kd_dok)
        {
            var dokumenKlaim = await Mediator.Send(new GetDokumenKlaimQuery()
            {
                kd_cob = kd_cob,
                kd_dok = kd_dok,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            dokumenKlaim.kd_cob = dokumenKlaim.kd_cob.Trim();
            
            return PartialView(Mapper.Map<DokumenKlaimViewModel>(dokumenKlaim));
        }
    }
}