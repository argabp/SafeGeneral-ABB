using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.NotaKomisiTambahans.Queries
{
    public class NotaKomisiTambahanDto : IMapFrom<NotaKomisiTambahan>
    {
        public string jns_sb_nt { get; set; }

        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string jns_tr { get; set; }
        
        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string? no_nt_lama { get; set; }
        
        public string? no_ref { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public string? kd_grp_ttj { get; set; }

        public string? kd_rk_ttj { get; set; }

        public string? nm_ttj { get; set; }

        public string? almt_ttj { get; set; }

        public string? kt_ttj { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public decimal nilai_nt { get; set; }

        public decimal? nilai_lns { get; set; }

        public DateTime tgl_nt { get; set; }

        public string? ket_nt { get; set; }

        public string flag_posting { get; set; }

        public decimal pst_ppn { get; set; }

        public decimal nilai_ppn { get; set; }

        public decimal pst_pph { get; set; }

        public decimal nilai_pph { get; set; }

        public string? tipe_mts { get; set; }

        public string? kd_jns_sor { get; set; }

        public string? kd_rk_sor { get; set; }

        public string? uraian { get; set; }

        public decimal? pst_nt { get; set; }

        public decimal? nilai_prm { get; set; }

        public string? no_pol_ttg { get; set; }

        public decimal? pst_lain { get; set; }

        public decimal? nilai_lain { get; set; }

        public string no_akseptasi { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }
        public DateTime? tgl_akh_ptg { get; set; }
        public DateTime? tgl_closing { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotaKomisiTambahan, NotaKomisiTambahanDto>();
        }
    }
}