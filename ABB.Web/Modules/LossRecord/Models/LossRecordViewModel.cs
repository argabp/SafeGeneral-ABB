using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LossRecord.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LossRecord.Models
{
    public class LossRecordViewModel : IMapFrom<GetLossRecordQuery>
    {
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string? kd_grp_ttg { get; set; }

        public string? kd_rk_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LossRecordViewModel, GetLossRecordQuery>();
        }
    }
}