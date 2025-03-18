using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.Alokasis.Queries
{
    public class GetDetailAlokasisQuery : IRequest<List<DetailAlokasiDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }
        
        public string kd_endt { get; set; }
    }

    public class GetDetailAlokasisQueryHandler : IRequestHandler<GetDetailAlokasisQuery, List<DetailAlokasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetDetailAlokasisQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<DetailAlokasiDto>> Handle(GetDetailAlokasisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<DetailAlokasiDto>(@"SELECT p.*
				FROM ri02e p
				WHERE p.kd_cb = @kd_cb AND 
				      p.kd_cob = @kd_cob AND 
				      p.kd_scob = @kd_scob AND 
				      p.kd_thn = @kd_thn AND 
				      p.no_pol = @no_pol AND 
				      p.no_updt = @no_updt AND
				      p.no_rsk = @no_rsk AND
				      p.kd_endt = @kd_endt", 
	            new { request.SearchKeyword, request.kd_cb, 
		            request.kd_cob, request.kd_scob, request.kd_thn,
		            request.no_pol, request.no_updt, request.no_rsk,
		            request.kd_endt
	            })).ToList();
        }
    }
}