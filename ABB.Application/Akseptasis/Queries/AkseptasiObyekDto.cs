using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Akseptasis.Queries
{
    public class AkseptasiObyekDto : IMapFrom<AkseptasiObyek>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }

        public string kd_grp_oby { get; set; }

        public string desk_oby { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public decimal pst_adj { get; set; }

        public string? no_pol_ttg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiObyek, AkseptasiObyekDto>();
        }
    }
}