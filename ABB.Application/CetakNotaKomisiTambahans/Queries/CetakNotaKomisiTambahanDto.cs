using System;

namespace ABB.Application.CetakNotaKomisiTambahans.Queries
{
    public class CetakNotaKomisiTambahanDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }
        
        public string nm_scob { get; set; }

        public string no_pol { get; set; }

        public string flag_posting { get; set; }

        public string jns_sb_nt { get; set; }

        public string no_akseptasi { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }
        public DateTime? tgl_akh_ptg { get; set; }
        public DateTime? tgl_closing { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? nm_ttj { get; set; }

        public Int16 no_updt { get; set; }

        public DateTime? tgl_nt { get; set; }

        public decimal? nilai_nt { get; set; }
    }
}