using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class AnalisaDanEvaluasiDto : IMapFrom<AnalisaDanEvaluasi>
    {
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string? ket_anev { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AnalisaDanEvaluasi, AnalisaDanEvaluasiDto>();
        }
    }
}