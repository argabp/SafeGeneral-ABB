using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.EntriNotas.Queries
{
    public class GetEntriNotaCancelsQuery : IRequest<List<NotaDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_pol { get; set; }
        public Int16 no_updt { get; set; }
    }

    public class GetEntriNotaCancelsQueryHandler : IRequestHandler<GetEntriNotaCancelsQuery, List<NotaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetEntriNotaCancelsQueryHandler> _logger;

        public GetEntriNotaCancelsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetEntriNotaCancelsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<NotaDto>> Handle(GetEntriNotaCancelsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<NotaDto>(@"SELECT pp.nm_ttg, pp.st_pas, p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob FROM uw08e p 
                        INNER JOIN rf01 cb
                           ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob
                           ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob
                           ON p.kd_cob = scob.kd_cob
                           AND p.kd_scob = scob.kd_scob
                        INNER JOIN uw01e pp
                            ON pp.kd_cb = p.kd_cb
                            AND pp.kd_cob = p.kd_cob
                            AND pp.kd_scob = p.kd_scob
                            AND pp.kd_thn = p.kd_thn
                            AND pp.no_pol = p.no_pol
                            AND pp.no_updt = p.no_updt
                        WHERE p.flag_cancel = 'Y' AND p.kd_cb = @kd_cb
                               AND p.kd_cob = @kd_cob AND p.kd_scob = @kd_scob
                               AND p.kd_thn = @kd_thn AND p.no_pol = @no_pol
                               AND p.no_updt = @no_updt", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob,
                    request.no_pol, request.kd_thn, request.no_updt
                })).ToList();

                var sequence = 1;
                foreach (var result in results)
                {
                    result.Id = sequence;
                    sequence++;
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}