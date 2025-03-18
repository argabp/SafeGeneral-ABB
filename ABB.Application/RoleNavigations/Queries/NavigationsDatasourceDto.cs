namespace ABB.Application.RoleNavigations.Queries
{
    public class NavigationsDatasourceDto
    {
        public string RoleId { get; set; }

        public int NavigationId { get; set; }

        public int ParentId { get; set; }

        public string Text { get; set; }
    }
}