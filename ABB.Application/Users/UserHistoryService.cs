using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using ABB.Application.Users.Queries;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Users
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public UserHistoryService(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddUserHistory(UserHistoryDto dto)
        {
            var userHistory = _mapper.Map<UserHistory>(dto);
            _context.UserHistory.Add(userHistory);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}