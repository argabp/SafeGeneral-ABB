using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class LokasiResikoViewModel : IMapFrom<SaveLokasiResikoCommand>
    {
        public string kd_pos { get; set; }

        public string jalan { get; set; }

        public string kota { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LokasiResikoViewModel, SaveLokasiResikoCommand>();
            profile.CreateMap<LokasiResikoViewModel, DeleteLokasiResikoCommand>();
        }
    }
}