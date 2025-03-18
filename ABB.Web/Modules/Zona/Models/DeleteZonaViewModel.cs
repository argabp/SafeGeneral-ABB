using ABB.Application.Common.Interfaces;
using ABB.Application.Zonas.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Zona.Models
{
    public class DeleteZonaViewModel : IMapFrom<DeleteZonaCommand>
    {
        public string kd_zona { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteZonaViewModel, DeleteZonaCommand>();
        }
    }
}