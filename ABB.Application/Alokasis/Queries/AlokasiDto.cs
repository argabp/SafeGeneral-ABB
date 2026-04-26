using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Alokasis.Queries
{
    public class AlokasiDto : IMapFrom<Alokasi>
    {
        public string Id { get; set; }

        public string nm_cb { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cob { get; set; }

        public string kd_cob { get; set; }
        
        public string nm_scob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }

        public string kd_mtu_prm { get; set; }

        public string nm_mtu_prm { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public DateTime tgl_closing { get; set; }

        public string flag_closing { get; set; }
        
        public string? nm_ttg { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }
        
        public DateTime? tgl_akh_ptg { get; set; }

        public string? ket_endt { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public string? st_tty { get; set; }

        public DateTime? tgl_sor { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AlokasiDto, Alokasi>();
            profile.CreateMap<Alokasi, AlokasiDto>();
        }
    }
}