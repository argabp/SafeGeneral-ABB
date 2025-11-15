using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GetMutasiKlaimAlokasisQuery : IRequest<List<MutasiKlaimAlokasiDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
    }

    public class GetMutasiKlaimAlokasisQueryHandler : IRequestHandler<GetMutasiKlaimAlokasisQuery, List<MutasiKlaimAlokasiDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetMutasiKlaimAlokasisQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<MutasiKlaimAlokasiDto>> Handle(GetMutasiKlaimAlokasisQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<MutasiKlaimAlokasiDto>(@"SELECT p.* 
				FROM cl05 p
				WHERE p.kd_cb = @kd_cb 
				    AND p.kd_cob = @kd_cob 
				    AND p.kd_scob = @kd_scob 
				    AND p.kd_thn = @kd_thn 
				    AND p.no_kl = @no_kl 
				    AND p.no_mts = @no_mts 
				    ", new
            {
                request.kd_cb, request.kd_cob, request.kd_scob,
                request.kd_thn, request.no_kl, request.no_mts
            })).ToList();
        }
    }
}