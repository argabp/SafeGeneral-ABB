using System.Threading.Tasks;
using ABB.Domain.IdentityModels;

namespace ABB.Application.Common.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        Task<AppUser> GetUser();
        Task<AppRole> GetRole();
        Task<string> GetRoleName();
    }
}