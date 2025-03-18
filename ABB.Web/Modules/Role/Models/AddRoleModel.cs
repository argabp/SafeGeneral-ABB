using System;
using System.ComponentModel;
using ABB.Application.Common.Interfaces;
using ABB.Application.Roles.Commends;
using ABB.Application.Roles.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Role.Models
{
    public class AddRoleModel : IMapFrom<AddRoleCommand>
    {
        public AddRoleModel()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; set; }
        [DisplayName("Role Name")] public string Name { get; set; }
        [DisplayName("Role Id")] public int? RoleCode { get; set; }
        public string Description { get; set; }
        public int WorkspaceId { get; set; }
        public string UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RolesDto, AddRoleModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName));
            profile.CreateMap<AddRoleModel, AddRoleCommand>();
        }
    }
}