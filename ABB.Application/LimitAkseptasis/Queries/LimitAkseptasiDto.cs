using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.LimitAkseptasis.Queries
{
    public class LimitAkseptasiDto : IMapFrom<LimitAkseptasi>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public decimal? pst_limit_cb { get; set; }

        public Int16? maks_panel { get; set; }
        
        public decimal? nilai_kapasitas { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitAkseptasi, LimitAkseptasiDto>();
        }
    }
}