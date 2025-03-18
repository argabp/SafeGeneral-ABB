using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.JenisSors.Queries
{
    public class GetJenisSorQuery : IRequest<JenisSorDto>
    {
        public string DatabaseName { get; set; }
        public string kd_jns_sor { get; set; }
    }

    public class GetJenisSorQueryHandler : IRequestHandler<GetJenisSorQuery, JenisSorDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetJenisSorQueryHandler> _logger;

        public GetJenisSorQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetJenisSorQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<JenisSorDto> Handle(GetJenisSorQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<JenisSorDto>("SELECT * FROM rf18 WHERE kd_jns_sor = @kd_jns_sor",
                    new { request.kd_jns_sor })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}