using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akseptasis.Queries
{
    public class GetKodeOkupasiDetailQuery : IRequest<List<string>>
    {
        public string DatabaseName { get; set; }

        public string kd_zona { get; set; }
        
        public string kd_kls_konstr { get; set; }
        
        public string kd_okup { get; set; }
        
        public string kd_scob { get; set; }
    }

    public class GetKodeOkupasiDetailQueryHandler : IRequestHandler<GetKodeOkupasiDetailQuery, List<string>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeOkupasiDetailQueryHandler> _logger;

        public GetKodeOkupasiDetailQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeOkupasiDetailQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetKodeOkupasiDetailQuery request,
            CancellationToken cancellationToken)
        {
            return await ExceptionHelper.ExecuteWithLoggingAsync(async () =>
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_uw02e_51", new
                {
                    key = string.Empty, request.kd_zona, request.kd_kls_konstr,
                    request.kd_okup, request.kd_scob
                })).ToList();
            }, _logger);
        }
    }
}