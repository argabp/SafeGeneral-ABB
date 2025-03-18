using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifPerluasanKendaraanOJKs.Queries
{
    public class GetKategorisQuery : IRequest<List<KategoriDto>>
    {
        public string DatabaseName { get; set; }
        public string SearchKeyword { get; set; }
    }

    public class GetKategorisQueryHandler : IRequestHandler<GetKategorisQuery, List<KategoriDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKategorisQueryHandler> _logger;

        public GetKategorisQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKategorisQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KategoriDto>> Handle(GetKategorisQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KategoriDto>(@"SELECT RTRIM(LTRIM(kd_kategori)) kd_kategori, RTRIM(LTRIM(nm_kategori)) nm_kategori FROM v_rf10_02")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}