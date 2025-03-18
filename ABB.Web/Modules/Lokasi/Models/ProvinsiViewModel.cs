using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class ProvinsiViewModel : IMapFrom<SaveProvinsiCommand>
    {
        public string kd_prop { get; set; }

        public string nm_prop { get; set; }

        public string no_pos { get; set; }

        public string kd_wilayah { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProvinsiViewModel, SaveProvinsiCommand>();
            profile.CreateMap<ProvinsiViewModel, DeleteProvinsiCommand>();
        }
    }
}