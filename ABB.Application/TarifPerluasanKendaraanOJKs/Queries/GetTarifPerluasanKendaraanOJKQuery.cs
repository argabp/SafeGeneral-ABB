using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifPerluasanKendaraanOJKs.Queries
{
    public class GetTarifPerluasanKendaraanOJKQuery : IRequest<TarifPerluasanKendaraanOJKDto>
    {
        public string DatabaseName { get; set; }
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }

        public byte no_kategori { get; set; }
    }

    public class GetTarifPerluasanKendaraanOJKQueryHandler : IRequestHandler<GetTarifPerluasanKendaraanOJKQuery, TarifPerluasanKendaraanOJKDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetTarifPerluasanKendaraanOJKQueryHandler> _logger;

        public GetTarifPerluasanKendaraanOJKQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetTarifPerluasanKendaraanOJKQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<TarifPerluasanKendaraanOJKDto> Handle(GetTarifPerluasanKendaraanOJKQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<TarifPerluasanKendaraanOJKDto>(@"SELECT * FROM rf15d01 WHERE kd_kategori = @kd_kategori AND kd_jns_ptg = @kd_jns_ptg 
                                                                            AND kd_wilayah = @kd_wilayah AND no_kategori = @no_kategori", new
                {
                    request.kd_kategori, request.kd_jns_ptg, request.kd_wilayah, request.no_kategori
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