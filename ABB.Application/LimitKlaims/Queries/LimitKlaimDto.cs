using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.LimitKlaims.Queries
{
    public class LimitKlaimDto : IMapFrom<LimitKlaim>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitKlaim, LimitKlaimDto>();
        }
    }
}