using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UsersDto>>
    {
        public string SearchKeyword { get; set; }
    }

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UsersDto>>
    {
        private readonly IDbConnection _db;
        public GetUsersHandler(IDbConnection db)
        {
            _db = db;
        }
        public async Task<List<UsersDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.QueryProc<UsersDto>("sp_USR_GetUsers", new { request.SearchKeyword });
            return result.ToList();
        }
    }
}