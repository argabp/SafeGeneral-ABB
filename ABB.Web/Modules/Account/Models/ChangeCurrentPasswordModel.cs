using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Account.Models
{
    public class ChangeCurrentPasswordModel : IMapFrom<ChangeCurrentPasswordCommand>
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ChangeCurrentPasswordModel, ChangeCurrentPasswordCommand>();
        }
    }
}