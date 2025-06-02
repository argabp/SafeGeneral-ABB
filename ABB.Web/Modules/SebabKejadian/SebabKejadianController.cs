using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.SebabKejadians.Commands;
using ABB.Application.SebabKejadians.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.SebabKejadian.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.SebabKejadian
{
    public class SebabKejadianController : AuthorizedBaseController
    {
        public ActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        
        public async Task<ActionResult> GetSebabKejadians([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetSebabKejadiansQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });

            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SebabKejadianViewModel model)
        {
            try
            {
                var command = Mapper.Map<SaveSebabKejadianCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Save Sebab Kejadian"});
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
        public async Task<IActionResult> DeleteSebabKejadian(string kd_cob, string kd_sebab)
        {
            try
            {
                var command = new DeleteSebabKejadianCommand()
                {
                    kd_cob = kd_cob, kd_sebab = kd_sebab,
                    DatabaseName = Request.Cookies["DatabaseValue"]
                };
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Delete Sebab Kejadian"});

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
            return PartialView(new SebabKejadianViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string kd_cob, string kd_sebab)
        {
            var sebabKejadian = await Mediator.Send(new GetSebabKejadianQuery()
            {
                kd_cob = kd_cob,
                kd_sebab = kd_sebab,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            
            return PartialView(Mapper.Map<SebabKejadianViewModel>(sebabKejadian));
        }
    }
}