using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.Roles.Commends;
using AutoMapper;

namespace ABB.Web.Modules.Sample.Models
{
    public class CreateRoleModel : IMapFrom<AddRoleCommand>
    {
        public CreateRoleModel()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }

        public string Username { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRoleModel, AddRoleCommand>();
        }
    }
}