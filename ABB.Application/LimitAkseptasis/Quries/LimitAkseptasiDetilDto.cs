using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.LimitAkseptasis.Quries
{
    public class LimitAkseptasiDetilDto : IMapFrom<LimitAkseptasiDetil>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public string nm_user { get; set; }

        public int thn { get; set; }
        
        public decimal pst_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitAkseptasiDetil, LimitAkseptasiDetilDto>();
        }
    }
}