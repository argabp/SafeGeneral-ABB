using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanBPPDANs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanBPPDAN.Models
{
    public class LaporanBPPDANViewModel : IMapFrom<GetLaporanBPPDANsQuery>
    {
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanBPPDANViewModel, GetLaporanBPPDANsQuery>();
        }
    }
}