using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.RekapitulasiProduksi.Quries;
using AutoMapper;

namespace ABB.Web.Modules.RekapitulasiProduksi.Models
{
    public class RekapitulasiProduksiViewModel : IMapFrom<GetRekapitulasiProduksiQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string jns_lap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RekapitulasiProduksiViewModel, GetRekapitulasiProduksiQuery>();
        }
    }
}