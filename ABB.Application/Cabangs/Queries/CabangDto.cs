using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Cabangs.Queries
{
    public class CabangDto : IMapFrom<Cabang>
    {
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string almt { get; set; }

        public string kt { get; set; }

        public string kd_pos { get; set; }

        public string no_tlp { get; set; }

        public string npwp { get; set; }

        public string no_fax { get; set; }

        public string email { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Cabang, CabangDto>();
        }
    }
}