using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Account.Models
{
    public class LoginModel : IMapFrom<LoginCommand>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string UserDatabase { get; set; }
        
        [Required(ErrorMessage = "Captcha harus diisi")]
        public string CaptchaInput { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginModel, LoginCommand>();
        }
    }
}