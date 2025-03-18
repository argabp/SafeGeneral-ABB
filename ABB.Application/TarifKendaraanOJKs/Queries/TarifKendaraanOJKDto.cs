namespace ABB.Application.TarifKendaraanOJKs.Queries
{
    public class TarifKendaraanOJKDto
    {
        public string Id { get; set; }
        
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string nm_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }
        
        public string nm_wilayah { get; set; }

        public short no_kategori { get; set; }

        public decimal nilai_ptg_mul { get; set; }
        public decimal nilai_ptg_akh { get; set; }
        public byte stn_rate_prm { get; set; }
        public string nm_stn_rate_prm { get; set; }
        public decimal pst_rate_prm_min { get; set; }
        public decimal pst_rate_prm_max { get; set; }
    }
}