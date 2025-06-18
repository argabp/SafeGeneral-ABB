using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenAkseptasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DokumenAkseptasi.Models
{
    public class DeleteDokumenAkseptasiViewModel : IMapFrom<DeleteDokumenAkseptasiCommand>
    {

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDokumenAkseptasiViewModel, DeleteDokumenAkseptasiCommand>();
        }
    }
}