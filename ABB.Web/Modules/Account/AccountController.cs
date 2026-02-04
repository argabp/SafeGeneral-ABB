using System;
using System.Threading.Tasks;
using ABB.Application.Common.Exceptions;
using ABB.Application.Users.Commands;
using ABB.Application.Users.Queries;
using ABB.Web.Extensions;
using ABB.Web.Modules.Account.Models;
using ABB.Web.Modules.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Account
{
    public class AccountController : AuthorizedBaseController
    {
        private readonly string Module = "Module";
        private readonly string DatabaseName = "DatabaseName";
        private readonly string DatabaseValue = "DatabaseValue";
        private readonly string UserCabang = "UserCabang";
        
        public AccountController()
        {
        }
        
        private async Task<ProfileViewModel> GetProfileModel()
        {
            var profile = await Mediator.Send(new GetUserProfileQuery());
            var userProfile = Mapper.Map<ChangeProfileModel>(profile);
            var userPassword = new ChangeCurrentPasswordModel() { Id = userProfile.Id };
            var model = new ProfileViewModel();
            model.UserProfile = userProfile;
            model.UserPassword = userPassword;
            return model;
        }
        
        public async Task<IActionResult> Profile()
        {
            return View(await GetProfileModel());
        }
        
        public async Task<IActionResult> ChangeProfile()
        {
            return PartialView(await GetProfileModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeProfile(ProfileViewModel model)
        {
            try
            {
                await Mediator.Send(Mapper.Map<ChangeUserProfileCommand>(model.UserProfile));
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex, "UserProfile");
            }

            return PartialView(model);
        }
        
        public async Task<IActionResult> ChangeCurrentPassword()
        {
            return PartialView(await GetProfileModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangeCurrentPassword(ProfileViewModel model)
        {
            try
            {
                await Mediator.Send(Mapper.Map<ChangeCurrentPasswordCommand>(model.UserPassword));
                return Json(new { Result = "OK", Message = "Data Berhasil Disimpan" });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex, "UserPassword");
            }
            return PartialView(model);
        }
        
        [AllowAnonymous]
        public IActionResult Login()
        {
            // real auth state check
            var hasSession = HttpContext.Session.GetString("SessionId") != null;
            var user = HttpContext.User.Identity?.Name;

            if (hasSession && !string.IsNullOrEmpty(user))
                return Redirect("/Home/Index");

            if ((bool)(TempData["SuccessChangePassword"] ?? false)) ViewData["ShowSuccessMessage"] = "true";
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.Username = model.Username?.ToLower() ?? "";
            
            try
            {
                if (await Mediator.Send(Mapper.Map<LoginCommand>(model)))
                {
                    var userCabang = await Mediator.Send(new GetCabangQuery()
                    {
                        kd_cb = model.UserDatabase
                    });
                    HttpContext.Response.Cookies.Append(Module, "-1");
                    if (userCabang != null)
                    {
                        HttpContext.Response.Cookies.Append(DatabaseName, userCabang.nm_cb);
                        HttpContext.Response.Cookies.Append(DatabaseValue, userCabang.database_name);
                        HttpContext.Response.Cookies.Append(UserCabang, userCabang.kd_cb);
                    }
                    
                    HttpContext.Session.SetString("SessionId", Guid.NewGuid().ToString("N"));
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }

            return View(model);
        }
        
        [AllowAnonymous]
        public IActionResult ChangePassword(string id)
        {
            var model = new ChangePasswordModel();
            model.Id = model.UpdatedBy = id;
            return View(model);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var command = Mapper.Map<ChangePasswordCommand>(model);
                command.UpdatedBy = model.Id;
                if (await Mediator.Send(command))
                {
                    TempData["SuccessChangePassword"] = true;
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            return View(model);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await Mediator.Send(new LogoutCommand());

            HttpContext.Response.Cookies.Delete(Module);
            HttpContext.Response.Cookies.Delete(DatabaseName);
            HttpContext.Response.Cookies.Delete(DatabaseValue);
            
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetUserCabang([FromQuery] string username)
        {
            var result = await Mediator.Send(new GetUserCabangQuery(){ Username = username});

            return Ok(result);
        }

    }
}
