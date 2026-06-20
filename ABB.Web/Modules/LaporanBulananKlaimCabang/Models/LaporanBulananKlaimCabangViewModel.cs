using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanBulananKlaimCabangs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanBulananKlaimCabang.Models
{
    public class LaporanBulananKlaimCabangViewModel : IMapFrom<GetLaporanBulananKlaimCabangsQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanBulananKlaimCabangViewModel, GetLaporanBulananKlaimCabangsQuery>();
        }
    }
}