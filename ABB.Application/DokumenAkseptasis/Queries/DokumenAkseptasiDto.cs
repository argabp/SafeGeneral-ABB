using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class DokumenAkseptasiDto : IMapFrom<DokumenAkseptasi>
    {
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_dokumenakseptasi { get; set; }

        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenAkseptasi, DokumenAkseptasiDto>();
        }
    }
}