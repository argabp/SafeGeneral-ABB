using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.ModuleNavigations.Commands;
using ABB.Application.ModuleNavigations.Queries;
using AutoMapper;

namespace ABB.Web.Modules.ModuleNavigation.Models
{
    public class ModuleNavigationViewModel : IMapFrom<SaveModuleNavigationCommand>
    {
        public int? ModuleId { get; set; }

        public List<int> Navigations { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ModuleNavigationViewModel, SaveModuleNavigationCommand>();
            profile.CreateMap<ModuleNavigationDto, ModuleNavigationViewModel>();
        }
    }
}