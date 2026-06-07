using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanMaiparks.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanMaipark.Models
{
    public class LaporanMaiparkViewModel : IMapFrom<GetLaporanMaiparksQuery>
    {
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanMaiparkViewModel, GetLaporanMaiparksQuery>();
        }
    }
}