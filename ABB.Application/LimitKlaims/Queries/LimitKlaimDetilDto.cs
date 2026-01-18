using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.LimitKlaims.Queries
{
    public class LimitKlaimDetilDto : IMapFrom<LimitKlaimDetil>
    {
        public int Id { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public string nm_user { get; set; }

        public int thn { get; set; }

        public decimal nilai_limit_awal { get; set; }

        public decimal nilai_limit_akhir { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitKlaimDetil, LimitKlaimDetilDto>();
        }
    }
}