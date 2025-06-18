using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.LimitAkseptasis.Quries
{
    public class LimitAkseptasiDto : IMapFrom<LimitAkseptasi>
    {
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_limit { get; set; }

        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitAkseptasi, LimitAkseptasiDto>();
        }
    }
}