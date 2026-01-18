namespace ABB.Domain.Entities
{
    public class LimitKlaimDetil
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }
        
        public decimal nilai_limit_akhir { get; set; }
    }
}