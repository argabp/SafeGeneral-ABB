namespace ABB.Application.LaporanBulananFakultatifs.Commands
{
    public class LaporanBulananFakultatifModel
    {
        public string cob { get; set; }

        public string ceding { get; set; }

        public string no_nota { get; set; }

        public string no_polis { get; set; }

        public string nm_ttg { get; set; }

        public string mtu { get; set; }

        public decimal gross_premi { get; set; }

        public decimal komisi { get; set; }

        public decimal klaim { get; set; }

        public decimal netto { get; set; }
    }
}