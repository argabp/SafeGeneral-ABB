using ABB.Application.Common.Interfaces;
using ABB.Application.Modules.Commends;
using ABB.Application.Modules.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Module.Models
{
    public class ModuleViewModel : IMapFrom<ModuleDto>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int Sequence { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ModuleDto, ModuleViewModel>();
            profile.CreateMap<ModuleViewModel, AddModuleCommand>();
            profile.CreateMap<ModuleViewModel, EditModuleCommand>();
        }
    }
}