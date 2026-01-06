using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DokumenKlaim.Models
{
    public class DeleteDokumenKlaimViewModel : IMapFrom<DeleteDokumenKlaimCommand>
    {

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDokumenKlaimViewModel, DeleteDokumenKlaimCommand>();
        }
    }
}