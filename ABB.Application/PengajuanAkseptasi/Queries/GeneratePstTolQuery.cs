using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GeneratePstTolQuery : IRequest<decimal?>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_tol { get; set; }
    }

    public class GeneratePstTolQueryHandler : IRequestHandler<GeneratePstTolQuery, decimal?>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GeneratePstTolQueryHandler> _logger;

        public GeneratePstTolQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GeneratePstTolQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<decimal?> Handle(GeneratePstTolQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<decimal?>("sp_GeneratePstTol", new { request.kd_cob, request.kd_tol })).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}