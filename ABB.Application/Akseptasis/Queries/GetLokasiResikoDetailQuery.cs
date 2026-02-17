using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetLokasiResikoDetailQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }

        public string kd_lok_rsk { get; set; }
    }

    public class GetLokasiResikoDetailQueryHandler : IRequestHandler<GetLokasiResikoDetailQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLokasiResikoDetailQueryHandler> _logger;

        public GetLokasiResikoDetailQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLokasiResikoDetailQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetLokasiResikoDetailQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_81", new { request.kd_lok_rsk })).ToList();
            }, _logger);
        }
    }
}