using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Lookups.Queries
{
    public class GetDetailLookupsQuery : IRequest<List<DetailLookupDto>>
    {
        public string DatabaseName { get; set; }
        public string kd_lookup { get; set; }
    }

    public class GetDetailLookupsQueryHandler : IRequestHandler<GetDetailLookupsQuery, List<DetailLookupDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetDetailLookupsQueryHandler> _logger;

        public GetDetailLookupsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetDetailLookupsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DetailLookupDto>> Handle(GetDetailLookupsQuery request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);

            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                var datas = (await _connectionFactory.Query<DetailLookupDto>("SELECT * FROM MS_LookupDetail WHERE kd_lookup = @kd_lookup",
                    new { request.kd_lookup })).ToList();

                int sequence = 1;
                foreach (var detailLookupDto in datas)
                {
                    detailLookupDto.Id = sequence;
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