using ABB.Application.Common.Interfaces;
using ABB.Application.Zonas.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.Zona.Models
{
    public class DetailZonaViewModel : IMapFrom<SaveDetailZonaCommand>
    {
        public string kd_zona { get; set; }

        public string kd_kls_konstr { get; set; }

        public string nm_zona_gb { get; set; }

        public string kd_okup { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailZonaViewModel, SaveDetailZonaCommand>();
            profile.CreateMap<DetailZona, DetailZonaViewModel>();
        }
    }
}