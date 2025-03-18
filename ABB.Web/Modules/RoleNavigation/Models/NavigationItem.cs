using System.Collections.Generic;

namespace ABB.Web.Modules.RoleNavigation.Models
{
    public class NavigationItem
    {
        public NavigationItem()
        {
            this.Navigation = new NavigationModel();
            this.SubNavigation = new List<NavigationModel>();
        }

        public string RoleId { get; set; }

        public NavigationModel Navigation { get; set; }

        public List<NavigationModel> SubNavigation { get; set; }
    }

    public class NavigationModel
    {
        public int NavigationId { get; set; }

        public string Text { get; set; }
    }
}