using System.ComponentModel;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using AutoMapper;

namespace ABB.Web.Modules.User.Models
{
    public class ChangePasswordModel : IMapFrom<ChangePasswordCommand>
        {
            public string Id { get; set; }
            [DisplayName("New Password")]
            public string NewPassword { get; set; }
            [DisplayName("Confirm Password")]
            public string ConfirmPassword { get; set; }



            public void Mapping(Profile profile)
            {
                profile.CreateMap<ChangePasswordModel, ChangePasswordCommand>();

            }
        }
}