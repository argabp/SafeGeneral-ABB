using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenKlaims.Commands;
using ABB.Application.DokumenKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DokumenKlaim.Models
{
    public class DokumenKlaimViewModel : IMapFrom<SaveDokumenKlaimCommand>
    {
        public string kd_cob { get; set; }

        public string kd_dok { get; set; }

        public string? nm_dok { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenKlaimViewModel, SaveDokumenKlaimCommand>();
            profile.CreateMap<DokumenKlaimDto, DokumenKlaimViewModel>();
        }
    }
}