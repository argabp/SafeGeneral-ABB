using ABB.Application.Asumsis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Asumsi.Models
{
    public class AsumsiViewModel : IMapFrom<AddAsumsiCommand>
    {
        public string KodeAsumsi { get; set; }

        public string NamaAsumsi { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AsumsiViewModel, AddAsumsiCommand>();
            profile.CreateMap<AsumsiViewModel, EditAsumsiCommand>();
            profile.CreateMap<AsumsiViewModel, DeleteAsumsiCommand>();
        }
    }
}