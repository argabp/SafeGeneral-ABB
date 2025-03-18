using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Akuisisis.Queries
{
    public class GetAkuisisiQuery : IRequest<AkuisisiDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }

        public int kd_thn { get; set; }
    }

    public class GetAkuisisiQueryHandler : IRequestHandler<GetAkuisisiQuery, AkuisisiDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetAkuisisiQueryHandler> _logger;

        public GetAkuisisiQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetAkuisisiQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<AkuisisiDto> Handle(GetAkuisisiQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<AkuisisiDto>(@"SELECT * FROM rf40 WHERE kd_mtu = @kd_mtu AND kd_cob = @kd_cob AND
                                                                        kd_scob = @kd_scob AND kd_thn = @kd_thn", new
                {
                    request.kd_mtu, request.kd_cob, request.kd_scob, request.kd_thn
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