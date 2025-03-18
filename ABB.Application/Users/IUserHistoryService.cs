using System.Threading.Tasks;
using ABB.Application.Users.Queries;

namespace ABB.Application.Users
{
    public interface IUserHistoryService
    {
        void AddUserHistory(UserHistoryDto dto);
        Task<int> Commit();
    }
}