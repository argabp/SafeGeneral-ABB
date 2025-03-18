using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.UserCabangs.Queries
{
    public class GetUserCabangsQuery : IRequest<List<UserCabangDto>>
    {
    }

    public class GetUserCabangsQueryHandler : IRequestHandler<GetUserCabangsQuery, List<UserCabangDto>>
    {
        private readonly IDbConnection _connection;

        public GetUserCabangsQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<UserCabangDto>> Handle(GetUserCabangsQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<UserCabangDto>(@"SELECT uc.userid, u.FirstName + ' ' + u.LastName username,
		uc.kd_cb, c.nm_cb FROM MS_UserCabang uc
	INNER JOIN MS_User u
		ON uc.userid = u.UserId
	INNER JOIN rf01 c
		ON uc.kd_cb = c.kd_cb"
                )).ToList();
        }
    }
}