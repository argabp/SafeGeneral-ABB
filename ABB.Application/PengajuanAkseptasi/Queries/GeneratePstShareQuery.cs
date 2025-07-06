using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GeneratePstShareQuery : IRequest<decimal>
    {
        public string DatabaseName { get; set; }

        public string st_pas { get; set; }
    }

    public class GeneratePstShareQueryHandler : IRequestHandler<GeneratePstShareQuery, decimal>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GeneratePstShareQueryHandler> _logger;

        public GeneratePstShareQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GeneratePstShareQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<decimal> Handle(GeneratePstShareQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<decimal>("sp_GenerateShare", new { request.st_pas })).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}