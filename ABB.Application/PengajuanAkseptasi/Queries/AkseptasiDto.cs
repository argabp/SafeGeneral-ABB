using System;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class AkseptasiDto
    {
        public string no_pol_ttg { get; set; }
        public string no_pol_ttg_masked { get; set; }

        public string nm_ttg { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }
    }
}