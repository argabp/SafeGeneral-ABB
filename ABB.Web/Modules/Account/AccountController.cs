using System;
using System.IO;
using System.Linq;
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
            {
                return Redirect("/Home/Index");
            }

            if ((bool)(TempData["SuccessChangePassword"] ?? false)) ViewData["ShowSuccessMessage"] = "true";
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.Username = model.Username?.ToLower() ?? "";
            
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
    
            // Validate Captcha first
            if (string.IsNullOrEmpty(model.CaptchaInput) || model.CaptchaInput != sessionCaptcha)
            {
                ModelState.AddModelError("CaptchaInput", "Kode Captcha salah.");
                // Return view with existing data
                return View(model); 
            }
            
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCaptcha()
        {
            // 1. Generate Random Text (Mix of Letters and Numbers)
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random rand = new Random();
            string captchaCode = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            // 2. Store in Session
            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            // 3. Create Image
            using (var bitmap = new Bitmap(130, 45))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.FromArgb(244, 244, 244)); // Light grey background

                // Add some noise lines so bots can't read it easily
                for (int i = 0; i < 3; i++)
                    graphics.DrawLine(new Pen(Color.Silver, 1), rand.Next(0, 130), rand.Next(0, 45), rand.Next(0, 130), rand.Next(0, 45));

                // Draw the text with a bit of rotation
                using (Font font = new Font("Arial", 20, FontStyle.Bold))
                {
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        var state = graphics.Save();
                        graphics.TranslateTransform(20 + (i * 18), 22);
                        graphics.RotateTransform(rand.Next(-15, 15));
                        graphics.DrawString(captchaCode[i].ToString(), font, Brushes.DarkBlue, -10, -15);
                        graphics.Restore(state);
                    }
                }

                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }
    }
}
