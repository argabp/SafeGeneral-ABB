using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Akseptasis.Queries
{
    public class AkseptasiOtherCargoDetailDto : IMapFrom<AkseptasiOtherCargoDetail>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_urut { get; set; }

        public string jns_angkut { get; set; }

        public string? kd_angkut { get; set; }

        public string nm_angkut { get; set; }

        public string no_bl { get; set; }
        
        public string no_inv { get; set; }
        
        public string no_po { get; set; }
        
        public string no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiOtherCargoDetail, AkseptasiOtherCargoDetailDto>();
        }
    }
}