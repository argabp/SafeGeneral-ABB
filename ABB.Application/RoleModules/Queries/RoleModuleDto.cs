using System.Collections.Generic;

namespace ABB.Application.RoleModules.Queries
{
    public class RoleModuleDto
    {
        public string RoleId { get; set; }

        public List<int> Modules { get; set; }
    }
}