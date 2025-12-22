using System;
using System.Threading.Tasks;
using ABB.Application.Cabangs.Commands;
using ABB.Application.Cabangs.Queries;
using ABB.Application.Common;
using ABB.Application.Common.Exceptions;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.Cabang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Cabang
{
    public class CabangController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<JsonResult> GetCabangs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetCabangsQuery()
            {
                SearchKeyword = searchkeyword,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Add()
        {
            var model = new CabangViewModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CabangViewModel model)
        {
            try
            {
                var command = Mapper.Map<AddCabangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                return PartialView(model);
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<IActionResult> Edit(string kd_cb)
        {
            var query = await Mediator.Send(new GetCabangQuery()
            {
                kd_cb = kd_cb,
                DatabaseName = Request.Cookies["DatabaseValue"]
            });
            var model = Mapper.Map<CabangViewModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CabangViewModel model)
        {
            try
            {
                var command = Mapper.Map<EditCabangCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string kd_cb)
        {
            try
            {
                await Mediator.Send(new DeleteCabangCommand() { kd_cb = kd_cb, 
                    DatabaseName = Request.Cookies["DatabaseValue"]});

                return Json(new { Result = true, Message = Constant.DataDisimpan });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }
    }
}