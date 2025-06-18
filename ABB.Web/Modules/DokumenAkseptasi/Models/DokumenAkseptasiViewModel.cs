using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenAkseptasis.Commands;
using ABB.Application.DokumenAkseptasis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DokumenAkseptasi.Models
{
    public class DokumenAkseptasiViewModel : IMapFrom<AddDokumenAkseptasiCommand>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_dokumenakseptasi { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenAkseptasiViewModel, AddDokumenAkseptasiCommand>();
            profile.CreateMap<DokumenAkseptasiViewModel, EditDokumenAkseptasiCommand>();
            profile.CreateMap<DokumenAkseptasiDto, DokumenAkseptasiViewModel>();
        }
    }
}