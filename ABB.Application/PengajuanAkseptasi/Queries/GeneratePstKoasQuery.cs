using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GeneratePstKoasQuery : IRequest<decimal>
    {
        public string DatabaseName { get; set; }

        public decimal pst_share { get; set; }
    }

    public class GeneratePstKoasQueryHandler : IRequestHandler<GeneratePstKoasQuery, decimal>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GeneratePstKoasQueryHandler> _logger;

        public GeneratePstKoasQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GeneratePstKoasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<decimal> Handle(GeneratePstKoasQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<decimal>("sp_GeneratePstKoas", new { request.pst_share })).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}