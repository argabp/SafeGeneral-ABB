using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ABB.Domain.IdentityModels;

namespace ABB.Application.Common.Helpers
{
    public interface IUserManagerHelper
    {
        Task<bool> CreateAsync(AppUser user, string password);
        Task<bool> AddToRoleAsync(AppUser user, string role);
        Task<bool> UpdateAsync(AppUser user);
        Task<bool> RemoveFromRolesAsync(AppUser user, IEnumerable<string> userRole);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<AppUser> FindByIdAsync(string id);
        Task<AppUser> FindByUsernameAsync(string username);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<bool> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<bool> DeleteAsync(AppUser user);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
    }
}