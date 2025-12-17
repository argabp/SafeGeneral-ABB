using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.TarifKebakaranOJKs.Queries
{
    public class GetDetailTarifKebakaranOJKsQuery : IRequest<List<DetailTarifKebakaranOJKDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_okup { get; set; }
    }

    public class GetDetailTarifKebakaranOJKsQueryHandler : IRequestHandler<GetDetailTarifKebakaranOJKsQuery, List<DetailTarifKebakaranOJKDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailTarifKebakaranOJKsQueryHandler> _logger;

        public GetDetailTarifKebakaranOJKsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailTarifKebakaranOJKsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailTarifKebakaranOJKDto>> Handle(GetDetailTarifKebakaranOJKsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var datas = (await _connectionFactory.Query<DetailTarifKebakaranOJKDto>("SELECT *  FROM rf11d01 WHERE kd_okup = @kd_okup",
                    new { request.kd_okup })).ToList();

                var sequence = 0;
                foreach (var item in datas)
                {
                    item.Id = sequence++;
                    sequence++;
                }

                return datas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}