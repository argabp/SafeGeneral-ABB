using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class GetPertanggunganKendaraanQuery : IRequest<PertanggunganKendaraanDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }
    }

    public class GetPertanggunganKendaraanQueryHandler : IRequestHandler<GetPertanggunganKendaraanQuery, PertanggunganKendaraanDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetPertanggunganKendaraanQueryHandler> _logger;

        public GetPertanggunganKendaraanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetPertanggunganKendaraanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<PertanggunganKendaraanDto> Handle(GetPertanggunganKendaraanQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<PertanggunganKendaraanDto>("SELECT d.* " +
                    "FROM dp01 d WHERE d.kd_cob = @kd_cob AND d.kd_scob = @kd_scob AND d.kd_jns_ptg = @kd_jns_ptg",
                    new { request.kd_cob, request.kd_scob, request.kd_jns_ptg })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}