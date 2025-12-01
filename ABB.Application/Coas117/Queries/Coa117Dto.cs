using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Coas117.Queries
{
    public class Coa117Dto : IMapFrom<Coa117>
    {
        public string Kode { get; set; }
        public string Nama { get; set; }
        public string Dept { get; set; }
        public string Type { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Coa117, Coa117Dto>()
                .ForMember(d => d.Kode, opt => opt.MapFrom(s => s.gl_kode))
                .ForMember(d => d.Nama, opt => opt.MapFrom(s => s.gl_nama))
                .ForMember(d => d.Dept, opt => opt.MapFrom(s => s.gl_dept))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.gl_type));
        }
    }
}