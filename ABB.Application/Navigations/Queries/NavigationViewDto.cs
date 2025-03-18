namespace ABB.Application.Navigations.Queries
{
    public class NavigationViewDto
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string IsActive { get; set; }
        public string SubNavigation { get; set; }
    }
}