using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.RiskAndLossProfiles.Commands;
using ABB.Application.RiskAndLossProfiles.Queries;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.RiskAndLossProfile.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.RiskAndLossProfile
{
    public class RiskAndLossProfileController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            
            return View();
        }
        
        public async Task<ActionResult> GetRiskAndLossProfiles([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetRiskAndLossProfilesQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RiskAndLossProfileViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveRiskAndLossProfileCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Risk And Loss Profile"});
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
        public async Task<IActionResult> Delete(string kd_cob, int nomor)
        {
            try
            {
                var command = new DeleteRiskAndLossProfileCommand()
                {
                    kd_cob = kd_cob, nomor = nomor,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Risk And Loss Profile"});

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
            return PartialView(new RiskAndLossProfileViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cob, int nomor)
        {
            var riskAndLossProfile = await Mediator.Send(new GetRiskAndLossProfileQuery()
            {
                kd_cob = kd_cob,
                nomor = nomor,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            riskAndLossProfile.kd_cob = riskAndLossProfile.kd_cob.Trim();
            
            return PartialView(Mapper.Map<RiskAndLossProfileViewModel>(riskAndLossProfile));
        }
    }
}