using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class KabupatenViewModel : IMapFrom<SaveKabupatenCommand>
    {
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string nm_kab { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KabupatenViewModel, SaveKabupatenCommand>();
            profile.CreateMap<KabupatenViewModel, DeleteKabupatenCommand>();
        }
    }
}