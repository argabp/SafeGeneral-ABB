using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetAkseptasiPranotaKoassQuery : IRequest<List<AkseptasiPranotaKoasDto>>
    {
        public string SearchKeyword { get; set; }
        public string DatabaseName { get; set; }
        public string KodeCabang { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetAkseptasiPranotaKoassQueryHandler : IRequestHandler<GetAkseptasiPranotaKoassQuery, List<AkseptasiPranotaKoasDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkseptasiPranotaKoassQueryHandler> _logger;

        public GetAkseptasiPranotaKoassQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GetAkseptasiPranotaKoassQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<AkseptasiPranotaKoasDto>> Handle(GetAkseptasiPranotaKoassQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<AkseptasiPranotaKoasDto>(@"SELECT r.nm_rk, p.*, p.kd_grp_pas + p.kd_rk_pas Id
                    FROM uw03a p
                        INNER JOIN rf03 r
                            ON p.kd_cb = r.kd_cb
                                AND p.kd_grp_pas = r.kd_grp_rk
                                AND p.kd_rk_pas = r.kd_rk
                    WHERE p.kd_cb = @KodeCabang AND 
                        p.kd_cob = @kd_cob AND 
                        p.kd_scob = @kd_scob AND 
                        p.kd_thn = @kd_thn AND 
                        p.no_aks = @no_aks AND 
                        p.no_updt = @no_updt AND
                        p.kd_mtu = @kd_mtu", 
                    new { request.SearchKeyword, request.KodeCabang, 
                        request.kd_cob, request.kd_scob, request.kd_thn,
                        request.no_aks, request.no_updt, request.kd_mtu
                    })).ToList();
            }, _logger);
        }
    }
}