using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ABB.Application.Common.Dtos;
using ABB.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ABB.Application.Common.Queries
{
    public class GetLookupDetailsQuery : IRequest<List<DropdownOptionDto>>
    {
        public string DatabaseName { get; set; }

        public string kd_lookup { get; set; }
    }

    public class GetLookupDetailsQueryHandler : IRequestHandler<GetLookupDetailsQuery, List<DropdownOptionDto>>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<GetLookupDetailsQueryHandler> _logger;

        public GetLookupDetailsQueryHandler(IDbConnectionFactory connectionFactory, ILogger<GetLookupDetailsQueryHandler> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<List<DropdownOptionDto>> Handle(GetLookupDetailsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _connectionFactory.CreateDbConnection(request.DatabaseName);
                return (await _connectionFactory.Query<DropdownOptionDto>(
                    "SELECT nm_detail_lookup Text, no_lookup Value FROM MS_LookupDetail WHERE kd_lookup = @kd_lookup", new { request.kd_lookup })).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}