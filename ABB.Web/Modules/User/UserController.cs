using System;
using System.Linq;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Users.Commands;
using ABB.Application.Users.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Base;
using ABB.Web.Modules.User.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace ABB.Web.Modules.User
{
    public class UserController : AuthorizedBaseController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            ViewBag.Module = Request.Cookies["Module"];
            ViewBag.DatabaseName = Request.Cookies["DatabaseName"];
            ViewBag.UserLogin = CurrentUser.UserId;
            
            return View();
        }
        public async Task<ActionResult> GetUsers([DataSourceRequest] DataSourceRequest request, string searchkeyword)
        {
            var ds = await Mediator.Send(new GetUsersQuery() { SearchKeyword = searchkeyword });
            return Json(ds.AsQueryable().ToDataSourceResult(request));
        }

        public async Task<JsonResult> GetRoles()
        {
            var result = await Mediator.Send(new GetRolesQuery());
            return Json(result);
        }

        [HttpGet]
        public  IActionResult Add()
        {
            var model = new AddUserModel();
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddUserModel model)
        {
            try
            {
                var command = Mapper.Map<AddUserCommand>(model);
                command.PasswordExpiredDate = DateTime.Now.AddYears(100);
                command.CreatedBy = CurrentUser.UserId;
                command.UserName = command.UserName?.ToLower();
                await Mediator.Send(command);

                return Json(new { Result = "OK", Message = "Successfully Add User" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id = null)
        {
            var query = await Mediator.Send(new GetUserQuery() { UserId = id });
            var model = Mapper.Map<EditUserModel>(query);
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            
            _logger.LogInformation(JsonConvert.SerializeObject(model));
            try
            {
                var command = Mapper.Map<EditUserCommand>(model);
                command.UpdatedBy = CurrentUser.UserId;
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Edit User" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return PartialView(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            var model = new ChangePasswordModel();
            model.Id = id;
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var command = Mapper.Map<ChangePasswordCommand>(model);
                command.UpdatedBy = CurrentUser.UserId;
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = "Successfully Change Password" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteUserCommand() { Id = id });
            return Json(new { Result = "OK", Message = "Successfully Delete User" });
        }
    }
}