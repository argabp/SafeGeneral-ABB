using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Coas.Queries
{
    public class CoaDto : IMapFrom<Coa>
    {
        public string Kode { get; set; }
        public string Nama { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Coa, CoaDto>()
                .ForMember(d => d.Kode, opt => opt.MapFrom(s => s.gl_kode))
                .ForMember(d => d.Nama, opt => opt.MapFrom(s => s.gl_nama));
        }
    }
}