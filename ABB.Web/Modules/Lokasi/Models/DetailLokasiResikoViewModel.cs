using ABB.Application.Common.Interfaces;
using ABB.Application.Lokasis.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.Lokasi.Models
{
    public class DetailLokasiResikoViewModel : IMapFrom<SaveDetailLokasiResikoCommand>
    {
        public string? kd_pos { get; set; }

        public string? kd_lok_rsk { get; set; }

        public string? gedung { get; set; }

        public string? alamat { get; set; }

        public string? kd_prop { get; set; }

        public string? kd_kab { get; set; }

        public string? kd_kec { get; set; }

        public string? kd_kel { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailLokasiResikoViewModel, SaveDetailLokasiResikoCommand>();
            profile.CreateMap<DetailLokasiResikoViewModel, DeleteDetailLokasiResikoCommand>();
            profile.CreateMap<DetailLokasiResiko, DetailLokasiResikoViewModel>();
        }
    }
}