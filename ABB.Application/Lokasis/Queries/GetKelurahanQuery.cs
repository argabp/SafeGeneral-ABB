using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lokasis.Queries
{
    public class GetKelurahanQuery : IRequest<List<KelurahanDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }

        public string kd_kec { get; set; }
    }

    public class GetKelurahanQueryHandler : IRequestHandler<GetKelurahanQuery, List<KelurahanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKelurahanQueryHandler> _logger;

        public GetKelurahanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKelurahanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KelurahanDto>> Handle(GetKelurahanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KelurahanDto>("SELECT kd_prop + kd_kab + kd_kec + kd_kel AS Id, kd_prop, " +
                                                                     "kd_kab, kd_kec, kd_kel, nm_kel FROM rf07_03 WHERE " +
                                                                     "kd_prop = @kd_prop AND kd_kab = @kd_kab AND kd_kec = @kd_kec",
                    new { request.kd_prop, request.kd_kab, request.kd_kec })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}