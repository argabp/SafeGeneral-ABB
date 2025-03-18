using ABB.Application.Common.Interfaces;
using ABB.Application.Kapals.Commands;
using ABB.Application.Kapals.Queries;
using AutoMapper;

namespace ABB.Web.Modules.Kapal.Models
{
    public class KapalViewModel : IMapFrom<SaveKapalCommand>
    {
        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string kd_negara { get; set; }

        public int? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? st_class { get; set; }

        public string? no_reg { get; set; }

        public string? no_imo { get; set; }

        public string? ekuitas { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<KapalViewModel, SaveKapalCommand>();
            profile.CreateMap<KapalDto, KapalViewModel>();
        }
    }
}