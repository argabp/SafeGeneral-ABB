using System.ComponentModel;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using ABB.Application.Users.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.Account.Models
{
    public class ChangeProfileModel : IMapFrom<UserProfileDto>, IMapFrom<ChangeUserProfileCommand>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        [DisplayName("First Name")] public string FirstName { get; set; }
        [DisplayName("Last Name")] public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public IFormFile ProfilePhoto { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfileDto, ChangeProfileModel>();
            profile.CreateMap<ChangeProfileModel, ChangeUserProfileCommand>();
        }
    }
}