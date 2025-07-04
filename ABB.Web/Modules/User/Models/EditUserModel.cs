using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using ABB.Application.Users.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.User.Models
{
    public class EditUserModel : IMapFrom<UserDto>, IMapFrom<EditUserCommand>
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string LeaderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Photo { get; set; }
        public IFormFile ProfilePhoto { get; set; }

        public string Signature { get; set; }
        
        public IFormFile SignatureFile { get; set; }

        public string Jabatan { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDto, EditUserModel>();
            profile.CreateMap<EditUserModel, EditUserCommand>();

        }
    }
}