using System.Threading.Tasks;
using ABB.Domain.IdentityModels;

namespace ABB.Application.Common.Helpers
{
    public interface ISignInManagerHelper
    {
        Task<bool> PasswordSignInAsync(AppUser user, string password, bool isPersistent = true,
            bool lockoutOnFailure = false);

        Task SignOutAsync();
    }
}