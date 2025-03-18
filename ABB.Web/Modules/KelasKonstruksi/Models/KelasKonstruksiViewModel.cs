using ABB.Application.Common.Interfaces;
using ABB.Application.KelasKonsturksis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.KelasKonstruksi.Models
{
    public class KelasKonstruksiViewModel : IMapFrom<SaveKelasKonstruksiCommand>
    {
        public string kd_kls_konstr { get; set; }

        public string nm_kls_konstr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KelasKonstruksiViewModel, SaveKelasKonstruksiCommand>();
            profile.CreateMap<KelasKonstruksiViewModel, DeleteKelasKonstruksiCommand>();
        }
    }
}