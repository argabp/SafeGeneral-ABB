using System;

namespace ABB.Application.CetakSlipPremiFakultatifKeluars.Queries
{
    public class CetakSlipPremiFakultatifKeluarModel
    {
        public string? nm_cob { get; set; }

        public string? nm_scob { get; set; }

        public string? nm_rk { get; set; }

        public string? no_slip { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? nm_ttg { get; set; }

        public DateTime? tgl_mul_reas { get; set; }

        public DateTime? tgl_akh_reas { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public decimal? pst_adj_reas { get; set; }

        public byte? stn_adj_reas { get; set; }

        public string? symbol { get; set; }

        public decimal? nilai_ttl_ptg_reas { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }

        public decimal? pst_kms_reas { get; set; }

        public string? ket_tc { get; set; }

        public string? ket_net { get; set; }

        public string? almt_rsk { get; set; }

        public decimal? nilai_prm_reas { get; set; }
        
        public decimal? nilai_adj_reas { get; set; }

        public decimal? nilai_kms_reas { get; set; }

        public decimal? nilai_nt { get; set; }

        public string? tgl_nt { get; set; }

        public string? nm_bag { get; set; }

        public string? nm_kpl_bag { get; set; }
    }
}