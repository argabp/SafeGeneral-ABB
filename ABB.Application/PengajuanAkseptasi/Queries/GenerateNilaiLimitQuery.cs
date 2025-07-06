using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GenerateNilaiLimitQuery : IRequest<decimal>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }

        public string kd_tol { get; set; }

        public decimal pst_share { get; set; }

        public decimal nilai_ttl_ptg { get; set; }
    }

    public class GenerateNilaiLimitQueryHandler : IRequestHandler<GenerateNilaiLimitQuery, decimal>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GenerateNilaiLimitQueryHandler> _logger;

        public GenerateNilaiLimitQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GenerateNilaiLimitQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<decimal> Handle(GenerateNilaiLimitQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<decimal>("sp_GenerateNilaiLimit", new
                {
                    request.kd_cob, request.kd_tol, request.pst_share, request.nilai_ttl_ptg
                })).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}