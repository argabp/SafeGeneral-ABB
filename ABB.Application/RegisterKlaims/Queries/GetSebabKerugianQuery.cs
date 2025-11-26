using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.RegisterKlaims.Queries
{
    public class GetSebabKerugianQuery : IRequest<string>
    {
        public string DatabaseName { get; set; }

        public string kd_cob { get; set; }
        
        public string kd_sebab { get; set; }
    }

    public class GetSebabKerugianQueryHandler : IRequestHandler<GetSebabKerugianQuery, string>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetSebabKerugianQueryHandler> _logger;

        public GetSebabKerugianQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetSebabKerugianQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<string> Handle(GetSebabKerugianQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.QueryProc<string>("spe_cl01e_10", new
                {
                    request.kd_cob, request.kd_sebab
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