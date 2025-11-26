using ABB.Application.Common.Interfaces;
using ABB.Application.RegisterKlaims.Commands;
using ABB.Application.RegisterKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RegisterKlaim.Models
{
    public class DokumenRegisterKlaimModel : IMapFrom<DokumenRegisterKlaimDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
        
        public string kd_dok { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenRegisterKlaimDto, DokumenRegisterKlaimModel>();
            profile.CreateMap<DokumenRegisterKlaimModel, DeleteDokumenRegisterKlaimCommand>();
        }
    }
}