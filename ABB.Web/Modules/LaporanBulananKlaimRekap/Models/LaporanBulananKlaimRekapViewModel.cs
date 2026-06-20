using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanBulananKlaimRekaps.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanBulananKlaimRekap.Models
{
    public class LaporanBulananKlaimRekapViewModel : IMapFrom<GetLaporanBulananKlaimRekapQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanBulananKlaimRekapViewModel, GetLaporanBulananKlaimRekapQuery>();
        }
    }
}