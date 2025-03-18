using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Kapals.Queries
{
    public class GetKapalQuery : IRequest<KapalDto>
    {
        public string DatabaseName { get; set; }
        public string kd_kapal { get; set; }
    }

    public class GetKapalQueryHandler : IRequestHandler<GetKapalQuery, KapalDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKapalQueryHandler> _logger;

        public GetKapalQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKapalQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<KapalDto> Handle(GetKapalQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KapalDto>(@"SELECT kd_kapal, nm_kapal, RTRIM(LTRIM(merk_kapal)) merk_kapal,
                                                                kd_negara, thn_buat, grt, st_class, no_reg,
                                                                no_imo, ekuitas FROM rf30 WHERE kd_kapal = @kd_kapal",
                    new { request.kd_kapal })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}