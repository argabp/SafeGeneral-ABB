using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.CetakLampiranEndorsments.Queries
{
    public class GetCetakLampiranEndorsmentsQuery : IRequest<List<CetakLampiranEndorsmentDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
    }

    public class GetCetakLampiranEndorsmentsQueryHandler : IRequestHandler<GetCetakLampiranEndorsmentsQuery, List<CetakLampiranEndorsmentDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCetakLampiranEndorsmentsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CetakLampiranEndorsmentDto>> Handle(GetCetakLampiranEndorsmentsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<CetakLampiranEndorsmentDto>(@"SELECT 
    		RTRIM(LTRIM(p.kd_cb)) + RTRIM(LTRIM(p.kd_cob)) + RTRIM(LTRIM(p.kd_scob)) + RTRIM(LTRIM(p.kd_thn)) + RTRIM(LTRIM(p.no_pol)) + CONVERT(varchar(max), p.no_updt) Id,
    		p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob, u.Username nm_usr_input
				FROM uw01e p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
					LEFT JOIN MS_User u
						ON u.UserId = p.kd_usr_input
				WHERE cb.kd_cb = @KodeCabang AND p.no_updt > 0 AND (p.no_pol like '%'+@SearchKeyword+'%' 
					OR p.no_pol_ttg like '%'+@SearchKeyword+'%' 
					OR p.no_updt like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR cob.kd_cob like '%'+@SearchKeyword+'%' 
					OR scob.kd_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_ttg like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}