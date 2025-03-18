using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.MataUangs.Queries
{
    public class GetDetailMataUangQuery : IRequest<List<DetailMataUangDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_mtu { get; set; }
    }

    public class GetDetailMataUangQueryHandler : IRequestHandler<GetDetailMataUangQuery, List<DetailMataUangDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailMataUangQueryHandler> _logger;

        public GetDetailMataUangQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailMataUangQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailMataUangDto>> Handle(GetDetailMataUangQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DetailMataUangDto>("SELECT kd_mtu + CONVERT(VARCHAR, tgl_mul, 126) + CONVERT(VARCHAR, tgl_akh, 126) AS Id," +
                                                                          "kd_mtu, tgl_mul, tgl_akh, nilai_kurs FROM rf06d WHERE kd_mtu = @kd_mtu",
                    new { request.kd_mtu })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}