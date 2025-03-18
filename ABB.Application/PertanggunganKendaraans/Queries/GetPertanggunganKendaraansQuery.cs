using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class GetPertanggunganKendaraansQuery : IRequest<List<PertanggunganKendaraanDto>>
    {
        public string DatabaseName { get; set; }
    }

    public class GetPertanggunganKendaraansQueryHandler : IRequestHandler<GetPertanggunganKendaraansQuery, List<PertanggunganKendaraanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPertanggunganKendaraansQueryHandler> _logger;

        public GetPertanggunganKendaraansQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPertanggunganKendaraansQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<PertanggunganKendaraanDto>> Handle(GetPertanggunganKendaraansQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PertanggunganKendaraanDto>("SELECT RTRIM(LTRIM(d.kd_cob)) + RTRIM(LTRIM(d.kd_scob)) + RTRIM(LTRIM(d.kd_jns_ptg)) AS Id, " +
                                                                     "d.*, r.nm_cob FROM dp01 d INNER JOIN rf04 r ON d.kd_cob = r.kd_cob")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}