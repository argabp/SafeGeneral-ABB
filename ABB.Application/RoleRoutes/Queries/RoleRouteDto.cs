using System.ComponentModel;

namespace ABB.Application.RoleRoutes.Queries
{
    public class RoleRouteDto
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        [DisplayName("Controller Name")] public string Controller { get; set; }

        [DisplayName("Route Name")] public string Action { get; set; }
    }
}