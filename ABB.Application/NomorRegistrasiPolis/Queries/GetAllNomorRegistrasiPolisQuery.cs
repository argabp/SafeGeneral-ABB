using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.NomorRegistrasiPolis.Queries
{
    public class GetAllNomorRegistrasiPolisQuery : IRequest<List<NomorRegistrasiPolisDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetAllNomorRegistrasiPolisQueryHandler : IRequestHandler<GetAllNomorRegistrasiPolisQuery, List<NomorRegistrasiPolisDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetAllNomorRegistrasiPolisQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<NomorRegistrasiPolisDto>> Handle(GetAllNomorRegistrasiPolisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var datas = (await _connectionFactory.Query<NomorRegistrasiPolisDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
				FROM uw01c p
					INNER JOIN rf01 cb
						ON p.kd_cb = cb.kd_cb
					INNER JOIN rf04 cob
						ON p.kd_cob = cob.kd_cob
					INNER JOIN rf05 scob
						ON p.kd_cob = scob.kd_cob
						AND p.kd_scob = scob.kd_scob
				WHERE (p.no_pol_ttg like '%'+@SearchKeyword+'%' 
					OR p.tgl_mul_ptg like '%'+@SearchKeyword+'%' 
					OR p.tgl_akh_ptg like '%'+@SearchKeyword+'%' 
					OR p.tgl_closing like '%'+@SearchKeyword+'%' 
					OR p.no_updt like '%'+@SearchKeyword+'%' 
					OR cb.nm_cb like '%'+@SearchKeyword+'%' 
					OR cob.nm_cob like '%'+@SearchKeyword+'%' 
					OR scob.nm_scob like '%'+@SearchKeyword+'%' 
					OR cb.kd_cb like '%'+@SearchKeyword+'%' 
					OR cob.kd_cob like '%'+@SearchKeyword+'%' 
					OR scob.kd_scob like '%'+@SearchKeyword+'%' 
					OR p.nm_ttg like '%'+@SearchKeyword+'%' 
					OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword })).ToList();

            int id = 0;
            foreach (var data in datas)
            {
	            id++;
	            data.Id = id;
            }
            
            return datas;
        }
    }
}