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
            await Task.Delay(0, cancellationToken);
            
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<NotaDto>("SELECT RTRIM(LTRIM(kd_cb)) + RTRIM(LTRIM(jns_tr)) " +
                                                                "+ RTRIM(LTRIM(jns_nt_msk)) + RTRIM(LTRIM(kd_thn)) + " +
                                                                "RTRIM(LTRIM(kd_bln)) + RTRIM(LTRIM(no_nt_msk)) + " +
                                                                "RTRIM(LTRIM(jns_nt_kel)) + RTRIM(LTRIM(no_nt_kel)) Id, * FROM uw08e")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}