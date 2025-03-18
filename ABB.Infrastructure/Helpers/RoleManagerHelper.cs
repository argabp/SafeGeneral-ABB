using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Infrastructure.Helpers
{
    public class RoleManagerHelper : IRoleManagerHelper
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleManagerHelper(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(AppRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(AppRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<AppRole> FindByNameAsync(string name)
        {
            var result = await _roleManager.FindByNameAsync(name);
            return result;
        }

        public async Task<bool> UpdateAsync(AppRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}