using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanBulananFakultatifs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LaporanBulananFakultatif.Models
{
    public class LaporanBulananFakultatifViewModel : IMapFrom<LaporanBulananFakultatifCommand>
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string jns_tr { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanBulananFakultatifViewModel, LaporanBulananFakultatifCommand>();
        }
    }
}