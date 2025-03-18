using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Infrastructure.Helpers
{
    public class SignInManagerHelper : ISignInManagerHelper
    {
        private readonly SignInManager<AppUser> _signIn;

        public SignInManagerHelper(SignInManager<AppUser> signIn)
        {
            _signIn = signIn;
        }

        public async Task<bool> PasswordSignInAsync(AppUser user, string password, bool isPersistent = true,
            bool lockoutOnFailure = false)
        {
            var result = await _signIn.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signIn.SignOutAsync();
        }
    }
}