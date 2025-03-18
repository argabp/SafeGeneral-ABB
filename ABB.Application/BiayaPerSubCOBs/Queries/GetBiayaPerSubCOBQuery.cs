using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.BiayaPerSubCOBs.Queries
{
    public class GetBiayaPerSubCOBQuery : IRequest<BiayaPerSubCOBDto>
    {
        public string DatabaseName { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }
    }

    public class GetBiayaPerSubCOBQueryHandler : IRequestHandler<GetBiayaPerSubCOBQuery, BiayaPerSubCOBDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetBiayaPerSubCOBQueryHandler> _logger;

        public GetBiayaPerSubCOBQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetBiayaPerSubCOBQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<BiayaPerSubCOBDto> Handle(GetBiayaPerSubCOBQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<BiayaPerSubCOBDto>(@"SELECT * FROM dp03 WHERE kd_mtu = @kd_mtu AND kd_cob = @kd_cob AND
                                                                        kd_scob = @kd_scob", new
                {
                    request.kd_mtu, request.kd_cob, request.kd_scob
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