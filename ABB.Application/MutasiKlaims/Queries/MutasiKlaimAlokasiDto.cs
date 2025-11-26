using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class MutasiKlaimAlokasiDto : IMapFrom<MutasiKlaimAlokasi>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string kd_grp_pas { get; set; }

        public string nm_grp_pas { get; set; }
        
        public string kd_rk_pas { get; set; }
        
        public string nm_rk_pas { get; set; }

        public decimal pst_share { get; set; }
        
        public decimal nilai_kl { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MutasiKlaimAlokasi, MutasiKlaimAlokasiDto>();
        }
    }
}