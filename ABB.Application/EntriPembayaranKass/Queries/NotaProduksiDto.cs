using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPembayaranKass.Queries
{
    public class NotaProduksiDto : IMapFrom<Produksi>
    {
        public string NoNota { get; set; }
        public string NamaTertanggung { get; set; }
        public decimal? Premi { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Produksi, NotaProduksiDto>()
                .ForMember(dest => dest.NoNota, opt => opt.MapFrom(src => src.no_nd))
                .ForMember(dest => dest.NamaTertanggung, opt => opt.MapFrom(src => src.nm_cust2))
                .ForMember(dest => dest.Premi, opt => opt.MapFrom(src => src.premi));
        }
    }
}