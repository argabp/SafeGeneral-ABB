namespace ABB.Application.RoleRoutes.Queries
{
    public class RoutesDto
    {
        public int RouteId { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public bool Active { get; set; }
    }
}