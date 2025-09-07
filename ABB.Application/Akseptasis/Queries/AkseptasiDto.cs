using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Akseptasis.Queries
{
    public class AkseptasiDto : IMapFrom<Akseptasi>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string nm_cb { get; set; }
        
        public string nm_cob { get; set; }
        
        public string nm_scob { get; set; }
        
        public byte no_renew { get; set; }

        public decimal thn_uw { get; set; }

        public string no_endt { get; set; }

        public string? kd_updt { get; set; }

        public string? no_reg { get; set; }

        public string? no_pol_lama { get; set; }

        public string? kd_grp_ttg { get; set; }

        public string? kd_rk_ttg { get; set; }

        public string nm_ttg { get; set; }

        public string? almt_ttg { get; set; }

        public string? kt_ttg { get; set; }

        public string? nm_qq { get; set; }

        public string? kd_grp_brk { get; set; }

        public string? kd_rk_brk { get; set; }

        public string st_pas { get; set; }

        public string? kd_grp_pas { get; set; }

        public string? kd_rk_pas { get; set; }

        public string? kd_grp_bank { get; set; }

        public string? kd_rk_bank { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }
        
        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public decimal pst_share_bgu { get; set; }

        public Int16 jk_wkt_ptg { get; set; }

        public decimal faktor_prd { get; set; }

        public string? no_pol_pas { get; set; }

        public string? no_pol_induk { get; set; }

        public string? ctt_pol { get; set; }

        public string? lamp_pol { get; set; }

        public string? ket_endt { get; set; }

        public string? desk_deduct { get; set; }

        public string? ket_klausula { get; set; }

        public string? no_brdr { get; set; }

        public DateTime? tgl_brdr { get; set; }

        public string flag_konv { get; set; }

        public string? flag_reas { get; set; }

        public DateTime? tgl_ttd { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string? kd_usr_closing { get; set; }

        public string? kd_grp_mkt { get; set; }

        public string? kd_rk_mkt { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_konfirm { get; set; }

        public string? no_survey { get; set; }

        public string? link_file { get; set; }

        public string? st_cover { get; set; }

        public DateTime? tgl_maintenance { get; set; }

        public byte? wpc { get; set; }

        public string? flag_dis_fleet { get; set; }

        public string? ket_no_reg { get; set; }

        public string? no_reff { get; set; }

        public string? st_aks { get; set; }

        public string? nomor_pengajuan { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Akseptasi, AkseptasiDto>();
        }
    }
}