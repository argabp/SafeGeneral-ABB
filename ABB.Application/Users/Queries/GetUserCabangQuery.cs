using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using MediatR;

namespace ABB.Application.Users.Queries
{
    public class GetUserCabangQuery : IRequest<List<DropdownOptionDto>>
    {
        public string Username { get; set; }
    }

    public class GetUserCabangQueryHandler : IRequestHandler<GetUserCabangQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnection _dbConnection;

        public GetUserCabangQueryHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetUserCabangQuery request, CancellationToken cancellationToken)
        {
            return (await _dbConnection.Query<DropdownOptionDto>(@"SELECT uc.kd_cb Value, c.nm_cb Text From MS_UserCabang uc
                               INNER JOIN rf01 c
                                    ON uc.kd_cb = c.kd_cb
                                INNER JOIN MS_User u
                                    ON uc.UserId = u.UserId
                               WHERE u.Username = @Username", new { request.Username })).ToList();
        }
    }
}