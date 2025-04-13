using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanProduksiAsuransiMasuk.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanProduksiAsuransiMasuk.Models
{
    public class LaporanProduksiAsuransiMasukViewModel : IMapFrom<GetLaporanProduksiAsuransiMasukQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }
        
        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }

        public string? kd_grp_ttg { get; set; }

        public string? kd_rk_ttg { get; set; }

        public string? kd_grp_mkt { get; set; }

        public string? kd_rk_mkt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanProduksiAsuransiMasukViewModel, GetLaporanProduksiAsuransiMasukQuery>();
        }
    }
}