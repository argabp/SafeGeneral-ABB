using System.Collections.Generic;
using System.ComponentModel;

namespace ABB.Web.Modules.RoleRoute.Models
{
    public class EditRoleRouteModel
    {
        public EditRoleRouteModel()
        {
            Routes = new List<RouteModel>();
        }

        public string RoleId { get; set; }

        [DisplayName("Role Name")] public string RoleName { get; set; }

        public List<RouteModel> Routes { get; set; }
    }
}