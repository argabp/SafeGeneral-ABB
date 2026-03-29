using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KlaimAlokasiReasuransis.Queries
{
    public class GenerateNilaiKlaimQuery : IRequest<string>
    {
        public decimal nilai_ttl_kl { get; set; }
        public decimal pst_share { get; set; }
    }

    public class GenerateNilaiKlaimQueryHandler : IRequestHandler<GenerateNilaiKlaimQuery, string>
    {
        private readonly IDbConnectionPst _connectionPst;
        private readonly ILogger<GenerateNilaiKlaimQueryHandler> _logger;

        public GenerateNilaiKlaimQueryHandler(IDbConnectionPst connectionPst,
            ILogger<GenerateNilaiKlaimQueryHandler> logger)
        {
            _connectionPst = connectionPst;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiKlaimQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                return (await _connectionPst.QueryProc<string>("spe_cl02e_05", 
                    new { request.nilai_ttl_kl, request.pst_share })).FirstOrDefault();
            }, _logger);
        }
    }
}