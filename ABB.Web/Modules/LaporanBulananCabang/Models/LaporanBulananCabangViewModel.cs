using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanBulananCabang.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanBulananCabang.Models
{
    public class LaporanBulananCabangViewModel : IMapFrom<GetLaporanBulananCabangQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanBulananCabangViewModel, GetLaporanBulananCabangQuery>();
        }
    }
}