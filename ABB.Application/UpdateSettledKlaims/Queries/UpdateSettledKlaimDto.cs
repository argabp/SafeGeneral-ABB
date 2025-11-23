using System;

namespace ABB.Application.UpdateSettledKlaims.Queries
{
    public class UpdateSettledKlaimDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string? no_pol_lama { get; set; }

        public Int16? no_updt { get; set; }

        public Int16? no_rsk { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string nomor_register_klaim { get; set; }
    }
}