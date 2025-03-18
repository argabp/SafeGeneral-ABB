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
    public class GetKabupatenQuery : IRequest<List<KabupatenDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_prop { get; set; }
    }

    public class GetKabupatenQueryHandler : IRequestHandler<GetKabupatenQuery, List<KabupatenDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetKabupatenQueryHandler> _logger;

        public GetKabupatenQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetKabupatenQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<KabupatenDto>> Handle(GetKabupatenQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<KabupatenDto>("SELECT kd_prop + kd_kab AS Id, kd_prop, kd_kab, " +
                                                                     "nm_kab FROM rf07_01 WHERE kd_prop = @kd_prop",
                    new { request.kd_prop })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}