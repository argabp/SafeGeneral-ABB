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
    public class GetEntriNotasQuery : IRequest<List<NotaDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetEntriNotasQueryHandler : IRequestHandler<GetEntriNotasQuery, List<NotaDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetEntriNotasQueryHandler> _logger;

        public GetEntriNotasQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetEntriNotasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<NotaDto>> Handle(GetEntriNotasQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var results = (await _connectionFactory.Query<NotaDto>(@"SELECT p.*, cb.nm_cb, cob.nm_cob, scob.nm_scob, aks.no_pol_pas FROM uw08e p 
                        INNER JOIN rf01 cb
                           ON p.kd_cb = cb.kd_cb
                        INNER JOIN rf04 cob
                           ON p.kd_cob = cob.kd_cob
                        INNER JOIN rf05 scob
                           ON p.kd_cob = scob.kd_cob
                           AND p.kd_scob = scob.kd_scob
                        inner join uw01e aks
                           on p.kd_cb=aks.kd_cb
                           and p.kd_cob=aks.kd_cob
                           and p.kd_scob=aks.kd_scob
                           and p.kd_thn=aks.kd_thn
                           and p.no_pol=aks.no_pol
                           and p.no_updt=aks.no_updt
                        WHERE p.flag_cancel = 'N'")).ToList();

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