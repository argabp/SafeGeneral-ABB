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
    public class GetKecamatanQuery : IRequest<List<KecamatanDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }

        public string kd_kab { get; set; }
    }

    public class GetKecamatanQueryHandler : IRequestHandler<GetKecamatanQuery, List<KecamatanDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKecamatanQueryHandler> _logger;

        public GetKecamatanQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKecamatanQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KecamatanDto>> Handle(GetKecamatanQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KecamatanDto>("SELECT kd_prop + kd_kab + kd_kec AS Id, kd_prop, " +
                                                                     "kd_kab, kd_kec, nm_kec FROM rf07_02 " +
                                                                     "WHERE kd_prop = @kd_prop AND kd_kab = @kd_kab",
                    new { request.kd_prop, request.kd_kab })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}