using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Exceptions;
using ABB.Application.UserCabangs.Commands;
using ABB.Application.UserCabangs.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.UserCabang.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.UserCabang
{
    public class UserCabangController : AuthorizedBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }

        public async Task<JsonResult> GetUserCabangs([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var result = await Mediator.Send(new GetViewUserCabangQuery());
            return Json(result.ToDataSourceResult(request));
        }

        public IActionResult Add()
        {
            var model = new AddUserCabangModel();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddUserCabangModel model)
        {
            try
            {
                var command = new AddUserCabangCommand();
                command.userid = model.userid;
                foreach (var item in model.Cabangs)
                {
                    if (item.Cabang == null) continue;
                    
                    command.Cabangs.Add(item.Cabang.Value);
                }

                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Add User Cabang" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var models = await Mediator.Send(new GetUserCabangsQuery());
            var viewModel = new EditUserCabangModel();
            viewModel.userid = id;

            foreach (var model in models.Where(w => w.userid == id))
            {
                viewModel.Cabangs.Add(new CabangItem()
                {
                    userid = id,
                    Cabang = new DropdownOptionDto()
                    {
                        Text = model.nm_cb,
                        Value = model.kd_cb
                    }
                });
            }

            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditUserCabangModel model)
        {
            try
            {
                var command = new EditUserCabangCommand();
                command.userid = model.userid;
                foreach (var item in model.Cabangs)
                {
                    if (item.Cabang == null) continue;
                    
                    command.Cabangs.Add(item.Cabang.Value);
                }

                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit User Cabang" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
                return Json(new { Result = "ERROR", Message = ex.Errors });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteUserCabangCommand() { userid = id });
            return Json(new { Result = "OK", Message = "Successfully Delete User Cabang" });
        }

        public async Task<ActionResult> GetCabangs()
        {
            var ds = await Mediator.Send(new GetCabangQuery());
            return Json(ds);
        }
        
        public async Task<ActionResult> GetUsers()
        {
            var ds = await Mediator.Send(new GetUserQuery());
            return Json(ds);
        }
    }
}