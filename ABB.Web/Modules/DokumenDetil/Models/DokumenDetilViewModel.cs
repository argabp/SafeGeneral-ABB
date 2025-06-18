using ABB.Application.Common.Interfaces;
using ABB.Application.DokumenDetils.Commands;
using AutoMapper;

namespace ABB.Web.Modules.DokumenDetil.Models
{
    public class DokumenDetilViewModel : IMapFrom<SaveDokumenDetilCommand>
    {
        public string kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenDetilViewModel, SaveDokumenDetilCommand>();
            profile.CreateMap<DokumenDetilViewModel, DeleteDokumenDetilCommand>();
        }
    }
}