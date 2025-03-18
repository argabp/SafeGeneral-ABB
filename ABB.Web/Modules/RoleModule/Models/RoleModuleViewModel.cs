using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.RoleModules.Commands;
using ABB.Application.RoleModules.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RoleModule.Models
{
    public class RoleModuleViewModel : IMapFrom<SaveRoleModuleCommand>
    {
        public string RoleId { get; set; }

        public List<int> Modules { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoleModuleViewModel, SaveRoleModuleCommand>();
            profile.CreateMap<RoleModuleDto, RoleModuleViewModel>();
        }
    }
}