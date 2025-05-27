using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.User.Models
{
    public class AddUserModel : IMapFrom<AddUserCommand>
    {
        public AddUserModel()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string LeaderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public bool LockoutEnabled { get; set; } = false;
        public string Photo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public IFormFile SignatureFile { get; set; }

        public string Jabatan { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddUserModel, AddUserCommand>();
        }

    }
}