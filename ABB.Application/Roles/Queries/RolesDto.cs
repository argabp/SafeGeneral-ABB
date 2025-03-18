using System.ComponentModel;

namespace ABB.Application.Roles.Queries
{
    public class RolesDto
    {
        public string RoleId { get; set; }
        public int WorkspaceId { get; set; }

        [DisplayName("Role Id")]
        public int RoleCode { get; set; }

        public string Description { get; set; }

        public string RoleName { get; set; }
    }
}