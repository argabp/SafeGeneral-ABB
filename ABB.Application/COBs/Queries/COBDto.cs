using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.COBs.Queries
{
    public class COBDto : IMapFrom<COB>
    {
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string nm_cob_ing { get; set; }

        public string kd_class { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<COB, COBDto>();
        }
    }
}