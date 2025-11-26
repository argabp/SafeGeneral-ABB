using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class DokumenRegisterKlaimDto : IMapFrom<DokumenRegisterKlaim>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }

        public string? flag_dok { get; set; }

        public string? link_file { get; set; }

        public string dokumenName  { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenRegisterKlaim, DokumenRegisterKlaimDto>();
        }
    }
}