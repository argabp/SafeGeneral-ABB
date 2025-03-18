using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.UserCabangs.Queries
{
    public class GetViewUserCabangQuery : IRequest<List<UserCabangDto>>
    {
    }

    public class GetViewUserCabangQueryHandler : IRequestHandler<GetViewUserCabangQuery, List<UserCabangDto>>
    {
        private readonly IDbConnection _connection;

        public GetViewUserCabangQueryHandler(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<UserCabangDto>> Handle(GetViewUserCabangQuery request, CancellationToken cancellationToken)
        {
            return (await _connection.Query<UserCabangDto>(@"SELECT uc.userid, u.FirstName + ' ' + u.LastName username,
                                                                (SELECT DISTINCT T1.results FROM
                                                                (
                                                                select REPLACE(STUFF(CAST((
                                                                                    SELECT   ', ' +CAST(nm_cb AS VARCHAR(MAX))
                                                                                    FROM (  
                                                                                            SELECT distinct nm_cb
                                                                                            FROM (SELECT DISTINCT sub.userid, r.nm_cb From MS_UserCabang sub
									                                                                INNER JOIN rf01 r ON sub.kd_cb = r.kd_cb) b
							                                                                WHERE b.userid = uc.userid
                                                                                        ) c
                                                                                    FOR XML PATH(''), TYPE) AS VARCHAR(MAX)), 1, 2, ''),' ',' ') AS results
                                                                                    from (SELECT DISTINCT sub.userid, r.nm_cb From MS_UserCabang sub
									                                                                INNER JOIN rf01 r ON sub.kd_cb = r.kd_cb) t) T1) AS nm_cb
                                                                FROM MS_UserCabang uc
	                                                                INNER JOIN MS_User u
		                                                                ON uc.userid = u.UserId
	                                                                GROUP BY uc.userid, u.FirstName, u.LastName"
            )).ToList();
        }
    }
}