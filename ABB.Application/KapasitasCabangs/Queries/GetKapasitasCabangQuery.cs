using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.KapasitasCabangs.Queries
{
    public class GetKapasitasCabangQuery : IRequest<KapasitasCabangDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }
    }

    public class GetKapasitasCabangQueryHandler : IRequestHandler<GetKapasitasCabangQuery, KapasitasCabangDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKapasitasCabangQueryHandler> _logger;

        public GetKapasitasCabangQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKapasitasCabangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<KapasitasCabangDto> Handle(GetKapasitasCabangQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KapasitasCabangDto>(@"SELECT * FROM rf43 WHERE kd_cb = @kd_cb AND kd_cob = @kd_cob AND
                                                                        kd_scob = @kd_scob AND thn = @thn", new
                {
                    request.kd_cb, request.kd_cob, request.kd_scob, request.thn
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