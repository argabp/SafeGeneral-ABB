using System.Threading.Tasks;
using ABB.Domain.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace ABB.Infrastructure.Validators
{
    public class CustomRoleValidator : RoleValidator<AppRole>
    {
        public override async Task<IdentityResult> ValidateAsync(RoleManager<AppRole> manager, AppRole role)
        {
            var roleName = await manager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RoleNameIsNotValid",
                    Description = "Role Name is not valid!"
                });
            }

            return IdentityResult.Success;
        }
    }
}