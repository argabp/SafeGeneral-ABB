using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class MutasiKlaimObyekDto : IMapFrom<MutasiKlaimObyek>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_oby { get; set; }

        public string nm_oby { get; set; }

        public decimal nilai_kl { get; set; }

        public Int16? no_rsk { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MutasiKlaimObyek, MutasiKlaimObyekDto>();
        }
    }
}