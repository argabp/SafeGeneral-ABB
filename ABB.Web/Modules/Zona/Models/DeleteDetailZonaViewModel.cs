using ABB.Application.Common.Interfaces;
using ABB.Application.Zonas.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Zona.Models
{
    public class DeleteDetailZonaViewModel : DeleteZonaViewModel, IMapFrom<DeleteDetailZonaCommand>
    {
        public string kd_kls_konstr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailZonaViewModel, DeleteDetailZonaCommand>();
        }
    }
}