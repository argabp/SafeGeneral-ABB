namespace ABB.Domain.Entities
{
    public class Navigation
    {
        public int NavigationId { get; set; }
        public string Text { get; set; }
        public int ParentId { get; set; }
        public int RouteId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
    }
}