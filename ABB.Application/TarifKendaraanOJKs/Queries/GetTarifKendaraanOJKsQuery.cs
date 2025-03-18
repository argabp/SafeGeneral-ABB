using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKendaraanOJKs.Queries
{
    public class GetTarifKendaraanOJKsQuery : IRequest<List<TarifKendaraanOJKDto>>
    {
        public string kd_kategori { get; set; }
        public string DatabaseName { get; set; }
    }

    public class GetTarifKendaraanOJKsQueryHandler : IRequestHandler<GetTarifKendaraanOJKsQuery, List<TarifKendaraanOJKDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetTarifKendaraanOJKsQueryHandler> _logger;

        public GetTarifKendaraanOJKsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetTarifKendaraanOJKsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<TarifKendaraanOJKDto>> Handle(GetTarifKendaraanOJKsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<TarifKendaraanOJKDto>(@"SELECT RTRIM(LTRIM(b.kd_kategori)) + RTRIM(LTRIM(b.kd_jns_ptg)) + 
                                                                        RTRIM(LTRIM(b.kd_wilayah)) + RTRIM(LTRIM(b.no_kategori)) Id ,
                                                                        b.* FROM rf15d01 b WHERE b.kd_kategori = @kd_kategori", 
                    new { request.kd_kategori })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}