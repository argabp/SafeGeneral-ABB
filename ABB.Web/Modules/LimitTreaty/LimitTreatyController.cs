using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.LimitTreaties.Commands;
using ABB.Application.LimitTreaties.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.LimitTreaty.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.LimitTreaty
{
    public class LimitTreatyController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetLimitTreaties([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetLimitTreatiesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] LimitTreatyViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveLimitTreatyCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Limit Treaty"});
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
        public async Task<IActionResult> Delete(string kd_cob, string kd_tol)
        {
            try
            {
                var command = new DeleteLimitTreatyCommand()
                {
                    kd_cob = kd_cob, kd_tol = kd_tol,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Limit Treaty"});

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
            return PartialView(new LimitTreatyViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cob, string kd_tol)
        {
            var limitTreaty = await Mediator.Send(new GetLimitTreatyQuery()
            {
                kd_cob = kd_cob,
                kd_tol = kd_tol,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            limitTreaty.kd_cob = limitTreaty.kd_cob.Trim();
            
            return PartialView(Mapper.Map<LimitTreatyViewModel>(limitTreaty));
        }
    }
}