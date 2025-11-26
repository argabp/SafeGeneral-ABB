using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetKodeTahunQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }

        public DateTime tgl_reg { get; set; }
    }

    public class GetKodeTahunQueryHandler : IRequestHandler<GetKodeTahunQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKodeTahunQueryHandler> _logger;

        public GetKodeTahunQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKodeTahunQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetKodeTahunQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_cl01e_05", new
                {
                    request.tgl_reg
                })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}