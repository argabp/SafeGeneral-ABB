namespace ABB.Application.CancelPostingNotaKlaimReasuransis.Queries
{
    public class CancelPostingNotaKlaimReasuransiDto
    {
        public string Id { get; set; }
        
        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public string st_tipe_dla { get; set; }

        public string nomor_berkas { get; set; }

        public string nm_ttg { get; set; }

        public string flag_cancel { get; set; }

        public string flag_posting { get; set; }
    }
}