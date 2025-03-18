using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class KecamatanViewModel : IMapFrom<SaveKecamatanCommand>
    {
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string nm_kec { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KecamatanViewModel, SaveKecamatanCommand>();
            profile.CreateMap<KecamatanViewModel, DeleteKecamatanCommand>();
        }
    }
}