namespace ABB.Application.Navigations.Queries
{
    public class LoggedInNavigationDto
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public int RouteId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
    }
}