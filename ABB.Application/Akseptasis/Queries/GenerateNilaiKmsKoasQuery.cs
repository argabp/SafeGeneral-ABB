using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiKmsKoasQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal pst_kms { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal nilai_dis { get; set; }
    }

    public class GenerateNilaiKmsKoasQueryHandler : IRequestHandler<GenerateNilaiKmsKoasQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiKmsKoasQueryHandler> _logger;

        public GenerateNilaiKmsKoasQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiKmsKoasQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiKmsKoasQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_12", 
                    new { request.pst_kms, request.nilai_prm, request.nilai_dis })).FirstOrDefault();
            }, _logger);
        }
    }
}