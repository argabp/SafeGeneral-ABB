using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Roles.Queries
{
    public class GetUsersQuery : IRequest<List<UsersDto>>
    {
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
            var sql = @"SELECT  UserId, 
                                ISNULL(FirstName,'') +' '+ ISNULL(LastName,'') FullName,
                                Case When Photo like '%default%' Then '/img/' + Photo Else '/img/profile-picture/' + Photo End Photo 
                        FROM MS_User
                        WHERE ISNULL(IsDeleted,0) = 0";
            var result = await _db.Query<UsersDto>(sql);
            return result.ToList();
        }
    }
}