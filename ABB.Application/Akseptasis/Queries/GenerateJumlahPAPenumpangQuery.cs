using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GenerateJumlahPAPenumpangQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }
        public Int16 jml_tmpt_ddk { get; set; }
    }

    public class GenerateJumlahPAPenumpangQueryHandler : IRequestHandler<GenerateJumlahPAPenumpangQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly ILogger<GenerateJumlahPAPenumpangQueryHandler> _logger;

        public GenerateJumlahPAPenumpangQueryHandler(IDbConnectionFactory connectionFactory,
            ILogger<GenerateJumlahPAPenumpangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GenerateJumlahPAPenumpangQuery request, CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw09e01_14", new { request.jml_tmpt_ddk })).FirstOrDefault();
            }, _logger);
        }
    }
}