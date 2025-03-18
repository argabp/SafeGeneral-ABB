using ABB.Application.Common.Interfaces;
using ABB.Application.Zonas.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Zona.Models
{
    public class SaveZonaViewModel : IMapFrom<AddZonaCommand>
    {
        public string kd_zona { get; set; }

        public string nm_zona { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveZonaViewModel, AddZonaCommand>();
            profile.CreateMap<SaveZonaViewModel, EditZonaCommand>();
        }
    }
}