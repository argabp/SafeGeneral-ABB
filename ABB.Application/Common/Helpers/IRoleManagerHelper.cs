using System.Threading.Tasks;
using ABB.Domain.IdentityModels;

namespace ABB.Application.Common.Helpers
{
    public interface IRoleManagerHelper
    {
        Task<bool> CreateAsync(AppRole role);
        Task<bool> UpdateAsync(AppRole role);
        Task<bool> DeleteAsync(AppRole role);
        Task<AppRole> FindByNameAsync(string name);
    }
}