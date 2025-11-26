using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class MutasiKlaimBebanDto : IMapFrom<MutasiKlaimBeban>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_urut { get; set; }

        public string st_jns { get; set; }
        
        public string ket_jns { get; set; }
        
        public string kd_mtu { get; set; }

        public decimal nilai_jns_org { get; set; }
        
        public decimal nilai_jns { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MutasiKlaimBeban, MutasiKlaimBebanDto>();
        }
    }
}