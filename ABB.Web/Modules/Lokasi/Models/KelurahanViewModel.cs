using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class KelurahanViewModel : IMapFrom<SaveKelurahanCommand>
    {
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }

        public string kd_kel { get; set; }

        public string nm_kel { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KelurahanViewModel, SaveKelurahanCommand>();
            profile.CreateMap<KelurahanViewModel, DeleteKelurahanCommand>();
        }
    }
}