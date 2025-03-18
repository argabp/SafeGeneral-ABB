using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.SCOBs.Queries
{
    public class SCOBDto : IMapFrom<SCOB>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_scob_ing { get; set; }

        public string kd_map_scob { get; set; }

        public string kd_sub_grp { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SCOB, SCOBDto>();
        }
    }
}