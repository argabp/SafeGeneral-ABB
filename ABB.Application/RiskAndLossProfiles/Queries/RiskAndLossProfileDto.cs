namespace ABB.Application.RiskAndLossProfiles.Queries
{
    public class RiskAndLossProfileDto
    {
        public string Id { get; set; }
        
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public int nomor { get; set; }

        public decimal bts1 { get; set; }
        
        public decimal bts2 { get; set; }
    }
}