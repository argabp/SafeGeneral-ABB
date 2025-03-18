using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKendaraanOJKs.Queries
{
    public class GetTarifKendaraanOJKQuery : IRequest<TarifKendaraanOJKDto>
    {
        public string DatabaseName { get; set; }
        public string kd_kategori { get; set; }

        public string kd_jns_ptg { get; set; }

        public string kd_wilayah { get; set; }

        public byte no_kategori { get; set; }
    }

    public class GetTarifKendaraanOJKQueryHandler : IRequestHandler<GetTarifKendaraanOJKQuery, TarifKendaraanOJKDto>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetTarifKendaraanOJKQueryHandler> _logger;

        public GetTarifKendaraanOJKQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetTarifKendaraanOJKQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<TarifKendaraanOJKDto> Handle(GetTarifKendaraanOJKQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<TarifKendaraanOJKDto>(@"SELECT * FROM rf15d01 WHERE kd_kategori = @kd_kategori AND kd_jns_ptg = @kd_jns_ptg 
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