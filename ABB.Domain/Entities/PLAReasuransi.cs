namespace ABB.Domain.Entities
{
    public class PLAReasuransi
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public short no_mts { get; set; }

        public short no_pla { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string? ket_pla { get; set; }

        public string flag_posting { get; set; }
    }
}