using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class RegisterKlaimDto : IMapFrom<RegisterKlaim>
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_cb { get; set; }
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string register_klaim { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string flag_pol_lama { get; set; }

        public string flag_tty_msk { get; set; }

        public string? no_pol_lama { get; set; }

        public string? kd_thn_pol { get; set; }

        public string? no_pol { get; set; }

        public Int16? no_updt { get; set; }

        public Int16? no_rsk { get; set; }

        public string? kd_jns_sor { get; set; }

        public string? kd_tty_msk { get; set; }

        public string? no_lks_lama { get; set; }

        public string flag_settled { get; set; }

        public string? st_jns_peny { get; set; }

        public string? ket_oby { get; set; }

        public DateTime tgl_lapor { get; set; }

        public DateTime tgl_kej { get; set; }

        public string? tempat_kej { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? kond_ptg { get; set; }

        public string? kd_sebab { get; set; }

        public string? sifat_kerugian { get; set; }

        public DateTime tgl_lns_prm { get; set; }

        public string? no_bukti_lns { get; set; }

        public DateTime tgl_reg { get; set; }

        public string? ket_kl { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public string? pelapor { get; set; }

        public string? st_pelapor { get; set; }

        public string? no_tlp_pelapor { get; set; }

        public string? penerima { get; set; }

        public string st_reg { get; set; }

        public string? kd_wilayah { get; set; }

        public string? kd_grp_bkl { get; set; }

        public string? kd_rk_bkl { get; set; }
        
        public string nm_ttg { get; set; }

        public Int16 no_mts { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterKlaim, RegisterKlaimDto>();
        }
    }
}