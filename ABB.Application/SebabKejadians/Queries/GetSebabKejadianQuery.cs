using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.SebabKejadians.Queries
{
    public class GetSebabKejadianQuery : IRequest<SebabKejadianDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        
        public string kd_sebab { get; set; }
    }

    public class GetSebabKejadianQueryHandler : IRequestHandler<GetSebabKejadianQuery, SebabKejadianDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetSebabKejadianQueryHandler> _logger;

        public GetSebabKejadianQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetSebabKejadianQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<SebabKejadianDto> Handle(GetSebabKejadianQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<SebabKejadianDto>("SELECT * FROM rf34 WHERE kd_cob = @kd_cob AND kd_sebab = @kd_sebab",
                    new { request.kd_cob, request.kd_sebab })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}