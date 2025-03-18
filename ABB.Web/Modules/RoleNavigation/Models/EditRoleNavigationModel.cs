using System.Collections.Generic;
using System.ComponentModel;

namespace ABB.Web.Modules.RoleNavigation.Models
{
    public class EditRoleNavigationModel
    {
        public EditRoleNavigationModel()
        {
            Navigations = new List<NavigationItem>();
        }

        [DisplayName("Role Name")] public string RoleId { get; set; }

        public List<NavigationItem> Navigations { get; set; }
        public string Username { get; set; }
    }
}