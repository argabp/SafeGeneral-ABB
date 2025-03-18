using System.Threading.Tasks;
using ABB.Domain.IdentityModels;

namespace ABB.Application.Common.Services
{
    public interface ILockOutService
    {
        Task CheckingLockOut(AppUser user);
        Task UpdateAccessFailedCount(AppUser user, bool validPassword);
    }
}