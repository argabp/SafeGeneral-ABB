using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateNilaiKomisiQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public decimal nilai_prm { get; set; }
        public decimal nilai_dis { get; set; }
        public decimal pst_kms { get; set; }
    }

    public class GenerateNilaiKomisiQueryHandler : IRequestHandler<GenerateNilaiKomisiQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateNilaiKomisiQueryHandler> _logger;

        public GenerateNilaiKomisiQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateNilaiKomisiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateNilaiKomisiQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_22", 
                    new { request.nilai_prm, request.nilai_dis, request.pst_kms })).FirstOrDefault();
            }, _logger);
        }
    }
}