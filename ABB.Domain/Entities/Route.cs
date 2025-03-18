namespace ABB.Domain.Entities
{
    public class Route
    {
        public int RouteId { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool IsActive { get; set; }
    }
}