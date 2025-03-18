using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Account.Models
{
    public class ChangePasswordModel : IMapFrom<ChangePasswordCommand>
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string UpdatedBy { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChangePasswordModel, ChangePasswordCommand>();
        }
    }
}