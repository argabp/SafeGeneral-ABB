using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class GetAkseptasiDataQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }

        public string no_pol_ttg { get; set; }
    }

    public class GetAkseptasiDataQueryHandler : IRequestHandler<GetAkseptasiDataQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkseptasiDataQueryHandler> _logger;

        public GetAkseptasiDataQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAkseptasiDataQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetAkseptasiDataQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);

                return (await _connectionFactory.QueryProc<string>("sp_GenerateAkseptasi",
                    new { request.no_pol_ttg })).ToList();
            }, _logger);
        }
    }
}