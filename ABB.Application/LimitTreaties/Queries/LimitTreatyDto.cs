namespace ABB.Application.LimitTreaties.Queries
{
    public class LimitTreatyDto
    {
        public string Id { get; set; }
        
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_tol { get; set; }

        public string? nm_tol { get; set; }

        public string? kd_sub_grp { get; set; }
    }
}