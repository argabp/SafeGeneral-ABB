using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetCopyEndorsQuery : IRequest<List<CopyEndorsDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
    }

    public class GetCopyEndorsQueryHandler : IRequestHandler<GetCopyEndorsQuery, List<CopyEndorsDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetCopyEndorsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<CopyEndorsDto>> Handle(GetCopyEndorsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            var datas = (await _connectionFactory.Query<CopyEndorsDto>(@"SELECT * FROM uw04c WHERE kd_cb = @kd_cb AND 
                           kd_cob = @kd_cob AND kd_scob = @kd_scob AND kd_thn = @kd_thn AND 
                           no_pol = @no_pol", new
            {
                request.kd_cb, request.kd_cob, request.kd_scob, 
                request.kd_thn, request.no_pol
            })).ToList();

            int sequence = 0;
            foreach (var data in datas)
            {
	            sequence++;
	            data.Id = sequence;
            }

            return datas;
            //         return (await _connectionFactory.Query<AkseptasiResikoDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob
            // FROM uw04a p
            // 	INNER JOIN rf01 cb
            // 		ON p.kd_cb = cb.kd_cb
            // 	INNER JOIN rf04 cob
            // 		ON p.kd_cob = cob.kd_cob
            // 	INNER JOIN rf05 scob
            // 		ON p.kd_scob = scob.kd_scob
            // WHERE cb.kd_cb = @KodeCabang AND (p.no_aks like '%'+@SearchKeyword+'%' 
            // 	OR p.no_pol_pas like '%'+@SearchKeyword+'%' 
            // 	OR p.st_pas like '%'+@SearchKeyword+'%' 
            // 	OR cb.nm_cb like '%'+@SearchKeyword+'%' 
            // 	OR cob.nm_cob like '%'+@SearchKeyword+'%' 
            // 	OR scob.nm_scob like '%'+@SearchKeyword+'%' 
            // 	OR p.nm_ttg like '%'+@SearchKeyword+'%' 
            // 	OR @SearchKeyword = '' OR @SearchKeyword IS NULL)", new { request.SearchKeyword, request.KodeCabang })).ToList();
        }
    }
}