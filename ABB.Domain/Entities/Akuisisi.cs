namespace ABB.Domain.Entities
{
    public class Akuisisi
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int kd_thn { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_acq { get; set; }

        public decimal nilai_maks_acq { get; set; }

        public decimal nilai_acq { get; set; }
    }
}