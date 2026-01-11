using ABB.Application.Common.Interfaces;
using ABB.Application.RegisterKlaims.Commands;
using ABB.Application.RegisterKlaims.Queries;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.RegisterKlaim.Models
{
    public class DokumenRegisterKlaimViewModel : IMapFrom<DokumenRegisterKlaimDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }

        public string? flag_dok { get; set; }

        public string? link_file { get; set; }

        public bool? flag_wajib { get; set; }

        public IFormFile file { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenRegisterKlaimViewModel, DokumenRegisterKlaimDto>();
            profile.CreateMap<DokumenRegisterKlaimViewModel, SaveDokumenRegisterKlaimCommand>();
            profile.CreateMap<DokumenRegisterKlaimDto, DokumenRegisterKlaimViewModel>();
        }
    }
}