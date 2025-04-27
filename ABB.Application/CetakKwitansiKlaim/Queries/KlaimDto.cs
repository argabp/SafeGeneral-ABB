namespace ABB.Application.CetakKwitansiKlaim.Queries
{
    public class KlaimDto
    {
        public string Id { get; set; }
        public string nm_cb { get; set; }
        
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string jns_tr { get; set; }
        public string jns_nt_msk { get; set; }
        public string no_nt_msk { get; set; }
        public string jns_nt_kel { get; set; }
        public string no_nt_kel { get; set; }
        public string flag_posting { get; set; }
        public string nm_ttj { get; set; }
        public string kd_bln { get; set; }
    }
}