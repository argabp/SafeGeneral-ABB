using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.DokumenKlaims.Queries
{
    public class GetDokumenKlaimQuery : IRequest<DokumenKlaimDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }
        
        public string kd_dok { get; set; }
    }

    public class GetDokumenKlaimQueryHandler : IRequestHandler<GetDokumenKlaimQuery, DokumenKlaimDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDokumenKlaimQueryHandler> _logger;

        public GetDokumenKlaimQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDokumenKlaimQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<DokumenKlaimDto> Handle(GetDokumenKlaimQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DokumenKlaimDto>("SELECT * FROM dp20 WHERE kd_cob = @kd_cob AND kd_dok = @kd_dok",
                    new { request.kd_cob, request.kd_dok })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}