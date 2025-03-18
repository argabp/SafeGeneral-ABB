using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Services;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Http;

namespace ABB.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _http;
        private readonly IUserManagerHelper _userManager;
        private readonly IRoleManagerHelper _roleManager;

        public CurrentUserService(IHttpContextAccessor http,
            IUserManagerHelper userManager, IRoleManagerHelper roleManager)
        {
            _http = http;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public string UserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserName => _http.HttpContext?.User.Identity.Name;

        public async Task<AppRole> GetRole()
        {
            var user = await GetUser();
            var roleName = (await _userManager.GetRolesAsync(user))?.FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }

        public async Task<AppUser> GetUser()
        {
            return await _userManager.GetUserAsync(_http.HttpContext.User);
        }

        public async Task<string> GetRoleName()
        {
            var user = await GetUser();
            var roleName = (await _userManager.GetRolesAsync(user))?.FirstOrDefault();
            return roleName;
        }
    }
}