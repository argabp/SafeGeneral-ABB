using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;

namespace ABB.Application.MutasiKlaims.Queries
{
    public class GetMutasiKlaimDetailsQuery : IRequest<List<MutasiKlaimDetailDto>>
    {
        public string DatabaseName { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }
    }

    public class GetMutasiKlaimDetailsQueryHandler : IRequestHandler<GetMutasiKlaimDetailsQuery, List<MutasiKlaimDetailDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetMutasiKlaimDetailsQueryHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<MutasiKlaimDetailDto>> Handle(GetMutasiKlaimDetailsQuery request, CancellationToken cancellationToken)
        {
            _connectionFactory.CreateDbConnection(request.DatabaseName);
            return (await _connectionFactory.Query<MutasiKlaimDetailDto>(@"SELECT p.*, c.flag_pol_lama 
				FROM cl03 p
                    INNER JOIN cl01 c
                        ON c.kd_cb = p.kd_cb
                            AND  c.kd_cob = p.kd_cob
                            AND  c.kd_thn = p.kd_thn
                            AND  c.no_kl = p.no_kl
				WHERE p.kd_cb = @kd_cb 
				    AND p.kd_cob = @kd_cob 
				    AND p.kd_scob = @kd_scob 
				    AND p.kd_thn = @kd_thn 
				    AND p.no_kl = @no_kl 
				    ", new
            {
                request.kd_cb, request.kd_cob, request.kd_scob,
                request.kd_thn, request.no_kl
            })).ToList();
        }
    }
}